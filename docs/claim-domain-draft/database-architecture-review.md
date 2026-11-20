# Database architecture review — GLOBAL11

**Document status:** Draft / design review  
**Target:** Oracle relational model and analytical layer  
**Evidence level:** Conceptual model supplied in the design discussion; no live database metadata or execution plans were supplied.

## 1. Architecture boundary

The proposal contains two distinct workloads:

1. **Transactional claim-management model (OLTP):** claims, clients, branches, employees, documents, checklist items and status history.
2. **Analytical layer (OLAP):** reporting structures derived from transactional data.

Do not assume that the proposed analytical view is already implemented or that its refresh behavior has been verified.

## 2. Conceptual entities

| Domain | Proposed entities | Notes |
|---|---|---|
| Claims | `APPLN_CLAIM`, `APPLN_DETAIL`, `APPLN_DOCUMENT`, `APPLN_STATUS_HISTORY`, `APPLN_RESPONSIBLE`, `APPLN_REMARK`, `ITEM_APPLN_CHECKLIST` | Claim lifecycle and its dependent records |
| Person and reference data | `CDB_PERSONAL`, `CDB_PASSPORT`, `CDB_ADDRESS`, `CDB_PERS_DOC`, `CDB_REF_LIST` | Client identity and reference values |
| Organization | `CDB_FIRMALCAR`, `CDB_DEP_CLAIM`, `CDB_EMP`, `CDB_APP` | Firms, branches, employees and organization applications |
| Insurance and storage | `CDB_INSURANCE`, `CDB_CORP_INS`, `CDB_DOCUMENT` | Insurance policies and document metadata |

Names are retained as supplied in the draft. They must be reconciled before creating a canonical data dictionary:

- `CDB_REF_LIST` vs `CDB_DOC_TYPE`;
- `CDB_PERS_DOC` vs `CDB_PERSONAL_DOCUMENT`;
- `CDB_CORP_INS` vs `CDB_CORPORATE_INSURANCE`;
- `CDB_APP` vs `CDB_APPLICATION`.

The model also needs an explicit relationship between `APPLN_DOCUMENT` and `CDB_DOCUMENT`. A document identifier in a checklist is not enough unless the corresponding foreign key is actually defined.

## 3. Relationship assumptions to verify

- `CDB_PERSONAL` 1:N `APPLN_CLAIM`;
- `CDB_DEP_CLAIM` 1:N `APPLN_CLAIM`;
- `APPLN_CLAIM` 1:N `APPLN_STATUS_HISTORY`;
- `APPLN_CLAIM` 1:N `APPLN_DETAIL` (not yet confirmed; this affects report aggregation);
- `APPLN_CLAIM` 1:N `APPLN_DOCUMENT`, `APPLN_REMARK` and checklist items;
- `APPLN_RESPONSIBLE` links claims and employees;
- `CDB_FIRMALCAR` 1:N `CDB_DEP_CLAIM`;
- `CDB_PERSONAL` 1:N `CDB_INSURANCE`;
- `CDB_FIRMALCAR` 1:N `CDB_CORP_INS`.

These are conceptual cardinalities, not verified constraints.

## 4. Index review

### 4.1 Claim creation date

A B-tree index on `APPLN_CLAIM(CREATED_DATE)` is a reasonable candidate when queries frequently select date ranges and the predicate is selective enough. Confirm with representative SQL, table statistics and execution plans.

### 4.2 Branch filter

A bitmap index on `APPLN_CLAIM(DEP_CLAIM_ID)` is not recommended as the default for a concurrently updated OLTP table. Low cardinality alone is not sufficient justification; bitmap locking and DML behavior can be problematic under concurrent writes.

Candidate OLTP index:

```sql
CREATE INDEX idx_appln_claim_dept
    ON appln_claim(dep_claim_id);
```

For combined person/branch lookup, choose column order from the real predicates:

```sql
-- When person_id is normally the leading filter:
CREATE INDEX idx_appln_claim_person_dept
    ON appln_claim(person_id, dep_claim_id);

-- Alternative only when branch_id is normally the leading filter:
-- CREATE INDEX idx_appln_claim_dept_person
--     ON appln_claim(dep_claim_id, person_id);
```

Do not create both variants without workload evidence. Check existing primary-key, unique and foreign-key indexes first.

### 4.3 Latest status

