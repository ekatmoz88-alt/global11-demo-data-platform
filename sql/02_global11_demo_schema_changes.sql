-- Global11_demo: relational schema changes
-- Source: project setup instructions supplied for this repository.
--
-- IMPORTANT:
--   * Run only after the Oracle Global 11g GLOBAL schema is installed.
--   * This script is a one-time change set; it is NOT idempotent.
--   * Verify that the columns and CUST_ACCOUNT table do not already exist.
--   * The source instructions initialize both role keys from MONTH_ID.
--     This project change set initializes both role keys from MONTH_ID;
--     it does not reconstruct independent shipment/delivery event dates.
--   * CUST_ACCOUNT is a physical CTAS table, not a view.

-- 1. Add role-playing time keys to UNITS_FACT.
ALTER TABLE GLOBAL.UNITS_FACT ADD SHIP_MONTH_ID VARCHAR2(7);
ALTER TABLE GLOBAL.UNITS_FACT ADD DELIVERY_MONTH_ID VARCHAR2(7);

-- 2. Initialize both role keys from the existing month key.
UPDATE GLOBAL.UNITS_FACT SET SHIP_MONTH_ID = MONTH_ID;
UPDATE GLOBAL.UNITS_FACT SET DELIVERY_MONTH_ID = MONTH_ID;

-- 3. Materialize customer ship-to and account attributes.
CREATE TABLE GLOBAL.CUST_ACCOUNT AS
SELECT
    GLOBAL.CUSTOMER_DIM.SHIP_TO_ID,
    GLOBAL.CUSTOMER_DIM.SHIP_TO_DSC,
    GLOBAL.CUSTOMER_DIM.SHIP_TO_DSC_FRENCH,
    GLOBAL.CUSTOMER_DIM.SHIP_TO_DSC_DUTCH,
    GLOBAL.ACCOUNT.ACCOUNT_ID,
    GLOBAL.ACCOUNT.FIRSTNAME,
    GLOBAL.ACCOUNT.LASTNAME,
    GLOBAL.ACCOUNT.ADDRESS,
    GLOBAL.ACCOUNT.ADDRESS2,
    GLOBAL.ACCOUNT.CITY,
    GLOBAL.ACCOUNT.STATE_PROV,
    GLOBAL.ACCOUNT.COUNTRY,
    GLOBAL.ACCOUNT.ZIP_PC,
    GLOBAL.ACCOUNT.PHONE,
    GLOBAL.ACCOUNT.FAX,
    GLOBAL.ACCOUNT.EMAIL
FROM GLOBAL.CUSTOMER_DIM
INNER JOIN GLOBAL.ACCOUNT
    ON GLOBAL.CUSTOMER_DIM.ACCOUNT_ID = GLOBAL.ACCOUNT.ACCOUNT_ID;

-- Review validation queries in docs/GLOBAL11-DEMO-SETUP.md before
-- considering this change set complete.
