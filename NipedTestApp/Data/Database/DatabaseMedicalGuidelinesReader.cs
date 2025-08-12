using System.Diagnostics.CodeAnalysis;
using Data.Json;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DataModels;

namespace Data.Database;

public class DatabaseMedicalGuidelinesReader : IMedicalGuidelinesReader
{
    private readonly IDbContextFactory<NipedDbContext> _dbContextFactory;
    
    public DatabaseMedicalGuidelinesReader(IDbContextFactory<NipedDbContext> dbContextFactory)
    {
        _dbContextFactory  = dbContextFactory;
        var dbContext = _dbContextFactory.CreateDbContext();
        LoadData(dbContext);
    }
    

    public bool TryGetGuideline(string id, [NotNullWhen(true)] out Guideline? guideline)
    {
        var dbContext = _dbContextFactory.CreateDbContext();
        guideline = dbContext.Guidelines.FirstOrDefault(x => x.Category == id);
        return guideline != null;
    }

    public Dictionary<string, Guideline> GetAll()
    {
        var dbContext = _dbContextFactory.CreateDbContext();
        return dbContext.Guidelines.ToDictionary(x => x.Category.ToLower(), x => x);
    }

    private void LoadData(NipedDbContext dbContext)
    {
        var jsonMedicalGuidelinesReader = new JsonMedicalGuidelinesReader();
        if (!dbContext.Guidelines.Any())
        {
            dbContext.AddRange(jsonMedicalGuidelinesReader.GetAll().Values);
        }
        dbContext.SaveChanges();
    }
}