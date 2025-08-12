using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Shared.DataModels;
using Data.DTOs;
using Microsoft.Extensions.Logging;

namespace Data.Json;

public class JsonClientDataReader : IClientDataReader
{
    private Dictionary<string, Client> Clients { get; set; } = new Dictionary<string, Client>();

    private Dictionary<string, List<Bloodwork>>  BloodWorks { get; set; } = new Dictionary<string, List<Bloodwork>>();

    private Dictionary<string, List<Questionnaire>>  Questionnaires { get; set; } = new Dictionary<string, List<Questionnaire>>();

    private readonly IMapper _mapper;
    
    public JsonClientDataReader(ILoggerFactory loggerFactory)
    {
        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DtoClient, Client>();
            cfg.CreateMap<DtoQuestionnaire, Questionnaire>();
            cfg.CreateMap<DtoBloodWork, Bloodwork>()
                .ForMember(x=> x.CholesterolTotal, opt => opt.MapFrom(src => src.Cholesterol.Total))
                .ForMember(x=> x.CholesterolHdl, opt => opt.MapFrom(src => src.Cholesterol.Hdl))
                .ForMember(x=> x.CholesterolLdl, opt => opt.MapFrom(src => src.Cholesterol.Ldl))
                .ForMember(x=> x.BloodPressureSystolic, opt => opt.MapFrom(src => src.BloodPressure.Systolic))
                .ForMember(x=> x.BloodPressureDiastolic, opt => opt.MapFrom(src => src.BloodPressure.Diastolic));
        }, loggerFactory);
        _mapper = mapConfig.CreateMapper();
        
        var json = File.ReadAllText("data/clientData.json");
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
        var dtoClientList = jsonDeserialized.RootElement.Deserialize<DtoClientList>(options);
        if (dtoClientList == null) return;
        foreach (var dtoClient in dtoClientList.Clients)
        {
            var client = _mapper.Map<Client>(dtoClient);
            Clients.Add(client.Id, client);
            
            var bloodwork = _mapper.Map<Bloodwork>(dtoClient.MedicalData.Bloodwork);
            bloodwork.ClientId = client.Id;
            BloodWorks.TryAdd(client.Id, []);
            BloodWorks[client.Id].Add(bloodwork);

            var questionnaire = _mapper.Map<Questionnaire>(dtoClient.MedicalData.Questionnaire);
            questionnaire.ClientId = client.Id;
            Questionnaires.TryAdd(client.Id, []);
            Questionnaires[client.Id].Add(questionnaire);
        }
    }

    public List<Client> GetAllClients(int lastResult = 0, int maxResult = 100_000)
    {
        return Clients.Values.Skip(lastResult).Take(maxResult).ToList();
    }

    public Client? GetClientData(string clientId, Dictionary<string, Guideline>? guidelines = null)
    {
        if (!Clients.TryGetValue(clientId, out var client))
        {
            return null;
        }

        return client;
    }

    public List<Bloodwork> GetBloodWorks(string? clientId = null, Dictionary<string, Guideline>? guidelines = null)
    {
        List<Bloodwork> bloodworks;
        bloodworks = !string.IsNullOrEmpty(clientId) ? 
            BloodWorks[clientId] : BloodWorks.Values.SelectMany(x => x).ToList();

        if (guidelines != null)
        {
            foreach (var bloodwork in bloodworks)
            {
                bloodwork.SetStatus(guidelines);
            }
        }

        return bloodworks;
    }
    
    public List<Questionnaire> GetQuestionnaires(string? clientId = null, Dictionary<string, Guideline>? guidelines = null)
    {
        List<Questionnaire> questionnaires;
        questionnaires = !string.IsNullOrEmpty(clientId) ? 
            Questionnaires[clientId] : Questionnaires.Values.SelectMany(x => x).ToList();
        
        if (guidelines != null)
        {
            foreach (var questionnaire in questionnaires)
            {
                questionnaire.SetStatus(guidelines);
            }   
        }

        return questionnaires;
    }
}