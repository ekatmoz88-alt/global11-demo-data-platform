-- Global11_demo: additional privileges for Oracle 12c
-- Source: project setup instructions supplied for this repository.
-- Run as a DBA/admin account with authority to grant these privileges.
-- These are additional project-specific grants, separate from the base
-- grants performed by the Oracle Global 11g installation script.

GRANT CREATE VIEW,
      CREATE CUBE,
      CREATE CUBE DIMENSION,
      CREATE CUBE MEASURE FOLDER
TO GLOBAL;
