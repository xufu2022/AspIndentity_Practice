# Authenticating API Clients

dotnet ef database drop --force --context ProductDbContext
dotnet ef database drop --force --context IdentityDbContext
dotnet ef database update --context ProductDbContext
dotnet ef database update --context IdentityDbContext

dotnet run
netstat -a -o -n
taskkill /F /PID 21224

Invoke-WebRequest -Uri https://localhost:44350/api/data
Invoke-WebRequest -Uri https://localhost:44350/api/data -Headers @{"X-Requested-With"="XMLHttpRequest"}
...
Invoke-WebRequest : The remote server returned an error: (401) Unauthorized.
...

## The CookieAuthenticationEvents Event Handler Properties for API Clients

| Name               | Description |
| ----------------- | ------------ |
| OnRedirectToLogin | This property is assigned a handler that is called when the user should be redirected to the sign-in URL. The default handler sends a status code 
response for requests with the X-Requested-With header and performs the redirection for all other requests. |
| OnRedirectToAccessDenied | This property is assigned a handler that is called when the user should be redirected to the access denied URL. The default handler sends a status code response for requests with the X-Requested-With header and performs the 
redirection for all other requests. |
| OnRedirectToLogout | This property is assigned a handler that is called when the user should be redirected to the sign-out URL. The default handler sends a 200 OK response with a Location header set to the sign-out URL and performs the redirection 
for all other requests. |
| OnRedirectToReturnUrl | This property is assigned a handler that is called when the user should be redirected to the URL that triggered a challenge response. The default handler sends a 200 OK response with a Location header set to the sign-out URL and performs the redirection for all other requests. |

The handler functions assigned to the properties described in Table 12-3 receive an instance of the RedirectContext<CookieAuthenticationOptions> class, whose most useful properties are described in the following table

| Name               | Description |
| ----------------- | ------------ |
| HttpContext | This property returns the HttpContext object for the current request. |
| Request | This property returns the HttpRequest object that describes the request |
| Response | This property returns the HttpRequest object that describes the response. |
| RedirectUri | This property returns the URL to which the user should be directed. |

## Authenticating API Clients Directly

Starting a Web Server in the wwwroot Folder
npx http-server -p 5100 -c-1

This command downloads and executes the JavaScript http-server package, which is a light-weight HTTP server. The arguments tell the server to listen for requests on port 5100 and to disable caching, which ensures that any changes you make to the JavaScript client will take effect the next time the browser is reloaded.

## Using Bearer Tokens

You cannot rely on cookies if your API supports clients that are not browsers. Bearer tokens are strings that are provided to the client during the sign-in process and then included as a header in subsequent requests. 
This is essentially the same mechanism as for cookies, but the client takes responsibility for receiving the token and including it in future requests instead of relying on the browser to do the work

## Configuring ASP.NET Core for JWT Bearer Tokens

dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 7.0.0

appsettings.json
```json
{
    ..., 
     "BearerTokens": {
        "ExpiryMins": "60",
        "Key": "mySuperSecretKey"
    }
}
```
```cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

...
services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts => {
        opts.TokenValidationParameters.ValidateAudience = false;
        opts.TokenValidationParameters.ValidateIssuer = false;
        opts.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes( Configuration["BearerTokens:Key"]));
    });
```


The AddJwtBearer extension method adds an authentication handler that will use JWT tokens and use them to authenticate requests. JWT tokens are intended to securely describe claims between two parties, and there are lots of features to support that goal, both in terms of the data that a token contains and how that data is validated. 

When using JWT tokens to authenticate clients for an ASP.NET Core API controller, the role of the token is much simpler because the JavaScript client doesn’t process the contents of the token and just treats it as an opaque block of data that is included in HTTP requests to identify the client to ASP.NET Core.

use the options pattern to set the ValidateAudience and ValidateIssuer properties to false, which reduces the amount of data I have to put into the token later. What is important is the ability to validate the cryptographic signature that tokens include, so I read the secret key from the configuration service and apply it using the options pattern.

**opts.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"]))**

## Updating Api Authentication Controller

```cs
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System;
using System.Text;
```

The SecurityTokenDescriptor Properties Used to Generate a Token

| Name               | Description |
| ----------------- | ------------ |
| Subject | This property is assigned the ClaimsIdentity object whose claims will be included in the token. |
| Expires | This property is used to specify the DateTime that determines when the token expires. The token won’t be validated if a client presents it after this point. |
| SigningCredentials | This property is used to specify the algorithm and the secret key that will be used to sign the token. |

The **SecurityTokenDescriptor** object is used to create a token using the CreateToken method defined by the JwtSecurityTokenHandler class, which is written as a string using the WriteToken method and sent in the response. 

The SignInManager<IdentityUser> Methods to Support Bearer Tokens

| Name               | Description |
| ----------------- | ------------ |
| CheckPasswordSignInAsync(user, password, lockout) | This method checks a password for an IdentityUser object without signing the user into the application. The lockout argument specifies whether an incorrect password will count toward a lockout. Lockouts |
| CreateUserPrincipalAsync(user) | This method creates a ClaimsPrincipal object from an IdentityUser object, which is used as the data for the bearer token |

## Specifying Token Authentication in the API Controller
```csharp
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
```

```js
//signin
if (response.ok) {
    let responseData = await response.json();
    if (responseData.success) {
        baseRequestConfig.headers = {"Authorization": `Bearer ${responseData.token}`}
    }
    processResponse(response, async () =>
        callback(responseData, errorHandler));
        return;
    }

    processResponse({ ok: false, status: "Auth Failed" }, async () => callback(responseData), errorHandler);

//signout
export const signOut = async function (callback) {
    //const response = await fetch(`${authUrl}/signout`, {
    // ...baseRequestConfig,
    // method: "POST"
    //});
    baseRequestConfig.headers = {};
    processResponse({ ok: true }, callback, callback);
}
```

