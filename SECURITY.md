# Data Platform Security & Sanitization Guidelines (`SECURITY.md`)

**Target:** Public data platform documentation and schema repository (`global11-demo-data-platform`) covering Oracle Data Modeler artifacts, SQLite data access, SQL scripts, and MkDocs deployment.

When reviewing Pull Requests, enforce the following security and data hygiene rules:

## 1. Data Sanitization & Confidentiality
- **Synthetic Demo Data Only:** Ensure all sample records, SQL `INSERT` statements, data dictionary examples, and Oracle source excerpts contain strictly synthetic/anonymized demo data—never real personally identifiable information (PII) or production datasets.
- **No Credentials or Connection Strings:** Flag any hardcoded database passwords, Oracle TNS strings, SQLite file paths exposing local user directories, API tokens, or private keys in `docs/`, `scripts/`, `sql/`, or `.github/workflows/`.
- **Scope Hygiene:** Ensure no unrelated internal system files or confidential artifacts from external projects are committed.

## 2. Script & CI/CD Workflow Security (`scripts/`, `.github/workflows/`)
- **Safe Scripting Practices:** Check Python/shell validation scripts for safe file handling and ensure no dynamic code execution (`eval`/`exec`) or unvalidated shell interpolation is introduced.
- **Least-Privilege GitHub Actions:** Verify that MkDocs / DMRS validation and GitHub Pages deployment workflows use minimal required permissions (`permissions: contents: read`) and pinned action versions.

## 3. Reporting Security Issues
- If you discover exposed sensitive data, credentials, or security vulnerabilities in this repository, please contact the maintainer (`@ekatmoz88-alt`) privately before opening a public issue.
