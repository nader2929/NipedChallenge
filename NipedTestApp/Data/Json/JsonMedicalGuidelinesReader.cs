using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Shared.DataModels;
using Data.DTOs;
using Microsoft.Extensions.Logging;

namespace Data.Json;

public class JsonMedicalGuidelinesReader : IMedicalGuidelinesReader
{
    private Dictionary<string, Guideline> Guidelines { get; } = new Dictionary<string, Guideline>();
    
    public JsonMedicalGuidelinesReader()
    {
        var json = File.ReadAllText("data/medicalGuidelines.json");
        LoadData(json);
    }

    private void LoadData(string json)
    {
        var jsonDeserialized = JsonDocument.Parse(json);
        // Deserialize the JSON string into a custom type
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        var guidelines = jsonDeserialized.RootElement.GetProperty("guidelines");
        ExtractNestedGuidelines(guidelines, "");
    }
    
    private void ExtractNestedGuidelines(JsonElement element, string path)
    {
        if (element.TryGetProperty("optimal", out var optimal) &&
            element.TryGetProperty("needsAttention", out var needsAttention) &&
            element.TryGetProperty("seriousIssue", out var seriousIssue))
        {
            Guidelines[path.ToLower()] = new Guideline()
            {
                Category = path,
                Optimal = optimal.GetString() ?? string.Empty,
                NeedsAttention = needsAttention.GetString() ?? string.Empty,
                SeriousIssue = seriousIssue.GetString() ?? string.Empty
            };
        }
        else
        {
            foreach (var prop in element.EnumerateObject())
            {
                var newPath = string.IsNullOrEmpty(path) ? prop.Name : $"{path}.{prop.Name}";
                if (prop.Value.ValueKind == JsonValueKind.Object)
                {
                    ExtractNestedGuidelines(prop.Value, newPath);
                }
            }
        }
    }

    public bool TryGetGuideline(string id, [NotNullWhen(true)] out Guideline? guideline)
    {
        return Guidelines.TryGetValue(id, out guideline);
    }

    public Dictionary<string, Guideline> GetAll()
    {
        return Guidelines;
    }
}