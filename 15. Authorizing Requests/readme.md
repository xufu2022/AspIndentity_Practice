# 15 Authorizing Requests

## The Members Defined by the AuthorizationHandlerContext Class

| Name               | Description |
| ----------------- | ------------ |
| User | This property returns the ClaimsPrincipal object for the request that requires authorization |
| Resource | This property returns the target of the request, which will be an endpoint |
| Requirements | This property returns a sequence of all the requirements for the resource/endpoint |
| PendingRequirements | This property returns a sequence of the requirements that have not been marked as satisfied |
| Succeed(requirement) | This method tells ASP.NET Core that the specified requirement has been satisfied |
| Fail() | This method tells ASP.NET Core that a requirement has not been satisfied |

The idea is that an authorization handler will process one or more of the requirements in the policy and, assuming the requirement is satisfied, mark them as succeeded. A request will be authorized if all the requirements succeed. Authorization fails if any handler calls the Fail method. Authorization will also fail if there are outstanding requirements for which a handler has not invoked the Succeed method

```csharp
public Task HandleAsync(AuthorizationHandlerContext context) {
            foreach (CustomRequirement req in
                context.PendingRequirements.OfType<CustomRequirement>().ToList()) {
                if (context.User.Identities.Any(ident => string.Equals(ident.Name,
                        req.Name, StringComparison.OrdinalIgnoreCase))) {
                    context.Succeed(req);
                }
            }
            return Task.CompletedTask;
        }
```

## Creating and Applying the Policy

Policies are created using the AuthorizationPolicy class, whose constructor accepts a sequence of requirements and a sequence of authentication scheme names. The policy will grant access when all the requirements are satisfied for users who have been authenticated using one of the specified schemes

```csharp
 opts.FallbackPolicy = new AuthorizationPolicy(
               new IAuthorizationRequirement[] {
                   new RolesAuthorizationRequirement(
                       new [] { "User", "Administrator" }),
                   new AssertionRequirement(context =>
                       !string.Equals(context.User.Identity?.Name, "Bob"))
               }, new string[] { "TestScheme" });
```

AuthorizationOptions Members

| Name               | Description |
| ----------------- | ------------ |
| DefaultPolicy | This property defines the policy that will be applied by default when authorization is required but no policy has been selected, such as when the Authorize attribute is applied with no arguments. Any authorized user will be granted access unless a new default policy is defined |
| FallbackPolicy | This property defines the policy that is applied when no other policy has been defined. There is no fallback policy by default, which means that all requests will be authorized when there is no explicitly defined policy |
| InvokeHandlersAfterFailure | This property determines whether a single failed requirement prevents subsequent requirements from being evaluated. The default value is true, which means that all requirements are evaluated. A false value means that a failure short-circuits the requirement process. |
| AddPolicy(name, policy) AddPolicy(name, builder) | This method adds a new policy, either using an AuthorizationPolicy object or using a builder function |
| GetPolicy(name) | This method retrieves a policy using its name. |

For this example, have assigned my policy to the FallbackPolicy property, which means that it will be used to authorize requests for which no explicit authorization has been defined and which sets a baseline for the minimum authorization required for all requests the application receives. 

```csharp
    services.AddTransient<IAuthorizationHandler, CustomRequirementHandler>();
    services.AddAuthorization(opts => { AuthorizationPolicies.AddPolicies(opts); });
```

## Using the Built-In Requirements

Useful Built-In Authorization Requirement Classes
| Name               | Description |
| ----------------- | ------------ |
| NameAuthorizationRequirement | This requirement is for a case-sensitive name match. |
| RolesAuthorizationRequirement | This requirement is for a role |
| ClaimsAuthorizationRequirement | This requirement is for a claim type and a range of acceptable values |
| AssertionRequirement | This requirement evaluates a function using an AuthorizationHandlerContext object and is satisfied if the result is true. |
| DenyAnonymousAuthorization | This requirement is satisfied by any authenticated user. |

## Combining Requirements

```csharp
opts.FallbackPolicy = new AuthorizationPolicy(
    new IAuthorizationRequirement[] {
        new RolesAuthorizationRequirement(
            new [] { "User", "Administrator" }),
        new AssertionRequirement(context =>
            !string.Equals(context.User.Identity.Name, "Bob"))
    }, Enumerable.Empty<string>());
```

## Restricting Access to a Specific Authorization Scheme
```csharp
public static string[] Schemes = new string[] { "TestScheme", "OtherScheme" };

public static void AddPolicies(AuthorizationOptions opts) {
    opts.FallbackPolicy = new AuthorizationPolicy(
        new IAuthorizationRequirement[] {
            new RolesAuthorizationRequirement(
                new [] { "User", "Administrator" }),
            new AssertionRequirement(context =>
                !string.Equals(context.User.Identity.Name, "Bob"))
        }, new string[] { "TestScheme" });
 }

```
The effect is that the policy requires all its requirements to be met and for the request to have been authenticated using the TestScheme scheme

