# AspIndentity_Practice

## Scaffolding

dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator --version 7.0.0
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 7.0.0
dotnet aspnet-codegenerator identity --listFiles

libman install font-awesome@5.15.1 -d wwwroot/lib/font-awesome
```html
<link href="/lib/font-awesome/css/all.min.css" rel="stylesheet" />
```

## Scaffolding an Identity UI Razor Page

dotnet aspnet-codegenerator identity --dbContext Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext --files Account.Login

The dotnet aspnet-codegenerator identity selects the Identity UI scaffolding tool. 
The --dbContext argument is used to specify the Entity Framework Core database context class. This argument must specify the complete name, including the namespace, of the context class used by the application. If the name does not match exactly, the scaffolding tool will create a new database context class, which will lead to inconsistent results later. 

The --files argument specifies the files that should be scaffolded, using one or more names from the list produced in the previous section, separated by semicolons. I have selected the Account.Login page, which is responsible for presenting users with the external authentication buttons.

The command in Listing 6-9 adds several files to the project. The Areas/Identity/IdentityHostingStartup.cs file is used to set up features that are specific to the Identity UI package but that should contain only an empty ConfigureServices method for this chapter, like this:

```cs
using Microsoft.AspNetCore.Hosting;
[assembly: HostingStartup(typeof(IdentityApp.Areas.Identity.IdentityHostingStartup))]
namespace IdentityApp.Areas.Identity {
 public class IdentityHostingStartup : IHostingStartup {
    public void Configure(IWebHostBuilder builder) {
        builder.ConfigureServices((context, services) => {
        });
    }
 }
}
```

## Scaffolding the Management Layout Files

dotnet aspnet-codegenerator identity --dbContext Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext --files "Account.Manage._Layout;Account.Manage._ManageNav"


## Login Out

```cs
services.ConfigureApplicationCookie(opts => {
    opts.LoginPath = "/Identity/SignIn";
    opts.LogoutPath = "/Identity/SignOut";
    opts.AccessDeniedPath = "/Identity/Forbidden";
 });
```
The ConfigureApplicationCookie extension method is provided by Identity and can be used to override the default settings by assigning new values to the properties defined by the CookieAuthenticationOptions class. 