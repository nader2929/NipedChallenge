using System.Diagnostics.CodeAnalysis;
using Shared.DataModels;

namespace Data;

public interface IMedicalGuidelinesReader
{
    public bool TryGetGuideline(string id, [NotNullWhen(true)] out Guideline? guideline);
    
    public Dictionary<string, Guideline> GetAll();
}