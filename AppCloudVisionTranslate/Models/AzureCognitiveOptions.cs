namespace AppCloudVisionTranslate.Models;

public class AzureCognitiveOptions
{
    public string VisionEndpoint { get; set; } = "";
    public string VisionKey { get; set; } = "";
    public string TranslatorEndpoint { get; set; } = "";
    public string TranslatorKey { get; set; } = "";
    public string TranslatorRegion { get; set; } = "";
}
