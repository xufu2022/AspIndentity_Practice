# Claims, Roles, and Confirmations

## Storing Claims in the User Store

The IUserClaimStore<T> Methods

| Name               | Description |
| ----------------- | ------------ |
| GetClaimsAsync(user, token) | This method returns an IList<Claim> that contains all the user’s claims |
| AddClaimsAsync(user, claims, token) | This method adds one or more claims to the user object, which is received as an IEnumerable<Claim>. |
| RemoveClaimsAsync(user, claims, token) | This method removes one or more claims to the user object, which is received as an IEnumerable<Claim>. |
| ReplaceClaimAsync(user, oldClaim, newClaim, token) | This method replaces one claim with another. The claims do not have to be of the same type. |
| GetUsersForClaimAsync(claim, token) | This method returns an IList<T> object, where T is the user class that contains all the users with a specific claim.  |

```csharp
public class AppUser {
    ......
    public IList<Claim> Claims { get; set; }

    public string SecurityStamp { get; set; }
}
```

The IUserSecurityStampStore<T> Methods
| Name               | Description |
| ----------------- | ------------ |
| SetSecurityStampAsync(user, stamp, token) | This method sets a new security stamp for the specified user. |
| GetSecurityStampAsync(user, token) | This method retrieves the specified user’s security stamp. |