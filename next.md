
As of 1.17.5 of Vault, lets check all Auth and Secret backends for consumer operations

Auth Methods

0. SAML is missing. So add it. Enterprise.SAML

1. AliCloud

Login is good.
Admin is also good.


Secret Backends

Everything seems to be there. Cool.


From Vault Changelog, after 1.13.0

1.13.1 https://github.com/hashicorp/vault/blob/main/CHANGELOG.md#1131
Looks good

1.13.2

0. sys/config/sanitize api
1. sdk/ldaputil: added connection_timeout to tune connection timeout duration for all LDAP plugins. [GH-20144]
2. auth/ldap: Add max_page_size configurable to LDAP configuration [GH-19032]

Done

1.13.3

1. audit: add a mount_point field to audit requests and response entries [GH-20411]
2. /v1/sys/decode-token new api core: Add possibility to decode a generated encoded root token via the rest API [GH-20595]
3. secrets/pki: add subject key identifier to read key response [GH-20642]
4. secrets/pki: Include per-issuer enable_aia_url_templating in issuer read endpoint. [GH-20354]

Done

1.13.4

1. sys/internal/api enhancements. So do the sys/internal apis for billing etc.

Done

1.13.5

1. secrets/pki: Support setting both maintain_stored_certificate_counts=false and publish_stored_certificate_count_metrics=false explicitly in tidy config. [GH-20664]

Done

1.13.6
Looks good

1.13.7

1. ** Merkle Tree Corruption Detection (enterprise) **: Add a new endpoint to check merkle tree corruption.
Done

1.13.8
Looks good

1.13.9

1. api/plugins: add tls-server-name arg for plugin registration [GH-23549]
Done

1.13.10 Looks good

1.13.11 Looks good

1.13.12 Looks good

1.13.13 Looks good

1.14.0

1. replication (enterprise): Add a new parameter for the update-primary API call that allows for setting of the primary cluster addresses directly, instead of via a token.
2. AWS Static Roles: The AWS Secrets Engine can manage static roles configured by users. [GH-20536]
3. MongoDB Atlas Database Secrets: Adds support for client certificate credentials [GH-20425]
4. MongoDB Atlas Database Secrets: Adds support for generating X.509 certificates on dynamic roles for user authentication [GH-20882]
5. OCI Auto-Auth: Add OCI (Oracle Cloud Infrastructure) auto-auth method [GH-19260]
6. api: Add Config.TLSConfig method to fetch the TLS configuration from a client config. [GH-20265]
7. openapi: Add openapi response definitions to /sys defined endpoints. [GH-18633]
8. openapi: Add openapi response definitions to pki/config_*.go [GH-18376]
9. openapi: Add openapi response definitions to vault/logical_system_paths.go defined endpoints. [GH-18515]
10. openapi: add openapi response definitions to /sys/internal endpoints [GH-18542]
11. openapi: add openapi response definitions to /sys/rotate endpoints [GH-18624]
12. openapi: add openapi response definitions to /sys/seal endpoints [GH-18625]
13. openapi: add openapi response definitions to /sys/tool endpoints [GH-18626]
14. openapi: add openapi response definitions to /sys/version-history, /sys/leader, /sys/ha-status, /sys/host-info, /sys/in-flight-req [GH-18628]
15. openapi: add openapi response definitions to /sys/wrapping endpoints [GH-18627]
16. openapi: add openapi response defintions to /sys/auth endpoints [GH-18465]
17. openapi: add openapi response defintions to /sys/capabilities endpoints [GH-18468]
18. openapi: add openapi response defintions to /sys/config and /sys/generate-root endpoints [GH-18472]
19. secrets/pki: Add missing fields to tidy-status, include new last_auto_tidy_finished field. [GH-20442]
20. secrets/pki: Include CA serial number, key UUID on issuers list endpoint. [GH-20276]
21. secrets/pki: Support TLS-ALPN-01 challenge type in ACME for DNS certificate identifiers. [GH-20943]
22. secrets/pki: add subject key identifier to read key response [GH-20642]
23. secrets/postgresql: Add configuration to scram-sha-256 encrypt passwords on Vault before sending them to PostgreSQL [GH-19616]
24. secrets/transit: Add support to import public keys in transit engine and allow encryption and verification of signed data [GH-17934]
25. secrets/transit: Allow importing RSA-PSS OID (1.2.840.113549.1.1.10) private keys via BYOK. [GH-19519]
26. secrets/transit: Support BYOK-encrypted export of keys to securely allow synchronizing specific keys and version across clusters. [GH-20736]
27. openapi: Small fixes for OpenAPI display attributes. Changed "log-in" to "login" [GH-20285]

