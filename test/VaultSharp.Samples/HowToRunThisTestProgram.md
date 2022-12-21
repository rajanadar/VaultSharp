
### How to run the VaultSharp End to End Test Flow

1. VaultSharp.Samples is a simple console App.
2. I debated between unit tests and a integration-like console app, and went with the console app
3. It truly tests out the e2e flow.

#### Step 1 of 2: Configure and start Vault in non-dev mode

Before you can run Program.cs, you need to start your Vault Server locally in a non-dev mode. This is what I do.

1. Create a folder anywhere on your file system.
2. Copy a version of Vault.exe in this folder. Note down the Version. 
3. Create a hcl file in this folder. My hcl file is as follows and named f.hcl:

```
ui = true
disable_mlock = true

backend "file" {
  path = "c:\\raja\\vault\\file_backend"
}

listener "tcp" {
  address     = "0.0.0.0:8200"
  tls_disable = "true"
}

raw_storage_endpoint = true

api_addr = "http://127.0.0.1:8200"
cluster_addr = "https://127.0.0.1:8201"

```

4. Now, open a command prompt, go to this folder and start the Vault server, as follows:

```
rd c:\raja\vault\file_backend /S /Q
vault server -config c:\raja\vault\f.hcl
```

#### Step 2 of 2: Run the C# Solution

At this point, we have a Vault Server running locally. That's the only step you need to do.
Don't worry about unsealing it or initializing it etc. The test-program will do all of that.

1. Now go to the VaultSharp C# solution
2. Open Program.cs, and set the vault version variable to the version of your vault.exe 
 - Note that the existing value would reflect the latest release of VaultSharp
 - You can change that, if you are testing VaultSharp for your version of Vault. Most of the times, it'll work without any issues.

   ```
   private const string ExpectedVaultVersion
   ```
   
3. Now run this Console App and check the ProgramOutput.txt file in your bin/debug folder.
4. This file emits 2 blocks of JSON for every vault interaction. Block 1 is the JSON returned by the vault server. And Block 2 is the JSON of the POCO that we deserialized the Vault server json into. It helps me quickly find out if the Vault folks added new JSON fields or not. (Their docs don't necessarily have all the fields)


FYI: I dev on a Windows Machine