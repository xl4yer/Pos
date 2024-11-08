using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using MudBlazor.Services;
using Neodynamic.Blazor;
using Pos.Class;
using Pos.Components;
using Pos.Services;
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
builder.Services.AddSingleton<INativePages, NativePages>();
builder.Services.AddScoped<MudThemeProvider>();
builder.Services.AddJSPrintManager();
builder.Services.AddAuthorization();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
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

app.UseStaticFiles();
app.UseCors("NewPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();
