using Shared.DataModels;

namespace NipedTestApp.Models.Clients;

public class StatisticsModel()
{
    public Dictionary<string, int> AgeRangeCounts { get; set; } = new ();
    
    public Dictionary<Gender, int> GenderCounts { get; set; } = new ();
    
    public Dictionary<MeasurementStatus, int> BloodSugarCounts { get; set; } = new ();
    public Dictionary<MeasurementStatus, int> BloodPressureSystolicCounts { get; set; } = new ();
    public Dictionary<MeasurementStatus, int> BloodPressureDiastolicCounts { get; set; } = new ();
    public Dictionary<MeasurementStatus, int> CholesterolTotalCounts { get; set; } = new ();
    public Dictionary<MeasurementStatus, int> CholesterolHdlCounts { get; set; } = new ();
    public Dictionary<MeasurementStatus, int> CholesterolLdlCounts { get; set; } = new ();
    
    public Dictionary<MeasurementStatus, int> ExerciseWeeklyMinutesCounts { get; set; } = new ();
    public Dictionary<MeasurementStatus, int> SleepQualityCounts { get; set; } = new ();
    public Dictionary<MeasurementStatus, int> StressLevelsCounts { get; set; } = new ();
    public Dictionary<MeasurementStatus, int> DietQualityCounts { get; set; } = new ();

    public void SetBloodworkStats(List<Bloodwork> bloodworks)
    {
        BloodSugarCounts = bloodworks
            .GroupBy(x => x.Status.BloodSugar)
            .ToDictionary(x => x.Key, y => y.Count());

        BloodPressureSystolicCounts = bloodworks
            .GroupBy(x => x.Status.BloodPressureSystolic)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, y => y.Count());
        BloodPressureDiastolicCounts = bloodworks
            .GroupBy(x => x.Status.BloodPressureDiastolic)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, y => y.Count());
    
        CholesterolTotalCounts = bloodworks
            .GroupBy(x => x.Status.CholesterolTotal)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, y => y.Count());
        CholesterolHdlCounts = bloodworks
            .GroupBy(x => x.Status.CholesterolHdl)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, y => y.Count());
        CholesterolLdlCounts = bloodworks
            .GroupBy(x => x.Status.CholesterolLdl)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, y => y.Count());
    }

    public void SetQuestionnairesStats(List<Questionnaire> questionnaires)
    {
        ExerciseWeeklyMinutesCounts = questionnaires
            .GroupBy(x => x.Status.ExerciseWeeklyMinutes)
            .ToDictionary(x => x.Key, y => y.Count());
        SleepQualityCounts = questionnaires
            .GroupBy(x => x.Status.SleepQuality)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, y => y.Count());
        StressLevelsCounts = questionnaires
            .GroupBy(x => x.Status.StressLevels)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, y => y.Count());
        DietQualityCounts = questionnaires
            .GroupBy(x => x.Status.DietQuality)
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, y => y.Count());
    }
}