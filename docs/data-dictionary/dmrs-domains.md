# DMRS_DOMAINS — Table Specification

**Document type:** Data Dictionary / Table Specification  
**Repository:** Oracle SQL Developer Data Modeler — DMRS metadata  
**Schema shown in source:** `GLOBAL`  
**Status:** Draft — diagram reviewed; physical implementation not verified

## 1. Purpose and scope

This specification records the structure of `DMRS_DOMAINS` as represented in the supplied DMRS model diagram, *Domains and Logical types.pdf*.

The document covers the displayed columns, source markers, primary-key declaration, indexes, and candidate metadata associations. It does not assert that diagram-level information is identical to the deployed database implementation.

## 2. Source and notation

The source diagram displays 17 columns, one primary-key constraint, and three indexes.

The diagram uses the markers `P`, `F`, and `*`. This specification preserves those markers as source notation:

- `P` — primary-key marker shown in the diagram.
- `F` — foreign-key marker shown in the diagram.
- `*` — marker shown in the diagram; its exact meaning is not established by the available legend.

**Nullability is not inferred from `*`.** The marker is not translated into `NOT NULL` without authoritative legend or DDL evidence.

## 3. Column specification

| # | Column | Data type | Source marker | Source-grounded description |
|---:|---|---|---|---|
| 1 | `Domain_ID` | `VARCHAR2(70)` | P, * | Domain identifier; marked as the primary-key column |
| 2 | `Domain_Name` | `VARCHAR2(256)` | * | Domain name |
| 3 | `OVID` | `VARCHAR2(36)` | * | Attribute named OVID; detailed semantics require confirmation |
| 4 | `Synonyms` | `VARCHAR2(4000)` | — | Synonyms metadata |
| 5 | `Logical_Type_ID` | `VARCHAR2(70)` | * | Logical type identifier |
| 6 | `Logical_Type_OVID` | `VARCHAR2(36)` | F, * | Logical-type OVID reference attribute; target requires DDL verification |
| 7 | `T_Size` | `NUMBER` | — | Type-size attribute |
| 8 | `T_Precision` | `NUMBER` | — | Type-precision attribute |
| 9 | `T_Scale` | `NUMBER` | — | Type-scale attribute |
| 10 | `Native_Type` | `VARCHAR2(60)` | — | Native type attribute |
| 11 | `LT_Name` | `VARCHAR2(256)` | * | Logical type name attribute |
| 12 | `DESIGN_ID` | `VARCHAR2(70)` | * | Design identifier |
| 13 | `Design_OVID` | `VARCHAR2(36)` | F, * | Design OVID reference attribute; target requires DDL verification |
| 14 | `Design_Name` | `VARCHAR2(256)` | * | Design name |
| 15 | `DEFAULT_VALUE` | `VARCHAR2(256)` | — | Default-value metadata |
| 16 | `UNIT_OF_MEASURE` | `VARCHAR2(30)` | — | Unit-of-measure metadata |
| 17 | `Char_Units` | `CHAR(4)` | — | Character-unit metadata |

Column spelling, capitalization, and data types are reproduced from the diagram. In particular, `DESIGN_ID`, `Design_OVID`, and `Design_Name` are not normalized.

## 4. Key definition

### 4.1 Primary key

| Constraint | Column |
|---|---|
| `DMRS_DOMAINS_PK` | `Domain_ID` |

The diagram displays `DMRS_DOMAINS_PK (Domain_ID)` and marks `Domain_ID` with `P`.

The physical constraint definition and enforcement state have not yet been checked against database DDL.

## 5. Index inventory

| Index name | Indexed column(s) |
|---|---|
| `DOMAINS_PK_IDX` | `Domain_ID` |
| `DOMAINS_FK_IDX` | `Design_OVID` |
| `DOMAINS_FK_IDXV1` | `Logical_Type_OVID` |

