# Using Roles and Claims

dotnet ef database drop --force --context ProductDbContext
dotnet ef database drop --force --context IdentityDbContext
dotnet ef database update --context ProductDbContext
dotnet ef database update --context IdentityDbContext

## Managing Claims

A claim is a piece of data that is known about the user. This is a vague description because there are no limits placed on what a claim can describe or where it comes from. Claims are represented using the Claim class defined in the System.Security.Claims namespace

| Name               | Description |
| ----------------- | ------------ | 
| Type | This property returns the claim type. | 
| Value | This property returns the claim value. | 
| Issuer | This property returns the source of the claim. | 

An application can collect claims about a user from multiple sources and act on what the claims assert about the user and how much the source of the claim is trusted

An application can also collect multiple identities for a single user. Different systems may use account 
names, for example, and so there may be different sets of claims for each identity.

In practice, many applications don’t use claims directly at all, relying on roles to manage a user’s access to restricted resources. Even when claims are used, applications tend not to have a nuanced understanding of a user’s claims and identities because building and maintaining that understanding is complex. But, even so, understanding how claims are used is important because they underpin important Identity features,

## The UserManager<IdentityUser> Methods for Working with Claims

| Name               | Description |
| ----------------- | ------------ |
| GetClaimsAsync(user) | This method returns an IList<Claim> containing the claims in the user store for the specified user |
| AddClaimAsync(user, claim) | This method adds a claim to the user store for the specified user |
| ReplaceClaimAsync(user, old, new) | This method replaces one claim with another for the specified user |
| RemoveClaimAsync(user, claim) | This method removes a claim from the user store for the specified user |

## Using Claims Data

```html
@{
    Func<string, bool> HasClearance = (string level)
        => User.HasClaim(ApplicationClaimTypes.SecurityClearance, level);
}
```

The ClaimsPrincipal Convenience Members for Claims
| Name               | Description |
| ----------------- | ------------ |
| Claims | This property returns an IEnumerable<Claim> containing the claims from all the ClaimIdentity objects associated with the ClaimsPrincipal. |
| FindAll(type)  FindFirst(type)  | This method returns all of the claims, or the first claim, with the specified type from all the ClaimIdentity objects associated with the ClaimsPrincipal. |
| FindAll(filter) FindFirst(filter) | This method returns the claims, or the first claim, that matches the specified filter predicate from all the ClaimIdentity objects associated with the ClaimsPrincipal |
| HasClaim(type, value) | This method returns true if any of the ClaimIdentity objects associated with |
| HasClaim(filter) | the ClaimsPrincipal has a claim with the specified type and value or that matches the specified predicate. |


