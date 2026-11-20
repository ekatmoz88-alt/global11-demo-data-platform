# Administrator guide — GLOBAL11 Oracle analytical layer

**Status:** Draft operational guidance. Commands below are templates, not authorization to modify production data. Confirm schema ownership, Oracle version, partition names, retention policy and backup status before execution.

## 1. Preconditions

Before running maintenance:

1. Confirm the target database and schema.
2. Confirm the Oracle version and partitioning feature availability.
3. Confirm the approved maintenance window and retention policy.
4. Verify backup/recovery readiness.
5. Capture current row counts, partition metadata and index status.
6. Test the procedure in a non-production environment.

Never run `DROP TABLE ... CASCADE CONSTRAINTS` as a routine partition-maintenance step.

## 2. Inspect partition metadata

List partitions for the history table:

```sql
SELECT partition_name, partition_position, tablespace_name
FROM user_tab_partitions
WHERE table_name = 'APPLN_STATUS_HISTORY'
ORDER BY partition_position;
```

The `HIGH_VALUE` column in Oracle dictionary views is a LONG value in relevant releases and is not convenient for ordinary filtering/reporting. Use a version-appropriate metadata inspection method when exact boundaries are required.

Check the table's partitioning metadata:

```sql
SELECT table_name, partitioning_type, interval
FROM user_part_tables
WHERE table_name = 'APPLN_STATUS_HISTORY';
```

## 3. Archive lifecycle

Partition maintenance is not equivalent to archiving. Use this sequence:

1. Identify the exact partition and date range.
2. Confirm the retention cutoff with the data owner.
3. Export or move the partition data to the approved archive target.
4. Record source partition name, boundary, row count, archive location and execution timestamp.
5. Validate archive row counts and sample records.
6. Obtain the required operational approval.
7. Only then perform the approved partition drop/truncate operation.
8. Verify remaining data, local/global index status and application behavior.

Example destructive operation — execute only after the preceding controls and approval:

```sql
-- DESTRUCTIVE: removes all rows from this partition.
ALTER TABLE appln_status_history
    TRUNCATE PARTITION p_historical;
```

Do not use the example partition name unless it exists and the intended boundary has been confirmed. For archival retention, evaluate `EXCHANGE PARTITION`, partition export or a controlled copy process rather than treating truncate as archive.

## 4. Materialized view refresh

The proposed view is `ARF_CASE_VIEW`. Its FAST refresh capability is not yet proven.

Inspect status and refresh metadata using the relevant dictionary views for the target Oracle version. A manual FAST refresh can be requested as follows:

```sql
BEGIN
    DBMS_MVIEW.REFRESH(
        list   => 'ARF_CASE_VIEW',
        method => 'F'
    );
END;
/
```

A complete refresh template:

```sql
BEGIN
    DBMS_MVIEW.REFRESH(
        list           => 'ARF_CASE_VIEW',
        method         => 'C',
        atomic_refresh => TRUE
    );
END;
/
```

If FAST refresh fails, do not automatically switch to COMPLETE in production. Capture the error, verify MV Logs and refresh capabilities, estimate the full-refresh workload, and obtain approval.

For initial capability analysis, use Oracle's `utlxmv.sql` to create `MV_CAPABILITIES_TABLE`, then run `DBMS_MVIEW.EXPLAIN_MVIEW` against the complete MV query. Review the results before enabling a scheduled refresh.

## 5. Gather optimizer statistics

After a successful refresh, collect statistics according to the database's statistics policy:

```sql
BEGIN
    DBMS_STATS.GATHER_TABLE_STATS(
        ownname => USER,
        tabname => 'ARF_CASE_VIEW'
    );
END;
/
```

For partitioned base tables, coordinate table, partition and global statistics collection with the DBA policy. Avoid forcing full statistics collection after every small refresh without measuring its cost.

## 6. Monitoring and incident record

For each maintenance execution, record:

- database and schema;
- operator and approval reference;
- operation and target object/partition;
- start/end time;
- source and archive row counts;
- MV refresh method, duration and result;
- statistics collection result;
- warnings, errors and recovery actions.

## 7. Outstanding decisions

- Oracle version/edition and partitioning availability;
- monthly retention period and archive destination;
- partition naming convention and historical migration strategy;
- local/global index design and post-maintenance validation;
- MV refresh schedule and acceptable data latency;
- FAST refresh eligibility of the final query;
- production alerting and recovery procedures.
