# DMRS Repository Metadata

**Documentation area:** Oracle SQL Developer Data Modeler repository metadata  
**Repository:** `GLOBAL11 Demo Data Platform`  
**Status:** Initial specifications in progress

## 1. Scope

This documentation area describes DMRS metadata structures represented in the supplied Oracle SQL Developer Data Modeler artifacts.

Initial scope:

- Domain definitions
- Logical data types
- Logical-to-native type mappings
- Native-to-logical type mappings
- Domain-associated values and value ranges
- Domain check-constraint metadata
- Relationships between metadata objects

The documentation is limited to evidence available from model diagrams, XML exports, database DDL, and catalog inspection. Undocumented application behavior is out of scope.

## 2. Documentation inventory

| Artifact | Purpose | Status |
| --- | --- | --- |
| [DMRS_DOMAINS](dmrs-domains.md) | Domain metadata table specification | Draft |
| DMRS_LOGICAL_TYPES | Logical type metadata specification | Planned |
| DMRS_LOGICAL_TO_NATIVE | Logical-to-native mapping specification | Planned |
| DMRS_NATIVE_TO_LOGICAL | Native-to-logical mapping specification | Planned |
| Domain value metadata | Associated values, ranges, and check-constraint specifications | Planned |
| Metadata relationship model | Verified entity and referential relationship documentation | Planned |

## 3. Evidence model

Every statement must be traceable to an identifiable source.

| Evidence class | Meaning |
| --- | --- |
| Confirmed | Directly visible in a supplied diagram, XML artifact, DDL, or catalog result |
| Inferred | Reasoned interpretation not independently verified |
| Not verified | Available evidence is insufficient for a reliable statement |

A diagram marker must not silently be converted into a physical database property. For example, `*` is not treated as `NOT NULL` unless its legend or physical DDL confirms that meaning.

## 4. Technical workflow

1. Capture metadata from the source artifact.
2. Record exact object and column names, preserving spelling and case.
3. Separate diagram evidence from physical database evidence.
4. Draft table specifications in Markdown.
5. Run automated documentation checks.
6. Build the MkDocs site.
7. Review the generated page and links.
8. Promote a document from Draft only after its verification criteria are met.

## 5. Quality gates

The Python validation script checks the initial `DMRS_DOMAINS` specification for required sections, the expected column inventory, duplicate entries, key and index identifiers, the source-marker/nullability caveat, and inclusion in MkDocs navigation.

These checks validate documentation structure and consistency. They do not connect to Oracle, inspect physical constraints, or prove that the deployed database matches the diagram.

## 6. Current limitations

The initial source is a model diagram. Physical DDL, constraint enforcement, nullability, index properties, and complete business semantics have not yet been verified.

**Release rule:** a successful documentation build does not automatically change a specification's evidence status from Draft to Final.