These objects are recorded as indexes because that is how they are identified in the source diagram. An index name containing `FK` is not, by itself, proof of a physical foreign-key constraint. Uniqueness, index type, tablespace, and column ordering require DDL or catalog verification.

## 6. Candidate metadata associations

The diagram marks `Design_OVID` and `Logical_Type_OVID` with `F`. The repository model also contains the related table names `DMRS_DESIGNS` and `DMRS_LOGICAL_TYPES`.

| Source column | Candidate target | Diagram evidence | Status |
|---|---|---|---|
| `Design_OVID` | `DMRS_DESIGNS.Design_OVID` | `F` marker; design-related metadata table appears in the source model | Candidate only; verify constraint and target column |
| `Logical_Type_OVID` | `DMRS_LOGICAL_TYPES.OVID` | `F` marker; logical-type metadata table appears in the source model | Candidate only; verify constraint and target column |

The candidate target columns above are hypotheses based on the naming and corresponding table attributes. They must not be treated as confirmed physical foreign-key definitions until checked against DDL or Oracle catalog metadata.

## 7. Attribute groups — descriptive classification

For documentation navigation, the columns can be grouped by their names:

- **Domain identity:** `Domain_ID`, `Domain_Name`, `OVID`.
- **Logical-type attributes:** `Logical_Type_ID`, `Logical_Type_OVID`, `LT_Name`.
- **Type characteristics:** `T_Size`, `T_Precision`, `T_Scale`, `Native_Type`.
- **Design attributes:** `DESIGN_ID`, `Design_OVID`, `Design_Name`.
- **Additional metadata:** `Synonyms`, `DEFAULT_VALUE`, `UNIT_OF_MEASURE`, `Char_Units`.

This grouping is a documentation aid derived from column names. It does not define business rules or runtime behavior.

## 8. Related domain metadata tables

The same source diagram includes the following domain-related tables:

- `DMRS_DOMAIN_AVT`
- `DMRS_DOMAIN_VALUE_RANGES`
- `DMRS_DOMAIN_CHECK_CONSTRAINTS`

Their names suggest separate representations of domain-associated values, value ranges, and check-constraint text. The exact semantics, cardinalities, and physical relationships are not specified here; each requires its own table-level inspection and DDL verification.

## 9. Validation checklist

- [ ] Retrieve physical DDL for `GLOBAL.DMRS_DOMAINS`.
- [ ] Verify the primary-key constraint and its backing index.
- [ ] Verify whether foreign-key constraints exist for `Design_OVID` and `Logical_Type_OVID`.
- [ ] Confirm the exact referenced tables and columns.
- [ ] Inspect nullability for all 17 columns.
- [ ] Verify index uniqueness, type, column order, and storage properties.
- [ ] Confirm the meaning of `OVID` in this repository version.
- [ ] Obtain the diagram legend and establish the meaning of `*`.
- [ ] Verify the semantics and permitted values of `T_Size`, `T_Precision`, `T_Scale`, `Char_Units`, and `UNIT_OF_MEASURE`.
- [ ] Inspect the DDL and relationship definitions for the three related domain metadata tables.

## 10. Evidence and status register

| Item | Evidence status |
|---|---|
| 17 column names and displayed data types | Confirmed from source diagram |
| `DMRS_DOMAINS_PK` and its displayed column | Confirmed from source diagram |
| Three index names and displayed columns | Confirmed from source diagram |
| `F` markers on the two OVID columns | Confirmed from source diagram |
| Candidate FK targets | Inferred; physical definitions not verified |
| Nullability | Not verified |
| Physical DDL and index properties | Not verified |
| Business semantics and validation rules | Not verified |
| Meaning of `*` | Not verified |

## 11. Source reference

- *Domains and Logical types.pdf* — supplied one-page DMRS model diagram.

**Document status:** Draft  
**Exit criterion for Final:** Complete physical DDL and relationship verification, resolve the source-marker legend, and review the domain-specific attribute semantics.
