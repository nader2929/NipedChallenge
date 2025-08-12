using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.DataModels;

[PrimaryKey(nameof(Id))]
public class Client
{
    public string Id { get; set; }

    public string Name { get; set; }
    
    public DateOnly DateOfBirth { get; set; }

    public Gender Gender { get; set; }
    
    [NotMapped]
    public int Age
    {
        get
        {
            var now = DateTimeOffset.Now;
            var dateOnlyNow = new DateOnly(now.Year, now.Month, now.Day);
            var age = now.Year - DateOfBirth.Year;
            if (DateOfBirth.Month >= now.Month && DateOfBirth.Day >= now.Day)
            {
                age =  age - 1;
            }
            return age;
        }
    }

    public List<Bloodwork> Bloodworks { get; set; } = new();
    public List<Questionnaire> Questionnaires { get; set; } = new();
}

public enum Gender
{
    Male,
    Female,
    Other
}