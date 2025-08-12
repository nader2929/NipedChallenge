using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;

namespace Shared.DataModels;

[PrimaryKey(nameof(Id))]
public class Questionnaire
{
    public int Id { get; set; }
    public string ClientId { get; set; }
    public Client Client { get; set; }
    public int ExerciseWeeklyMinutes { get; set; }
    public string SleepQuality { get; set; }
    public string StressLevels { get; set; } 
    public string DietQuality { get; set; }

    [NotMapped]
    public QuestionnaireStatus Status { get; set; } = new ();

    public void SetStatus(Dictionary<string, Guideline> guidelines)
    {
        Status.ExerciseWeeklyMinutes = GuidelinesChecker.ClassifyMeasurement(ExerciseWeeklyMinutes, guidelines["exerciseweeklyminutes"]);
        Status.SleepQuality = GuidelinesChecker.ClassifyMeasurement(SleepQuality, guidelines["sleepquality"]);
        Status.StressLevels = GuidelinesChecker.ClassifyMeasurement(StressLevels, guidelines["stresslevels"]);
        Status.DietQuality = GuidelinesChecker.ClassifyMeasurement(DietQuality, guidelines["dietquality"]);
    }
}

public class QuestionnaireStatus
{
    public MeasurementStatus ExerciseWeeklyMinutes { get; set; }
    public MeasurementStatus SleepQuality { get; set; }
    public MeasurementStatus StressLevels { get; set; } 
    public MeasurementStatus DietQuality { get; set; }
}