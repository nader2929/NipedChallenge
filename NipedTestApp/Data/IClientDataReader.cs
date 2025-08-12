using Shared.DataModels;

namespace Data;

public interface IClientDataReader
{
    public List<Client> GetAllClients(int lastResult = 0, int maxResult = 100_000);

    public Client? GetClientData(string clientId, Dictionary<string, Guideline>? guidelines = null);

    public List<Bloodwork> GetBloodWorks(string? clientId = null, Dictionary<string, Guideline>? guidelines = null);

    public List<Questionnaire> GetQuestionnaires(string? clientId = null, Dictionary<string, Guideline>? guidelines = null);

}