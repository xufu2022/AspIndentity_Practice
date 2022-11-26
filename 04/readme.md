# Using the Identity UI Package

| Problem               | Solution |
| ----------------- | ------------ | 
|  Add Identity and the Identity UI package to a project  | Add the NuGet packages to the project and configure them using the AddDefaultIdentity method in the Startup class. Create a database migration and use it to prepare a database for storing user data. | 
|  Present the user with the registration or sign-in links  | Create a shared partial view named _LoginPartial.cshtml | 
|  Create a consistent layout for the application and the Identity UI package  |Define a Razor Layout and refer to it in a Razor View Start created in the Areas/Identity/Pages folder. | 
|  Add support for confirmations  | Create an implementation of the IEmailSender interface and register it as a service in the Startup class| 
