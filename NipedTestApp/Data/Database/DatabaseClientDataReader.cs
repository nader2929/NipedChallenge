using Data.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.DataModels;

namespace Data.Database;

public class DatabaseClientDataReader : IClientDataReader
{
    private readonly IDbContextFactory<NipedDbContext> _dbContextFactory;

    public DatabaseClientDataReader(ILoggerFactory loggerFactory, IDbContextFactory<NipedDbContext> dbContextFactory)
    {
        _dbContextFactory  = dbContextFactory;
        var dbContext = dbContextFactory.CreateDbContext();
        LoadData(dbContext, loggerFactory);
    }
    
    public List<Client> GetAllClients(int lastResult = 0, int maxResult = 100_000)
    {
        var dbContext = _dbContextFactory.CreateDbContext();
        return dbContext.Clients.Skip(lastResult).Take(maxResult).ToList();
    }

    public Client? GetClientData(string clientId, Dictionary<string, Guideline>? guidelines = null)
    {
        var dbContext = _dbContextFactory.CreateDbContext();
        var client = dbContext.Clients.FirstOrDefault(x => x.Id == clientId);
        return client;
    }

    public List<Bloodwork> GetBloodWorks(string? clientId = null, Dictionary<string, Guideline>? guidelines = null)
    {
        var dbContext = _dbContextFactory.CreateDbContext();
        List<Bloodwork> bloodworks;
        bloodworks = !string.IsNullOrEmpty(clientId) ? 
            dbContext.Bloodworks.Where(x => x.ClientId == clientId).ToList() : 
            dbContext.Bloodworks.ToList();

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
        var dbContext = _dbContextFactory.CreateDbContext();
        List<Questionnaire> questionnaires;
        questionnaires = !string.IsNullOrEmpty(clientId) ? 
            dbContext.Questionnaires.Where(x => x.ClientId == clientId).ToList() : 
            dbContext.Questionnaires.ToList();
        
        if (guidelines != null)
        {
            foreach (var questionnaire in questionnaires)
            {
                questionnaire.SetStatus(guidelines);
            }   
        }

        return questionnaires;
    }

    private void LoadData(NipedDbContext dbContext, ILoggerFactory loggerFactory)
    {
        var jsonClientDataReader = new JsonClientDataReader(loggerFactory);
        if (!dbContext.Clients.Any())
        {
            dbContext.AddRange(jsonClientDataReader.GetAllClients());
        }

        if (!dbContext.Bloodworks.Any())
        {
            dbContext.AddRange(jsonClientDataReader.GetBloodWorks());
        }

        if (!dbContext.Questionnaires.Any())
        {
            dbContext.AddRange(jsonClientDataReader.GetQuestionnaires());
        }

        dbContext.SaveChanges();
    }
}