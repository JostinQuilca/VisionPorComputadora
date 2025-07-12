using AppCloudVisionTranslate.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Azure Cognitive
builder.Services.Configure<AzureCognitiveOptions>(
    builder.Configuration.GetSection("AzureCognitive"));

// HttpClient para Computer Vision
builder.Services.AddHttpClient("vision", client =>
{
    var cfg = builder.Configuration;
    client.BaseAddress = new Uri(cfg["AzureCognitive:VisionEndpoint"]!);
    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", cfg["AzureCognitive:VisionKey"]!);
});

// HttpClient para Translator
builder.Services.AddHttpClient("translator", client =>
{
    var cfg = builder.Configuration;
    client.BaseAddress = new Uri(cfg["AzureCognitive:TranslatorEndpoint"]!);
    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", cfg["AzureCognitive:TranslatorKey"]!);
    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", cfg["AzureCognitive:TranslatorRegion"]!);
});

// Servicios Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
});

// Permitir imágenes grandes
builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 10 * 1024 * 1024);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
