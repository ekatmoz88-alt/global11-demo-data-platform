#!/usr/bin/env python3
"""Validate source-grounded DMRS documentation before the MkDocs build.

This script checks documentation consistency only. It does not connect to Oracle
or validate physical database objects."""
from __future__ import annotations

import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
SPEC = ROOT / "docs" / "data-dictionary" / "dmrs-domains.md"
NAV = ROOT / "mkdocs.yml"

EXPECTED_COLUMNS = [
    "Domain_ID", "Domain_Name", "OVID", "Synonyms", "Logical_Type_ID",
    "Logical_Type_OVID", "T_Size", "T_Precision", "T_Scale", "Native_Type",
    "LT_Name", "DESIGN_ID", "Design_OVID", "Design_Name", "DEFAULT_VALUE",
    "UNIT_OF_MEASURE", "Char_Units",
]
REQUIRED_SECTIONS = [
    "## 1. Purpose and scope",
    "## 2. Source and notation",
    "## 3. Column specification",
    "## 4. Key definition",
    "## 5. Index inventory",
    "## 6. Candidate metadata associations",
    "## 9. Validation checklist",
    "## 10. Evidence and status register",
    "## 11. Source reference",
]

def fail(message: str) -> None:
    print(f"ERROR: {message}", file=sys.stderr)
    raise SystemExit(1)

def main() -> None:
    if not SPEC.is_file():
        fail(f"Specification not found: {SPEC.relative_to(ROOT)}")
    if not NAV.is_file():
        fail("mkdocs.yml not found")

    content = SPEC.read_text(encoding="utf-8")
    navigation = NAV.read_text(encoding="utf-8")
    missing_sections = [section for section in REQUIRED_SECTIONS if section not in content]
    if missing_sections:
        fail("Missing sections: " + ", ".join(missing_sections))

    rows = re.findall(r"^\\|\\s*\\d+\\s*\\|\\s*`([^`]+)`\\s*\\|", content, re.MULTILINE)
    if len(rows) != len(EXPECTED_COLUMNS):
        fail(f"Expected {len(EXPECTED_COLUMNS)} column rows, found {len(rows)}")
    duplicates = sorted({name for name in rows if rows.count(name) > 1})
    if duplicates:
        fail("Duplicate column rows: " + ", ".join(duplicates))
    missing = [name for name in EXPECTED_COLUMNS if name not in rows]
    unexpected = [name for name in rows if name not in EXPECTED_COLUMNS]
    if missing or unexpected:
        fail(f"Column inventory mismatch. Missing={missing}; unexpected={unexpected}")

    for token in ("DMRS_DOMAINS_PK", "DOMAINS_PK_IDX", "DOMAINS_FK_IDX", "DOMAINS_FK_IDXV1"):
        if token not in content:
            fail(f"Required key/index identifier not documented: {token}")
    if "`*`" not in content or "not translated into `NOT NULL`" not in content:
        fail("The source-marker/nullability caveat is missing")
    if "data-dictionary/dmrs-domains.md" not in navigation:
        fail("DMRS_DOMAINS specification is not listed in mkdocs.yml navigation")

    print("PASS: DMRS_DOMAINS documentation checks")
    print(f"PASS: {len(rows)} expected columns documented exactly once")
    print("PASS: required sections, key/index identifiers, evidence caveat, and navigation")

if __name__ == "__main__":
    main()