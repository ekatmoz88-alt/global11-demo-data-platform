# Global11_demo / Cube_1111 — Final OLAP Specification

**Documentation Status:** Documentation Complete  
**Technical Validation:** Pending  
**Target environment:** Oracle AW 11gR2 / JDBC connection `Global_11r21`

This document consolidates the final OLAP design summary, the submitted DDL/SQL artifacts, and the Mermaid visualization for `Global11_demo / Cube_1111`. It records the documented design; it does not certify successful execution in Oracle Analytic Workspace.

## 1. Completion summary

| Stage | Status | Notes |
|:---|:---|:---|
| Logical design (I.1–I.2, II.1–II.10) | Complete | Time roles, ragged hierarchies, value-based structures, links |
| Measures and attributes (II.12–II.13) | Complete | Basic mapping, calculated measures (RANK/SUM), calculated attribute (address); Summary Levels are not exported to AW |
| Export and pre-compute (II.11, II.14) | Documented | Recreate AW, JDBC `Global_11r21`, Populate Cubes |
| DDL / SQL scripts | Prepared | `DBMS_AW.EXECUTE` blocks and SQL validation query |
| Visualization | Added | Mermaid OLAP cube diagram |
| Documentation | Complete | Documentation package is ready for handoff |
| Technical validation | Pending | Requires script execution and object inspection in the target Oracle AW environment |

## 2. Submitted relational preparation DDL

The following is the DDL supplied for the final `Cube_1111` design. It is retained as submitted; object names, column mappings, and compatibility must be checked against the actual target schema before execution.

```sql
ALTER TABLE GLOBAL.UNITS_FACT ADD (SHIP_MONTH_ID VARCHAR2(20), DELIVERY_MONTH_ID VARCHAR2(20));
UPDATE GLOBAL.UNITS_FACT SET SHIP_MONTH_ID = MONTH_ID, DELIVERY_MONTH_ID = MONTH_ID;

CREATE TABLE GLOBAL.cust_account AS
SELECT c.DIM_ACCOUNT_ID, c.GLOBAL_CUSTOMER_DIM_SHIP_TO_ID,
       a.GLOBAL_ACCOUNT_FIRSTNAME, a.GLOBAL_ACCOUNT_LASTNAME,
       a.GLOBAL_ACCOUNT_ADDRESS1, a.GLOBAL_ACCOUNT_ADDRESS2,
       a.GLOBAL_ACCOUNT_CITY, a.GLOBAL_ACCOUNT_STATE_PROV,
       a.GLOBAL_ACCOUNT_COUNTRY, a.GLOBAL_ACCOUNT_ZIP_PC
FROM GLOBAL.GLOBAL_CUSTOMER_DIM c
JOIN GLOBAL.GLOBAL_ACCOUNT a
  ON c.DIM_ACCOUNT_ID = a.GLOBAL_ACCOUNT_ACCOUNT_ID;
```

## 3. Submitted Oracle Analytic Workspace DDL

```sql
BEGIN
  DBMS_AW.EXECUTE('AW CREATE GLOBAL_AW');
END;
/

BEGIN
  DBMS_AW.EXECUTE('
    DEFINE TIME_DIM DIMENSION TEXT
    HIERARCHY CALENDAR (MONTH ROLLUP TO QUARTER ROLLUP TO YEAR)
    ATTRIBUTE TIME_SPAN INTEGER
    ATTRIBUTE TIME_END DATE
  ');
END;
/

BEGIN
  DBMS_AW.EXECUTE('
    DEFINE CUSTOMER_DIM DIMENSION TEXT
    HIERARCHY SHIP_TO_COUNTRY_HIER (
      SHIP_TO ROLLUP TO CITY
      SKIP LEVEL STATE_PROVINCE SKIP LEVEL COUNTRY
      ROLLUP TO WAREHOUSE
    )
  ');
END;
/

BEGIN
  DBMS_AW.EXECUTE('DEFINE PRODUCT_DIM DIMENSION TEXT HIERARCHY PROD_HIER (ITEM ROLLUP TO CATEGORY)');
END;
/

BEGIN
  DBMS_AW.EXECUTE('
    DEFINE FULL_ADDRESS ATTRIBUTE TEXT <CUSTOMER_DIM>
    FULL_ADDRESS = IF GLOBAL_ACCOUNT_ADDRESS2 IS NA THEN GLOBAL_ACCOUNT_ADDRESS1
                   ELSE GLOBAL_ACCOUNT_ADDRESS1 || '', '' || GLOBAL_ACCOUNT_ADDRESS2
    PROPERTY ''ATTRIBUTE_TYPE'' ''CALCULATED''
  ');
END;
/

BEGIN
  DBMS_AW.EXECUTE('
    DEFINE CUBE_1111 CUBE <TIME_DIM CUSTOMER_DIM PRODUCT_DIM>
    MEASURES (MSR_1, MSR_2, CALC_MSR1)

    DEFINE MSR_1 VARIABLE DECIMAL <CUBE_1111>
    MSR_1 = GLOBAL.UNITS_FACT.UNITS
    PROPERTY ''AGGREGATION'' ''SUM''

    DEFINE MSR_2 VARIABLE DECIMAL <CUBE_1111>
    MSR_2 = TOTAL(MSR_1)
    PROPERTY ''MEASURE_TYPE'' ''CALCULATED''

    DEFINE CALC_MSR1 VARIABLE INTEGER <CUBE_1111>
    CALC_MSR1 = RANK(MSR_1 DESC WITHIN SHIP_TO_COUNTRY_HIER)
    PROPERTY ''MEASURE_TYPE'' ''CALCULATED''
    PROPERTY ''AGGREGATION'' ''NONE''
  ');
END;
/

BEGIN
  DBMS_AW.EXECUTE('
    AGGREGATE CUBE_1111 BY TIME_DIM, CUSTOMER_DIM
    UPDATE GLOBAL_AW
  ');
END;
/
COMMIT;
```

