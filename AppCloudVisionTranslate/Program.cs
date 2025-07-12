using AppCloudVisionTranslate.Models;

var builder = WebApplication.CreateBuilder(args);

#region ░░ Configuración ░░────────────────────────────────────────────

// 1. Opciones de Azure Cognitive
builder.Services.Configure<AzureCognitiveOptions>(
    builder.Configuration.GetSection("AzureCognitive"));

// 2. HttpClient para Computer Vision
builder.Services.AddHttpClient("vision", client =>
{
    var cfg = builder.Configuration;
    client.BaseAddress = new Uri(cfg["AzureCognitive:VisionEndpoint"]!);
    client.DefaultRequestHeaders.Add(
        "Ocp-Apim-Subscription-Key",
        cfg["AzureCognitive:VisionKey"]!);
});

// 3. HttpClient para Translator
builder.Services.AddHttpClient("translator", client =>
{
    var cfg = builder.Configuration;
    client.BaseAddress = new Uri(cfg["AzureCognitive:TranslatorEndpoint"]!);
    client.DefaultRequestHeaders.Add(
        "Ocp-Apim-Subscription-Key",
        cfg["AzureCognitive:TranslatorKey"]!);
    client.DefaultRequestHeaders.Add(
        "Ocp-Apim-Subscription-Region",
        cfg["AzureCognitive:TranslatorRegion"]!);
});

// 4. Servicios Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// (Opcional) Aumentar el límite de tamaño de archivo si vas a subir imágenes grandes
builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 10 * 1024 * 1024); // 10 MB

#endregion

var app = builder.Build();

#region ░░ Canalización HTTP ░░────────────────────────────────────────

// Página de error genérica en producción
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseRouting();

// Endpoints de Blazor
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

#endregion

app.Run();
