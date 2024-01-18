# acumatica-customizationAPI
## Developer Tools to Manage Customization Projects Remotely

When this package is installed on an acumatica site, it allows you to manage customization projects via an api call to an Acumatica Webhook.
This is useful for managing customization projects in a source control system like git. Often times developers can forget to load their database customizations (scripts, site map entries etc)
from the database and save them before committing. This would be the start of a helpful tool to prevent this.

The webhook URL is: {BaseURL}/Webhooks/{YourTenantName}/74026c0b-201e-41f6-bb6d-8a0068aa7c42

Use the following query parameters:

| Parameter | Description                 |
|-----------|-----------------------------|
| Project   | The name of the project you want to work with |
| Method    | The action you want to take on the project    |
| Dir       | The directory the project lives in |

| Method | Description |
|--------|-------------|
| reloadfromdb | Loads the database scripts from the databases and persists them in the customization project |
| saveproject | Saves the named project to the folder specified in the Dir query parameter | 



## Setting up the development environment
Run the following commands in PowerShell to set up the development environment:

```powershell
.\initrepo.ps1
```
