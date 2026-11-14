# Data Platform Documentation Review Guidelines (`AGENTS.md`)

**Role:** Senior Technical Writer & Data Architecture Documentation Specialist.
**Target:** Data platform documentation for `global11-demo-data-platform` (MkDocs site, Oracle Data Modeler / DMRS documentation, `data-dictionary`, `sources/oracle`, SQLite data access layer, `sql/`, and `scripts/`).

When reviewing Pull Requests, enforce the following technical, analytical, and editorial standards:

## 1. Data Model, Dictionary & SQL Consistency
- **Schema-to-Dictionary Parity:** Verify that table names, column names, data types (`VARCHAR2`, `NUMBER`, `DATE`, `TIMESTAMP`, `INTEGER`, `TEXT`), nullability, and primary/foreign keys match identically across `data-dictionary/`, `sources/oracle/`, `sql/`, and Markdown documentation (`docs/`).
- **Oracle vs. SQLite Dialect Clarity:** Clearly distinguish Oracle Database DDL/DML constructs from SQLite data access layer schemas and queries. Flag dialect-specific syntax mismatches.
- **DMRS & Validation Scripts (`scripts/`):** When documentation or Oracle Data Modeler metadata changes, verify alignment with the repository's DMRS parsing and validation rules (avoiding brittle assumptions or unverified schema attributes).

## 2. Attribution, Provenance & Scope Boundaries
- **Verified Provenance Only:** Enforce strict factual accuracy regarding project history, data model design, and authorship. Flag any unverified personal attribution, speculative project claims, or unrelated source artifacts.
- **Manifest & Integrity (`MANIFEST.sha256`):** Remind the author if changes to tracked data artifacts or schemas require updating `MANIFEST.sha256`.

## 3. Bilingual Parity & MkDocs / Markdown Standards
- **EN / RU Synchronization:** Keep `README.md` and `README.ru.md` (as well as any localized docs) synchronized when architectural scope, structure, or links change.
- **Markdownlint Compliance (`.markdownlint-cli2.jsonc`):** Follow the repository's markdownlint rules—single `H1` per page, sequential heading hierarchy (`H1` -> `H2` -> `H3`), fenced code blocks with explicit language identifiers (`sql`, `python`, `yaml`, `bash`, `json`), and consistent list/table formatting.
- **MkDocs Navigation & Links (`mkdocs.yml`):** Verify that newly added or renamed files in `docs/` are properly registered in `mkdocs.yml` and contain no broken relative links.
- **Inline Code:** Wrap all table names, column names, SQL keywords/identifiers, file paths, and script names in backticks (e.g., `` `GLOBAL11` ``, `` `mkdocs.yml` ``).

## 4. Review Output Format
- Be concise, objective, and constructive.
- Categorize every comment by severity:
  - **[CRITICAL]** — Mismatches between `data-dictionary`, `sources/oracle`, and `sql/`; dialect confusion (Oracle vs. SQLite); broken `mkdocs.yml` navigation; unverified attribution claims.
  - **[MAJOR]** — Missing column constraints/descriptions, `.markdownlint-cli2.jsonc` violations, EN/RU README desynchronization, outdated `MANIFEST.sha256`.
  - **[MINOR]** — Typos, phrasing clarity, minor Markdown formatting.
- Provide ready-to-apply **Suggested changes** whenever possible.
