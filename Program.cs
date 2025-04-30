using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using MudBlazor.Services;
using Neodynamic.Blazor;
using Pos;
using Pos.Class;
using Pos.Components;
using Pos.Hubs;
using Pos.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();
builder.Services.AddTransient<AppDb>();
builder.Services.AddTransient<UserServices>();
builder.Services.AddTransient<ProductServices>();
builder.Services.AddTransient<TempServices>();
builder.Services.AddTransient<PurchaseServices>();
builder.Services.AddTransient<QtyServices>();
builder.Services.AddSingleton<INativePages, NativePages>();
builder.Services.AddScoped<MudThemeProvider>();
builder.Services.AddJSPrintManager();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var appSettingsSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Cashier", policy => policy.RequireRole("Cashier"));
    options.AddPolicy("AdminCashier", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(ClaimTypes.Role, "Cashier") ||
            context.User.HasClaim(ClaimTypes.Role, "Admin")));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = false
    };

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for menuion scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles(); // Serve static files early in the pipeline
app.UseRouting(); // Set up routing
app.UseCors("NewPolicy"); // Apply CORS policy after routing is enabled
app.UseAuthentication(); // Authentication should come before authorization
app.UseAuthorization(); // Authorization follows authentication

// Map hubs before mapping controllers or Razor Components
app.MapHub<Hubs>("/hub");

// Map controllers for API endpoints
app.MapControllers();

// Map Razor Components for server-side rendering
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add antiforgery handling last in this scenario
app.UseAntiforgery();

app.Run(); // Run the application
