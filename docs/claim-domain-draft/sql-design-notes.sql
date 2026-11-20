-- GLOBAL11 Oracle OLAP design notes
-- STATUS: illustrative fragments only; NOT a production migration.
-- Confirm Oracle version, schema, existing indexes, constraints and workload first.

-- 1. Candidate B-tree indexes for transactional claim access.
-- Do not create duplicates of existing PK/FK/UK indexes.
CREATE INDEX idx_appln_claim_created
    ON appln_claim(created_date);

CREATE INDEX idx_appln_claim_dept
    ON appln_claim(dep_claim_id);

-- Choose ONE composite order based on actual predicates and execution plans:
-- CREATE INDEX idx_appln_claim_person_dept
--     ON appln_claim(person_id, dep_claim_id);
-- CREATE INDEX idx_appln_claim_dept_person
--     ON appln_claim(dep_claim_id, person_id);

-- 2. Candidate status-history access path.
CREATE INDEX idx_status_claim_changed
    ON appln_status_history(appln_id, changed_at);

-- 3. The supplied MV log for APPLN_DETAIL omitted the join key APPLN_ID.
-- This is a design correction to evaluate, not a guarantee of FAST REFRESH.
-- Existing MV logs must be inspected before attempting to replace them.
-- CREATE MATERIALIZED VIEW LOG ON appln_detail
--   WITH ROWID, SEQUENCE (appln_id, estimated_loss, incident_date)
--   INCLUDING NEW VALUES;

-- 4. Do not drop/recreate the existing history table as a shortcut.
-- Test interval partitioning and TIMESTAMP key compatibility on the target
-- Oracle release before writing the migration. Oracle 11g has no IDENTITY
-- columns; use sequences/triggers or application-generated identifiers.

-- 5. MV capability analysis:
-- Run Oracle's utlxmv.sql to create MV_CAPABILITIES_TABLE, then:
-- BEGIN
--   DBMS_MVIEW.EXPLAIN_MVIEW('ARF_CASE_VIEW');
-- END;
-- /
-- Inspect MV_CAPABILITIES_TABLE before claiming REFRESH FAST support.

-- 6. Report grain must be decided before aggregating:
-- Claim grain: aggregate APPLN_DETAIL to one row per APPLN_ID first.
-- Detail grain: count distinct APPLN_ID when reporting number of claims.
-- Never sum duplicated claim-level measures across detail rows.