## Targeting Authorization Policies
```csharp
        public async Task Invoke(HttpContext context) {
            Endpoint? ep = context.GetEndpoint();
            if (ep != null) {
                Dictionary<(string, string), bool> results
                    = new Dictionary<(string, string), bool>();
                bool allowAnon = ep.Metadata.GetMetadata<IAllowAnonymous>() != null;
                IEnumerable<IAuthorizeData> authData =
                    ep?.Metadata.GetOrderedMetadata<IAuthorizeData>()
                        ?? Array.Empty<IAuthorizeData>();
                AuthorizationPolicy? policy = await
                    AuthorizationPolicy.CombineAsync(policyProvider, authData);
                foreach (ClaimsPrincipal cp in GetUsers()) {
                    results[(cp.Identity?.Name ?? "(No User)", cp.Identity?.AuthenticationType ?? "")] =
                            allowAnon || policy == null
                                || await AuthorizeUser(cp, policy);
                }
                context.Items["authReport"] = results;
                if (ep?.RequestDelegate != null) {
                    await ep.RequestDelegate(context);
                }
            } else {
                await next(context);
            }
        }
```

### Changing the Default Authorization Policy
```csharp
    opts.DefaultPolicy = new AuthorizationPolicy(
        new IAuthorizationRequirement[] {
            new RolesAuthorizationRequirement(
                new string[] { "Administrator"})
    }, Enumerable.Empty<string>());
```

## Configuring Targeted Authorization Polices

| Name               | Description |
| ----------------- | ------------ |
| AuthenticationSchemes | This property is used to specify a comma-separated list of allowed authentication schemes. |
| Policy | This property is used to specify a policy by name. |
| Roles | This property is used to specify a comma-separated list of allowed roles. |

## Using Named Policies
```csharp
 opts.AddPolicy("UsersExceptBob", new AuthorizationPolicy(
    new IAuthorizationRequirement[] {
        new RolesAuthorizationRequirement(new[] { "User" }),
        new AssertionRequirement(context =>
            !string.Equals(context.User.Identity.Name, "Bob"))
 }, Enumerable.Empty<string>()));
```

## The AuthorizationPolicyBuilder Methods

| Name               | Description |
| ----------------- | ------------ |
| AddAuthenticationSchemes(schemes) | This method adds one or more authentication schemes to the set of schemes that will be accepted by the policy. |
| RequireAssertion(func) | This method adds an AssertionRequirement to the policy |
| RequireAuthenticatedUser() | This method adds a DenyAnonymousAuthorizationRequirement to the policy, requiring requests to be authenticated. |
| RequireClaim(type) | This method adds a ClaimsAuthorizationRequirement to the policy, requiring a specific type of claim with any value. |
| RequireClaim(type, values) | This method adds a ClaimsAuthorizationRequirement to the policy, requiring a specific type of claim with one or more acceptable values. |
| RequireRole(roles) | This method adds a RolesAuthorizationRequirement to the policy. |
| RequireUserName(name) | This method adds a NameAuthorizationRequirement to the policy |
| AddRequirements(reqs) | This method adds one or more IAuthorizationRequirement objects to the policy, which is useful for adding custom requirements. |

```csharp
    opts.AddPolicy("UsersExceptBob", builder => builder.RequireRole("User")
        .AddRequirements(new AssertionRequirement(context =>
            !string.Equals(context.User.Identity.Name, "Bob"))).AddAuthenticationSchemes("OtherScheme"));
```

## Applying Policies Using Razor Page Conventions
```csharp
    opts.AddPolicy("NotAdmins", builder =>
        builder.AddRequirements(new AssertionRequirement(
            context =>!context.User.IsInRole("Administrator"))));


    services.AddRazorPages(opts => { opts.Conventions.AuthorizePage("/Secret", "NotAdmins"); });
    services.AddControllersWithViews(opts => {
        opts.Conventions.Add(new AuthorizationPolicyConvention("Home", policy: "NotAdmins"));
        opts.Conventions.Add(new AuthorizationPolicyConvention("Home", action: "Protected", policy: "UsersExceptBob"));
    });
```

The Razor Pages Authorization Extension Methods

| Name               | Description |
| ----------------- | ------------ |
| AuthorizePage(page, policy)  | This method applies an authorization policy to a specific page. |
| AuthorizePage(page) | This method applies the default policy to a specific page. |
| AuthorizeFolder(name, policy) | This method applies an authorization policy to all the pages in a single folder |
| AuthorizeFolder(name) | This method applies the default policy to all the pages in a single folder. |
| AllowAnonymousToPage(page) | This method grants anonymous access to a specific page. |
| AllowAnonymousToFolder(name) | This method grants anonymous access to all the pages in a single folder. |