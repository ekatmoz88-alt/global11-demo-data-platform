# GLOBAL11 Demo Data Platform

Documentation and reproducible analysis of the **Global11_demo Data Modeler** learning project based on the Oracle Global 11g sample schema.

This repository is dedicated to the Oracle relational model, its documented demonstration changes, the data dictionary, and conceptual OLAP artefacts. It does not document the separate Deshifrator WPF application suite.

## Scope

- Oracle Global 11g sample schema and the `GLOBAL` relational model.
- Data dictionary extracted from the supplied project materials.
- Role-playing time-key demonstration in `GLOBAL.UNITS_FACT`.
- CTAS creation of `CUST_ACCOUNT`.
- Conceptual OLAP model inventory and diagrams.

## Repository structure

- `docs/GLOBAL11-OLAP-DOCUMENTATION.md` — OLAP documentation and conceptual patterns.
- `docs/GLOBAL11-DEMO-SETUP.md` — setup sequence and documented schema changes.
- `docs/model/GLOBAL11-RELATIONAL-MODEL.md` — relational tables, relationships, and interpretation boundaries.
- `docs/sources/ORACLE-GLOBAL11-SOURCES.md` — Oracle package inventory and source references.
- `data-dictionary/` — extracted data dictionary.
- `sql/` — reviewed SQL scripts and execution notes.
- `sources/oracle/` — supplied Oracle Data Modeler archive.

## Quick start

1. Prepare the Oracle Global 11g sample schema using the official README and distribution referenced in [sources](docs/sources/ORACLE-GLOBAL11-SOURCES.md).
2. Install the `GLOBAL` schema using its documented installation procedure.
3. For Oracle 12c, apply the additional privileges in [01_oracle12c_privileges.sql](sql/01_oracle12c_privileges.sql).
4. Apply [02_global11_demo_schema_changes.sql](sql/02_global11_demo_schema_changes.sql) once.
5. Do not run `global_11g_create_cubes.sql` for this scenario: the project instructions explicitly exclude Analytic Workspace creation.

## Confirmed demonstration changes

### Role-playing time keys

Two columns are added to `GLOBAL.UNITS_FACT`:

- `SHIP_MONTH_ID VARCHAR2(7)`
- `DELIVERY_MONTH_ID VARCHAR2(7)`

The supplied instruction populates both from `MONTH_ID`. This demonstrates two roles for one time dimension; it does not provide independent shipment and delivery dates.

### CUST_ACCOUNT

A physical table is created using `CREATE TABLE ... AS SELECT`, joining `CUSTOMER_DIM` and `ACCOUNT` by `ACCOUNT_ID`. This is a CTAS table, not a view.

## Evidence and limitations

The primary evidence is the supplied project instruction and the Oracle Global 11g archive (README and SQL scripts). The documentation distinguishes source facts, archive inventory, conceptual OLAP patterns, and items requiring verification against a live database.

Execution in a specific Oracle installation is not claimed. Before running scripts, verify the database version, object availability, privileges, and whether the scripts have already been applied.

- The schema-change scripts are intended for one-time application and are not idempotent.
- Both role-playing columns initially receive the same `MONTH_ID`.
- CTAS does not automatically reproduce all constraints, indexes, triggers, or later source-table changes.
- Oracle 12c privileges are separate from the base schema installation.
- OLAP artefacts in the documentation do not mean that an Analytic Workspace is created or deployed.

## Project boundary

Deshifrator, ModuleMobile, ModuleStationary, PassportPaint, SQLite, and their C# source excerpts are documented separately in the [Deshifrator documentation repository](https://github.com/ekatmoz88-alt/deshifrator-docs). No technical relationship between that application suite and the Oracle GLOBAL11 model is asserted.