Done

1.14.1

1. openapi: Better mount points for kv-v1 and kv-v2 in openapi.json [GH-21563]
2. secrets/pki: Add a parameter to allow ExtKeyUsage field usage from a role within ACME. [GH-21702]
3. openapi: Fix response schema for PKI Issue requests [GH-21449]
4. openapi: Fix schema definitions for PKI EAB APIs [GH-21458]

Done

1.14.2

1. auto-auth/azure: Added Azure Workload Identity Federation support to auto-auth (for Vault Agent and Vault Proxy). [GH-22264]
2. kmip (enterprise): Add namespace lock and unlock support [GH-21925]

Done

1.14.3

1. ** Merkle Tree Corruption Detection (enterprise) **: Add a new endpoint to check merkle tree corruption.

Done

1.14.4 Looks good

1.14.5

1. api/plugins: add tls-server-name arg for plugin registration [GH-23549]
2. kmip (enterprise): Return a structure in the response for query function Query Server Information.

Done

1.14.6 Looks good

1.14.7 Looks good

1.14.8 Looks good

1.14.9

1. oidc/provider: Adds code_challenge_methods_supported to OpenID Connect Metadata [GH-24979]
Done

1.14.10 Looks good

1.14.11 Looks good

1.14.12 Looks good

1.14.13 Looks good

1.15.0

1. Certificate Issuance External Policy Service (CIEPS) (enterprise): Allow highly-customizable operator control of certificate validation and generation through the PKI Secrets Engine.
2. Copyable KV v2 paths in UI: KV v2 secret paths are copyable for use in CLI commands or API calls [GH-22551]
3. Database Static Role Advanced TTL Management: Adds the ability to rotate
4. Event System: Add subscribe capability and subscribe_event_types to policies for events. [GH-22474] static roles on a defined schedule. [GH-22484]
5. GCP IAM Support: Adds support for IAM-based authentication to MySQL and PostgreSQL backends using Google Cloud SQL. [GH-22445]
6. SAML Auth Method (enterprise): Enable users to authenticate with Vault using their identity in a SAML Identity Provider.
7. Secrets Sync (enterprise): Add the ability to synchronize KVv2 secret with external secrets manager solutions.
8. api: add support for cloning a Client's tls.Config. [GH-21424]
9. api: adding a new api sys method for replication status [GH-20995]
10. auth/aws: Added support for signed GET requests for authenticating to vault using the aws iam method. [GH-10961]
11. auth/azure: Add support for azure workload identity authentication (see issue #18257). Update go-kms-wrapping dependency to include PR #155 [GH-22994]
12. auth/azure: Added Azure API configurable retry options [GH-23059]
13. auth/cert: Adds support for requiring hexadecimal-encoded non-string certificate extension values [GH-21830]
14. auto-auth: added support for LDAP auto-auth [GH-21641]
15. aws/auth: Adds a new config field use_sts_region_from_client which allows for using dynamic regional sts endpoints based on Authorization header when using IAM-based authentication. [GH-21960]
16. openapi: Better mount points for kv-v1 and kv-v2 in openapi.json [GH-21563]
17. secrets/db: Remove the service_account_json parameter when reading DB connection details [GH-23256]
18. secrets/pki: Add a parameter to allow ExtKeyUsage field usage from a role within ACME. [GH-21702]

Done







