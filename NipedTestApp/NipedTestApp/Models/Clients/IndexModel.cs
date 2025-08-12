using Shared.DataModels;

namespace NipedTestApp.Models.Clients;

public class IndexModel
{
    public List<Client> Clients { get; set; } = new();
    
    public List<Bloodwork> Bloodworks { get; set; } = new();
    
    public List<Questionnaire> Questionnaires { get; set; } = new();
}