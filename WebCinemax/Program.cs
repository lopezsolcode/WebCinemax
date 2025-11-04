using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using WebCinemax.Components;
using WebCinemax.Models;
using WebCinemax.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQuickGridEntityFrameworkAdapter();

// 🔹 Registrar DbContextFactory
builder.Services.AddDbContextFactory<CineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebCinemaxContext")
                         ?? throw new InvalidOperationException("Connection string 'WebCinemaxContext' not found.")));

// 🔹 Registrar servicios propios
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<PeliculaService>();
builder.Services.AddScoped<SalaService>();
builder.Services.AddScoped<FuncionService>();
builder.Services.AddScoped<ButacaService>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<WebCinemax.Models.Usuario>,
                            Microsoft.AspNetCore.Identity.PasswordHasher<WebCinemax.Models.Usuario>>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ProtectedSessionStorage>();

// 🔹 Razor Components (Blazor Server)
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

var app = builder.Build();

// 🔹 Configurar el pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

// ⚙️ Middleware para exigir autenticación global (excepto login/logout)
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "";

    // ✅ Rutas públicas permitidas sin login
    if (path.StartsWith("/login") || path.StartsWith("/logout") || path.StartsWith("/_framework")
        || path.StartsWith("/_blazor") || path.StartsWith("/_content"))

    {
        await next();
        return;
    }

    // Verificar autenticación dentro del contexto Blazor
    var authProvider = context.RequestServices.GetRequiredService<AuthenticationStateProvider>();
    var authState = await authProvider.GetAuthenticationStateAsync();

    if (authState.User?.Identity is not { IsAuthenticated: true })
    {
        // Redirigir al login si no está autenticado
        context.Response.Redirect("/login");
        return;
    }

    await next();
});

// 🔹 Mapear componentes Blazor
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();