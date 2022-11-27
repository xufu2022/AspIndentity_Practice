# Configuring Identity

Identity is configured using the standard ASP.NET Core options pattern, using the settings defined by the **IdentityOptions** class defined in the Microsoft.AspNetCore.Identity namespace. Table below describes the most useful properties defined by the IdentityOptions class, each of which leads to its own set of options, 
described in the sections that follow

| Name               | Description |
| ----------------- | ------------ | 
| User | This property is used to configure the username and email options for user accounts using the UserOptions class, as described in the “Configuring User Options” section | 
| Password | This property is used to define the password policy using the PasswordOptions class, as described in the “Configuring Password Options” section. | 
| SignIn | This property is used to specify the confirmation requirements for accounts using the SignInOptions class, as described in the “Configuring Sign IN Confirmation Requirements” section | 
| Lockout | This property uses the LockoutOptions class to define the policy for locking out accounts after a number of failed attempts to sign in, as described in the “Configuring Lockout Options” section. | 

## Configuring User Options

The UserOptions Properties 
| Name               | Description |
| ----------------- | ------------ | 
| AllowedUserNameCharacters | This property specifies the characters allowed in usernames. The default value is the set of upper and lowercase A–Z characters, the digits 0–9, and the symbols -._@+ (hyphen, period, underscore, at character, and plus symbol). | 
| RequireUniqueEmail | This property determines whether email addresses must be unique. The default value is false | 

Configuring Sign-in Confirmation Requirements

| Name               | Description |
| ----------------- | ------------ | 
| RequireConfirmedEmail | When this property is set to true, only accounts with confirmed email addresses can sign in. The default value is false | 
| RequireConfirmedPhoneNumber | When this property is set to true, only accounts with confirmed phone numbers can sign in. The default value is false. | 
| RequireConfirmedAccount | When set to true, only accounts that pass verification by the IUserConfirmation<T> interface can sign in. I describe this interface in detail in Chapter 9, and the default implementation checks that the email address has been confirmed. This default value for this property is false. | 

Configuring Lockout Options

The IdentityOptions.Lockout property is assigned a LockoutOptions object, which is used to configure 
lockouts that prevent sign-ins, even if the correct password is used, after a number of failed attempts. 

| Name               | Description |
| ----------------- | ------------ | 
| MaxFailedAccessAttempts | This property specifies the number of failed attempts allowed before an account is locked out. The default value is 5. | 
| DefaultLockoutTimeSpan | This property specifies the duration for lockouts. The default value is 5 minutes | 
| AllowedForNewUsers | This property determines whether the lockout feature is enabled for new accounts. The default value is true. | 