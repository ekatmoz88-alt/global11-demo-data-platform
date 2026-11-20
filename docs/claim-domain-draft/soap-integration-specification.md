# SOAP/XML integration specification — GLOBAL11 claim domain

**Status:** integration design draft. The supplied XML fragment is not a complete SOAP contract.

## 1. Supplied request payload

The source material contains the following business fields:

```xml
<sch:CreateClaimRequest>
  <sch:PersonId>1250</sch:PersonId>
  <sch:DeptClaimId>8</sch:DeptClaimId>
  <sch:ClaimNumber>CLM-2026-00891</sch:ClaimNumber>
</sch:CreateClaimRequest>
```

The [XSD fragment](soap-create-claim-request.xsd) models these three elements. Its namespace `urn:global11:claim` is explicitly provisional and must be replaced with the namespace from the real service contract.

## 2. Proposed field mapping

| XML element | Conceptual database field | Notes |
|---|---|---|
| `PersonId` | `APPLN_CLAIM.PERSON_ID` | Client/person reference |
| `DeptClaimId` | `APPLN_CLAIM.DEP_CLAIM_ID` | Branch reference |
| `ClaimNumber` | `APPLN_CLAIM.CLAIM_NUMBER` | Business claim number |

The mapping is conceptual until checked against the actual service implementation and database constraints.

## 3. WSDL components still required

A complete WSDL cannot be generated from the request body alone. The following information is missing:

- verified target namespace and XML prefix conventions;
- service and port names;
- SOAP 1.1 or SOAP 1.2 binding;
- operation/action name and transport;
- endpoint address;
- response message and response XSD;
- SOAP Fault structure and error codes;
- authentication and transport-security requirements;
- timeout, retry and idempotency behavior.

## 4. Contract validation

When the source WSDL becomes available:

1. compare request and response messages with the service code;
2. validate XML samples against the authoritative XSD;
3. confirm SOAP version, binding and action;
4. test success and fault responses;
5. verify field mapping and referential constraints;
6. publish the full WSDL/XSD bundle only after the service endpoint and namespace are confirmed.

Do not publish the provisional namespace or example identifiers as production values.
