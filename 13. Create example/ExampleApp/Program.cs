using ExampleApp;
using ExampleApp.Custom;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(opts => {
    opts.DefaultScheme
        = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(opts => {
    opts.LoginPath = "/signin";
    opts.AccessDeniedPath = "/signin/403";
});

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();

app.UseMiddleware<RoleMemberships>();
app.UseMiddleware<ClaimsReporter>();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapGet("/secret", SecretEndpoint.Endpoint)
    .WithDisplayName("secret");

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();