An index on `STATUS_CODE` alone does not identify the latest status of a claim. Status history contains multiple events per claim.

Candidate index for retrieving a claim's event history in time order:

```sql
CREATE INDEX idx_status_claim_changed
    ON appln_status_history(appln_id, changed_at);
```

The query must still select the latest event deterministically. If the current status is a frequent operational filter, consider maintaining current status separately while preserving the event history.

## 5. Partitioning of APPLN_STATUS_HISTORY

Monthly interval partitioning by `CHANGED_AT` is a candidate for a large append-oriented history table. The supplied example is not a safe migration:

- `DROP TABLE ... CASCADE CONSTRAINTS` is destructive and can affect dependent constraints;
- one historical partition contains every row before the transition boundary; it does not create one old partition per month;
- `TRUNCATE PARTITION` deletes data and is not an archival operation;
- interval-partitioning support and restrictions must be tested on the exact Oracle version and partition-key datatype;
- local/global index behavior and constraints must be designed before partition maintenance.

Use a non-destructive migration plan: create the target structure, copy and reconcile data, validate row counts and constraints, plan cutover, and retain a rollback path. Archive/export and verify a partition before any destructive truncate/drop operation.

## 6. ARF_CASE_VIEW: refresh and grain

The proposed view joins:

- `APPLN_CLAIM`;
- `CDB_PERSONAL`;
- `CDB_DEP_CLAIM`;
- `APPLN_DETAIL` using a left join.

The materialized view grain is therefore potentially **one row per claim detail**, not one row per claim, if `APPLN_DETAIL` is 1:N. This must be decided before using `COUNT(APPLN_ID)` or summing loss values.

Choose one of these models:

- **Claim-grain mart:** aggregate detail rows to one row per claim before joining to dimensions.
- **Detail-grain fact:** retain one row per detail and count distinct claims separately.

Define the meaning of estimated loss: sum of all details, one claim-level estimate, or another business measure. Do not silently duplicate a claim-level amount across detail rows.

### FAST REFRESH

The existence of MV Logs does not prove that the query supports `REFRESH FAST`. In particular:

- the detail-table log must include the join key `APPLN_ID`;
- required row identifiers and referenced columns must be present in the logs;
- outer joins, expressions and aggregate behavior must satisfy Oracle's fast-refresh restrictions;
- the complete query must be checked on the target database using `DBMS_MVIEW.EXPLAIN_MVIEW` and the Oracle-provided `utlxmv.sql` diagnostic table script.

Do not document `REFRESH FAST ON COMMIT` as operationally available until the capability check and an actual refresh test succeed. ON COMMIT refresh can add work to OLTP commit processing. For a reporting workload, scheduled `ON DEMAND` refresh or ETL may be more appropriate.

## 7. OLAP fact grain

Do not collapse all lifecycle data into a single fact without defining the grain.

| Fact | Grain |
|---|---|
| Claim fact | One row per claim |
| Status-event fact | One row per status transition |
| Claim-detail fact | One row per claim detail |
| Checklist fact | One row per checklist item or verification event, as defined by the process |

Potential dimensions include person/client, branch, employee, date and status. Slowly changing dimension behavior, historical attribution and late-arriving changes remain design decisions.

## 8. Oracle version compatibility

The conceptual DDL used `GENERATED BY DEFAULT AS IDENTITY`. Identity columns are not available in Oracle 11g. If Oracle 11g is the target, use sequences with triggers or application-generated identifiers. Do not mix 11g assumptions with later-version DDL without a compatibility pass.

## 9. Acceptance checklist

- [ ] Confirm Oracle version, edition, installed options and licensing.
- [ ] Extract actual tables, columns, data types, PK/FK/UK constraints and indexes from the data dictionary.
- [ ] Reconcile the entity-name variants listed above.
- [ ] Confirm cardinality of `APPLN_DETAIL`.
- [ ] Define document linkage and retention/audit policy.
- [ ] Capture representative SQL and execution plans before adding indexes.
- [ ] Test partition DDL and migration on the target Oracle version.
- [ ] Define archival retention and recovery procedures.
- [ ] Run `DBMS_MVIEW.EXPLAIN_MVIEW` and test FAST/COMPLETE refresh.
- [ ] Define the fact grain and validate aggregation results against source records.
- [ ] Keep all unverified design elements marked as draft.
