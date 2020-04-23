
1. VaultSharp.Samples is a simple console App.
2. I debated between unit tests and a integration-like console app, and went with the console app
3. It truly tests out the e2e flow.
4. Before you run the app, do the following:


1. Create a folder
2. Have Vault.exe in it. The vault version can be figured by the 
   ```
   private const string ExpectedVaultVersion
   ```
   varible in Program.cs
3. Start it with a file backend. Here is the sample hcl (f.hcl) and command I use.

```
backend "file" {
  path = "c:\\raja\\vault\\file_backend"
  }

listener "tcp" {
  address = "127.0.0.1:8200"
  tls_disable = 1
}

raw_storage_endpoint = true
```

```
rd c:\raja\vault\file_backend /S /Q
vault server -config c:\raja\vault\f.hcl
```

4. This will start up the Vault server. That's it with the setup.
5. Now run this Console App and check the ProgramOutput.txt file in your bin/debug folder. (not in the prject folder)

Hopefully you get a nice text file.

FYI: I dev on a Windows 10 x64 bit OS.
