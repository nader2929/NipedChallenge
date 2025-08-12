using Shared.DataModels;

namespace NipedTestApp.Models.Clients;

public struct ClientDataModel
{
    public Client Client { get; set; }
    
    public List<Bloodwork> Bloodworks { get; set; }
    
    public List<Questionnaire> Questionnaires { get; set; }
}