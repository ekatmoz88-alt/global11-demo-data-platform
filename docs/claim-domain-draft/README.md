# GLOBAL11 — Oracle Data Architecture & OLAP

**Status:** mixed evidence set. The relational claim-domain and API materials are design drafts; the `Cube_1111` section records extracted OLAP metadata and preserves unresolved observations.

This documentation keeps two related but distinct evidence tracks visible:

1. **Relational claim-domain design:** conceptual Oracle entities, candidate indexes, partitioning, materialized-view design, and incomplete REST/SOAP contract drafts. These are not verified against a live database or deployed services.
2. **Oracle OLAP metadata — `Cube_1111`:** metadata extracted from the analytic workspace. This evidence must not be treated as proof that the separate claim-domain design was implemented in the same database or cube.

## Documents

- [Database architecture review](database-architecture-review.md) — claim-domain entity model, index strategy, partitioning, materialized-view grain and outstanding decisions.
- [Administrator guide](administrator-guide.md) — draft operational procedures for partition inspection, archival, materialized-view refresh and statistics.
- [REST API specification](rest-api-specification.md) — known fields, conceptual mappings and missing contract decisions.
- [OpenAPI 3.1 draft](openapi-draft.yaml) — reusable request/response schemas only. Operation paths and HTTP methods are intentionally not invented because the supplied excerpt did not include them.
- [SOAP/XML integration specification](soap-integration-specification.md) — request example, field mapping and WSDL prerequisites.
- [SOAP CreateClaim request XSD](soap-create-claim-request.xsd) — request payload structure only; this is not a complete WSDL contract.
- [SQL design notes](sql-design-notes.sql) — reviewed illustrative SQL fragments. Not a migration script.

## Cube_1111 — extracted OLAP metadata

The available metadata records identify the following properties:

| Property | Recorded observation | Evidence boundary |
|---|---|---|
| Model type | Dimensional / Multidimensional | Extracted model metadata |
| Primary fact entity | `UNITS FACT` | Identified in the extracted metadata |
| Additional fact entity | `PRICE FACT` | Appears in raw logs; active model membership is not confirmed |
| Time roles | `Order_Month`, `Shipped_month`, `Delivery_month` | Three role-playing uses of the Time dimension |
| Measures | `MSR_1`, `MSR_2`, `CALC_MSR1` | Extracted measure metadata; `CALC_MSR1` uses `RANKOVER(H)` |
| Default aggregation | `SUM` | Recorded default |
| Aggregation target | `AGGREGATE CUBE_1111` | Recorded aggregation metadata |
| Sparsity | Not marked as sparse in the inspected metadata | Limited to the inspected metadata |

These observations describe the extracted metadata, not query performance, successful aggregation execution, refresh behavior, or production deployment.

## Relational claim-domain scope and evidence

The supplied materials describe a conceptual Oracle schema containing claim, person, branch, employee, document, insurance and reference-data entities. The proposed analytical view joins claims, persons, branches and claim details.

The following have **not** been confirmed against a live Oracle instance:

- Oracle version, edition, installed options and licensing;
- actual table/column names, keys, constraints and data types;
- production query workload and execution plans;
- successful creation and FAST refresh of `ARF_CASE_VIEW`;
- actual REST routes, HTTP methods, authentication and error payloads;
- SOAP namespace, binding, service address, response and fault definitions.

## Release gate

Do not run the SQL fragments as a production migration. Before implementation, confirm the target Oracle version and compare the design with the data dictionary. Oracle 11g does not support identity columns; use sequences and triggers or application-generated keys if 11g is the target.

The dashboard links to this documentation as a portfolio design and metadata-analysis case study, not as proof of a deployed production system.
