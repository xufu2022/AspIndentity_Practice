# Creating the Example Project

-- dotnet --list-sdks
-- dotnet --list-runtimes

dotnet new globaljson --sdk-version 7.0.100 --output ExampleApp
dotnet new web --no-https --output ExampleApp --framework net7.0
dotnet new sln -o ExampleApp

dotnet sln ExampleApp add ExampleApp

dotnet tool uninstall --global Microsoft.Web.LibraryManager.Cli
dotnet tool install --global Microsoft.Web.LibraryManager.Cli --version 2.1.175
libman init -p cdnjs
libman install twitter-bootstrap@4.5.0 -d wwwroot/lib/twitter-bootstrap

##  The Static Methods Defined by the AuthenticateResult Class

| Name               | Description |
| ----------------- | ------------ |
| Success(ticket) | This method creates a result indicating that authentication succeeded. |
| Fail(message) | This method creates a result indicating that authentication failed. |
| NoResult() | This method creates a result indicating no authentication was performed for this request. For the example application, this means that a request did not contain a cookie named authUser. |

```csharp
result = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(ident), scheme.Name));
```
