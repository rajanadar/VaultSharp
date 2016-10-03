## 0.6.1 (Unreleased)

DEPRECATIONS/CHANGES:

  * AppId backend is now deprecated, but still supported.

FEATURES:

  * All the new Authentication backends are now supported: AppRole and AWS-EC2
  * All the new Secret backends are now supported: MongoDB, MSSQL and RabbitMQ
  * All the List Apis are now supported.
  * New Token Apis pertaining to token-roles are now supported.

IMPROVEMENTS:

  * You can now provide a delegate for HttpClient to be executed. Use this to set proxy settings, message handlers etc.

BUG FIXES:

  * Fixed deadlock issue with Auth login. #5
  * 

MISC:

  * Basically the change log for  VaultSharp 0.6.1 follows the changelog 
    for Vault 0.6.1 here. (https://github.com/hashicorp/vault/blob/master/CHANGELOG.md#061-august-22-2016)

## 0.4.1 (January 21, 2016)

IMPROVEMENTS:

  * Added extensive XML documentation to the Apis.
  
This is a documentation-addition-only release; other than the version number 
there are no changes from 0.4.0.
  
## 0.4.0 (January 21, 2016)

  * Initial release
  * Parity with Hashicorp's Vault 0.4.1 Api features
