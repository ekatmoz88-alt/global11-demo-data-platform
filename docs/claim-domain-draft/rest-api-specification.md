# REST API specification — GLOBAL11 claim domain

**Status:** contract draft / incomplete. This document does not describe a confirmed deployed API.

## 1. Scope

The supplied fragment provides:

- a claim creation request schema with `person_id`, `dep_claim_id` and `claim_number`;
- a claim response shape with `appln_id`, identifiers, claim number and `created_date`;
- a checklist update body with `is_checked`, `emp_id` and optional `document_id`;
- two path parameters named `id` and `itemId`.

The excerpt does not include the full `paths` mapping, HTTP method, route template, authentication scheme, complete success response or error model. These details are intentionally not guessed.

## 2. Reusable schemas

The machine-readable draft is [openapi-draft.yaml](openapi-draft.yaml). It defines:

| Schema | Purpose |
|---|---|
| `ClaimCreate` | Required client, branch and claim number fields |
| `ClaimResponse` | Claim identifier and returned claim data |
| `ChecklistItemUpdate` | Checklist state, employee and optional document |
| `ClaimId` | Integer path parameter named `id` |
| `ChecklistItemId` | Integer path parameter named `itemId` |

The supplied representation of `is_checked` is integer `0` or `1`. Do not change it to JSON boolean until the implementation is inspected.

## 3. Contract questions required before endpoint publication

1. What is the exact route for claim creation?
2. What HTTP method is used to update a checklist item?
3. Do `id` and `itemId` refer to `appln_id` and `checklist_item_id`?
4. Is `emp_id` required by the service even though the conceptual table allowed it to be nullable?
5. Does `document_id` reference `CDB_DOCUMENT.DOCUMENT_ID`?
6. What are the exact success payloads and error response bodies?
7. How are authentication, authorization and idempotency handled?
8. Are timestamps UTC, offset-aware, or local database timestamps?

## 4. Data mapping

| API field | Proposed database field | Status |
|---|---|---|
| `person_id` | `APPLN_CLAIM.PERSON_ID` | Conceptual mapping |
| `dep_claim_id` | `APPLN_CLAIM.DEP_CLAIM_ID` | Conceptual mapping |
| `claim_number` | `APPLN_CLAIM.CLAIM_NUMBER` | Conceptual mapping |
| `appln_id` | `APPLN_CLAIM.APPLN_ID` | Conceptual mapping |
| `emp_id` | `ITEM_APPLN_CHECKLIST.EMP_ID` | Conceptual mapping |
| `document_id` | `ITEM_APPLN_CHECKLIST.DOCUMENT_ID` | Requires FK verification |

## 5. Release gate

Before treating this as a production OpenAPI contract, add the verified route and method, request/response examples, security scheme, error responses, and implementation-backed field constraints. Then run Spectral and compare the contract against service tests or source code.
