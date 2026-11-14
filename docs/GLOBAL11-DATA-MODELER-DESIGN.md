# Global11 — Oracle SQL Developer Data Modeler Design

## Project context

Global11 was deployed and operated on an office server using Oracle Global 11g. The deployment work was carried out in collaboration with **Philip Stoyanov**. The project materials also include the `Global11_demo` design for Oracle SQL Developer Data Modeler and a build procedure describing how its dimensional model was assembled.

The public references establish the technical context of the Data Modeler sample: Jeff Smith's 13 January 2014 article explains that the SQL Developer Data Modeler installation included a `global11_demo.dmd` sample and a PDF describing how the model was built. The related 2013 Oracle slide deck is titled *Dimensional Modeling in Oracle SQL Developer Data Modeler* and names Philip Stoyanov as its presenter.

- [Jeff Smith — Dimensional Modeling in Oracle SQL Developer Data Modeler (13 January 2014)](https://www.thatjeffsmith.com/archive/2014/01/dimensional-modeling-in-oracle-sql-developer-data-modeler/)
- [Philip Stoyanov — Dimensional Modeling in Oracle SQL Developer (Oracle slide deck, 2013)](https://www.slideshare.net/slideshow/dimensional-modeling-in-oracle-sql-developer/29966332)

The Data Modeler design metadata recorded in the project materials identifies the design as `global11_demo`, with creator `PSTOYANO`, creation timestamp `2012-12-12 04:59:41 UTC`, and owner design name `global11`. These metadata identify the stored design record; they do not by themselves establish the division of deployment tasks.

## Scope and environment preparation

The supplied build instructions describe the following preparation:

1. Install the Oracle Global 11g `GLOBAL` sample schema using the Oracle installation instructions and distribution archive.
2. Do **not** create an Analytic Workspace as part of this initial schema setup.
3. For Oracle 12c, grant the additional privileges specified by the project instructions:

```sql
GRANT CREATE VIEW,
      CREATE CUBE,
      CREATE CUBE DIMENSION,
      CREATE CUBE MEASURE FOLDER
TO GLOBAL;
```

4. Add `SHIP_MONTH_ID` and `DELIVERY_MONTH_ID` to `GLOBAL.UNITS_FACT`, initially populating both from `MONTH_ID`.
5. Create `GLOBAL.CUST_ACCOUNT` using CTAS to join `CUSTOMER_DIM` and `ACCOUNT` by `ACCOUNT_ID`.

The exact DDL and execution cautions are documented in [Global11_demo setup](GLOBAL11-DEMO-SETUP.md).

## Data Modeler construction workflow

The supplied build procedure describes this sequence:

1. Import the `GLOBAL` schema into SQL Developer Data Modeler.
2. Engineer the relational model into a logical model.
3. Create a dimensional model.
4. Define dimensions, including time dimensions.
5. Define levels, level keys, descriptive attributes, and calculated attributes.
6. Create level-based hierarchies by connecting a dimension to its base level and linking levels in roll-up order.
7. Define joins and child-to-parent attributes when levels are mapped to different entities.
8. Define value-based (parent-child) hierarchies using level keys, parent keys, and descriptive attributes.
9. Mark ragged / skip-level hierarchy jumps using the **New Ragged Hierarchy Link** tool.
10. Define dimension usage between cubes and dimensions, including fact-to-base-level joins and role-playing settings.
11. Define cubes, fact entities, measures, and pre-computation settings.
12. Define basic and calculated measures. Summary levels for basic measures can be documented in the model, but the supplied instructions state that they are not exported to Oracle OLAP.
13. Define calculated level attributes.
14. Export the selected dimensional model and cubes to Oracle Analytic Workspace.

### Dimensional concepts confirmed by the source material

| Concept | Meaning in the Data Modeler design |
| --- | --- |
| Dimension | Regular or time dimension containing levels and hierarchies |
| Level | Grain-bearing level mapped to an entity, with key and descriptive attributes |
| Level-based hierarchy | Roll-up path connecting levels from base grain upward |
| Value-based hierarchy | Parent-child structure defined by level key and parent key |
| Ragged hierarchy | Hierarchy with a skip-level jump, represented by a dedicated ragged link |
| Dimension usage | Link from a cube to a dimension, including fact-to-base-level join attributes |
| Role-playing dimension | Reuse of one dimension in multiple fact roles; Oracle OLAP creates a separate dimension for each role |
| Cube | Container for fact entities, measures, dimension usage, joins, and slices |
| Measure | Basic measure mapped to a fact attribute or calculated measure based on an expression |

The role-playing time setup is illustrated by `MONTH_ID`, `SHIP_MONTH_ID`, and `DELIVERY_MONTH_ID`. The supplied initialization assigns the same source value to both added columns; it demonstrates role mapping, not independent shipment and delivery event dates.

## Export to Oracle Analytic Workspace

The supplied project notes describe the Data Modeler export behavior as follows:

- If an Analytic Workspace with the requested name already exists, the export process drops it and recreates it.
- When an output filename is supplied, metadata are also exported to a file.
- File-only export still requires a connection to Oracle Database: metadata are created in the connected database session but are not committed to the database.
- **Default Schema** resolves objects in the relational / physical model that do not have a schema explicitly assigned. If no default schema is set, the schema from the active connection is used.
- The user can select which cubes are included in the export.

These are documented product/workflow behaviors, not proof that an Analytic Workspace was created during the historical deployment. The initial Global11 schema setup explicitly says not to create an AW; Data Modeler export is a separate, deliberate operation. The project documentation therefore keeps the relational deployment, dimensional design, and any actual AW export/deployment as distinct evidence states.

## Relationship to the Cube_1111 documentation

The dimensional concepts above provide the modeling vocabulary for the separate [Cube_1111 specification](model/CUBE-1111-FINAL-SPECIFICATION.md). That specification records the recovered design and submitted SQL/DDL artefacts. Its technical validation remains **Pending** until the scripts and objects are checked in the target Oracle OLAP environment.

The broader [OLAP documentation](GLOBAL11-OLAP-DOCUMENTATION.md) also distinguishes source-backed relational facts from conceptual OLAP patterns.

## Resume-ready project summary

**Oracle Global11 — Data Modeling & OLAP Architecture**

- Participated in deployment of the Oracle Global11 environment in collaboration with Philip Stoyanov.
- Worked with the Oracle SQL Developer Data Modeler dimensional-design context: relational-to-logical engineering, dimensions, levels, roll-up and ragged hierarchies, dimension usage, role-playing time dimensions, cubes, and measures.
- Documented Oracle Global 11g schema preparation, project-specific SQL changes, data structures, and the Data Modeler-to-Oracle-OLAP export workflow.
- Preserved the distinction between the historically deployed relational platform, the dimensional design, and OLAP artefacts whose runtime deployment has not been independently verified for this portfolio.

**Technology:** Oracle Database 11g / 12c · Oracle SQL Developer Data Modeler · Oracle OLAP · SQL · dimensional modeling · DMRS metadata · technical documentation.

## Evidence and attribution

- The collaboration with Philip Stoyanov is part of the project history supplied by the portfolio author.
- The Oracle slide deck is attributed to Philip Stoyanov; it is cited as a technical reference, not claimed as the author's work.
- Jeff Smith's article identifies the `global11_demo.dmd` sample and its accompanying build-description PDF in the Data Modeler sample installation.
- The design metadata and project build notes are retained as separate evidence from a live database or Analytic Workspace validation.
