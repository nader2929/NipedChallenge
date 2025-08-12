using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;

namespace Shared.DataModels;

[PrimaryKey(nameof(Id))]
public class Bloodwork
{
    public int Id { get; set; }
    public string ClientId { get; set; }
    public Client Client { get; set; }
    public int CholesterolTotal { get; set; }
    public int CholesterolHdl { get; set; }
    public int CholesterolLdl { get; set; }
    public int BloodSugar { get; set; }
    public int BloodPressureSystolic { get; set; }
    public int BloodPressureDiastolic { get; set; }
    
    [NotMapped]
    public BloodworkStatus Status { get; set; } = new();

    public void SetStatus(Dictionary<string, Guideline> guidelines)
    {
        Status.CholesterolTotal = GuidelinesChecker.ClassifyMeasurement(CholesterolTotal, guidelines["cholesterol.total"]);
        Status.CholesterolHdl = GuidelinesChecker.ClassifyMeasurement(CholesterolHdl, guidelines["cholesterol.hdl"]);
        Status.CholesterolLdl = GuidelinesChecker.ClassifyMeasurement(CholesterolLdl, guidelines["cholesterol.ldl"]);
        Status.BloodSugar = GuidelinesChecker.ClassifyMeasurement(BloodSugar, guidelines["bloodsugar"]);
        Status.BloodPressureSystolic = GuidelinesChecker.ClassifyMeasurement(BloodPressureSystolic, guidelines["bloodpressure.systolic"]);
        Status.BloodPressureDiastolic = GuidelinesChecker.ClassifyMeasurement(BloodPressureDiastolic, guidelines["bloodpressure.diastolic"]);
    }
}

public class BloodworkStatus
{
    public MeasurementStatus CholesterolTotal { get; set; }
    public MeasurementStatus CholesterolHdl { get; set; }
    public MeasurementStatus CholesterolLdl { get; set; }
    public MeasurementStatus BloodSugar { get; set; }
    public MeasurementStatus BloodPressureSystolic { get; set; }
    public MeasurementStatus BloodPressureDiastolic { get; set; }
}