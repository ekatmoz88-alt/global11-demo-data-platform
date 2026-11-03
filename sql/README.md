# SQL scripts

## Execution order

1. Install the Oracle Global 11g sample schema using the vendor-provided package and its README.
2. On Oracle 12c, execute `01_oracle12c_privileges.sql` as an authorized DBA/admin.
3. Execute `02_global11_demo_schema_changes.sql` once, after confirming the target objects/columns do not already exist.

## Safety and scope

- Scripts are transcribed from the supplied Global11_demo setup instructions.
- `02_global11_demo_schema_changes.sql` is intentionally not idempotent.
- Both role-playing month keys are initialized from `MONTH_ID`, exactly as in the supplied source.
- The CTAS creates a physical table; it is not a view and is not automatically refreshed.
- No script in this directory creates an Analytic Workspace.
- Do not execute Oracle package file `global_11g_create_cubes.sql` for this project setup.

Review [the setup guide](../docs/GLOBAL11-DEMO-SETUP.md) for validation queries and caveats.
