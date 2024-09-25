using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;
using WEB_253505_Bekarev.ClassHelpers;
using WEB_253505_Bekarev.Extensions.HostingExtensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

UriData uriData = new UriData { ApiUri = builder.Configuration.GetSection("UriData")["ApiUri"] };
builder.RegisterCustomServices(uriData);


var keycloakData =
builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer()
            .AddOpenIdConnect(options =>
            {
                options.Authority =
                $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
                options.ClientId = keycloakData.ClientId;
                options.ClientSecret = keycloakData.ClientSecret;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.Scope.Add("openid"); // Customize scopes as needed
                options.SaveTokens = true;
                options.RequireHttpsMetadata = false; // позволяет обращаться к локальному Keycloak по http
                options.MetadataAddress =
                $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
            });

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

app.UseSession();
 
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
