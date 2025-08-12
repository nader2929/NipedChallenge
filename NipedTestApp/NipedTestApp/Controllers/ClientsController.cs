using Microsoft.AspNetCore.Mvc;
using Data;
using NipedTestApp.Models.Clients;

namespace NipedTestApp.Controllers;

[Controller]
[Route("clients")]
public class ClientsController(IClientDataReader clientDataReader, IMedicalGuidelinesReader medicalGuidelinesReader) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var guidelines = medicalGuidelinesReader.GetAll();
        var model = new IndexModel
        {
            Clients = clientDataReader.GetAllClients(),
            Bloodworks = clientDataReader.GetBloodWorks(guidelines: guidelines),
            Questionnaires = clientDataReader.GetQuestionnaires(guidelines: guidelines)
        };
        return View(model);
    }

    [HttpGet("{id}")]
    public IActionResult ClientInfo(string id)
    {
        var guidelines = medicalGuidelinesReader.GetAll();
        var client = clientDataReader.GetClientData(id, guidelines: guidelines);
        if (client == null)
        {
            return NotFound();
        }

        var clientDataModel = new ClientDataModel
        {
            Client = client,
            Bloodworks = clientDataReader.GetBloodWorks(clientId: id, guidelines: guidelines),
            Questionnaires = clientDataReader.GetQuestionnaires(clientId: id, guidelines: guidelines)
        };
        
        return View(clientDataModel);
    }

    [HttpGet("statistics")]
    public IActionResult Statistics()
    {
        var guidelines = medicalGuidelinesReader.GetAll();
        var clients = clientDataReader.GetAllClients();
        var bloodworks = clientDataReader.GetBloodWorks(guidelines: guidelines);
        var questionnaires = clientDataReader.GetQuestionnaires(guidelines: guidelines);
        
        var ageRanges = Enumerable.Range(0, 10).Select(x => (min: 7 * x + 18, max: 7 * x + 7 + 18)).ToList();
        var ageCounts = new Dictionary<string, int>();
        foreach (var range in ageRanges)
        {
            var count = clients.Count(x => x.Age >= range.min && x.Age < range.max);
            ageCounts[$"{range.min}-{range.max - 1}"] = count;
        }
        
        var genderCounts = clients
            .GroupBy(x => x.Gender)
            .ToDictionary(x => x.Key, y => y.Count());

        var model = new StatisticsModel
        {
            AgeRangeCounts = ageCounts,
            GenderCounts = genderCounts,
        };
        model.SetBloodworkStats(bloodworks);
        model.SetQuestionnairesStats(questionnaires);
        return View(model);
    }
}