## 4. SQL validation query

```sql
WITH base_agg AS (
  SELECT f.R_SHIP_TO_ID, cd.SHIP_TO_DSC, ca.GLOBAL_ACCOUNT_COUNTRY AS COUNTRY,
         SUM(f.UNITS) AS MSR_1
  FROM GLOBAL.UNITS_FACT f
  JOIN GLOBAL.CUSTOMER_DIM cd ON f.R_SHIP_TO_ID = cd.SHIP_TO_ID
  LEFT JOIN GLOBAL.cust_account ca ON cd.DIM_ACCOUNT_ID = ca.DIM_ACCOUNT_ID
  GROUP BY f.R_SHIP_TO_ID, cd.SHIP_TO_DSC, ca.GLOBAL_ACCOUNT_COUNTRY
)
SELECT R_SHIP_TO_ID, SHIP_TO_DSC, COUNTRY, MSR_1,
       RANK() OVER (PARTITION BY COUNTRY ORDER BY MSR_1 DESC) AS CALC_MSR1
FROM base_agg
ORDER BY COUNTRY, CALC_MSR1;
```

This query is a relational validation analogue for the aggregation and ranking intent. Its successful execution and equivalence to the AW hierarchy calculation have not yet been confirmed.

## 5. Requirement checklist

| Item | Description | Documented status |
|:---|:---|:---|
| I.1 | Role-playing time columns (three-month roles) | Complete — `MONTH_ID`, `SHIP_MONTH_ID`, `DELIVERY_MONTH_ID` |
| I.2 | `cust_account` join | Complete in design — dimension/account consolidation |
| II.4 | Time dimension, rollup, span/end | Complete in design — attributes defined |
| II.8 | Value-based hierarchy | Complete in design — level key / parent key concept |
| II.9 | Ragged / skip-level hierarchy | Complete in design — `SKIP LEVEL` |
| II.10 | Dimension usage / role playing | Complete in design — time roles and `R_SHIP_TO_ID` |
| II.11 | Cube and pre-computation | Complete in documentation — `AGGREGATE CUBE_1111` |
| II.12a | Basic measure mapped to fact attribute | Complete in design — `MSR_1 = UNITS_FACT.UNITS` |
| II.12b | Summary Levels not included in AW | Recorded — not included in submitted AW DDL |
| II.12c | Calculated measures | Complete in design — `MSR_2`, `CALC_MSR1` |
| II.13 | Calculated attribute | Complete in design — `FULL_ADDRESS` |
| II.14 | Export to Oracle AW | Documented — Recreate AW, JDBC `Global_11r21` |

## 6. Technical validation boundary

**Technical Validation remains Pending.** Before treating the scripts as executable or the cube as deployed, the target Oracle environment must be used to verify:

- that the submitted source tables and columns exist and the joins use the correct keys;
- that the role-playing time columns and dimension mappings correspond to the actual fact data;
- that the AW DDL syntax and object definitions are accepted by the target Oracle OLAP version;
- that the cube measures, hierarchy references, calculated attribute, aggregation, and pre-computation behave as intended;
- that the relational SQL validation query executes and produces results consistent with the intended hierarchy calculation.

The documentation completion status is independent of this runtime validation. No successful AW deployment or script execution is asserted by this document.

## 7. Diagram

The final high-level visualization is maintained separately in [`olap-diagram.mmd`](olap-diagram.mmd).
