using Shared.DataModels;

namespace Data.DTOs;

public class DtoClientList
{
    public List<DtoClient> Clients { get; set; } =  new List<DtoClient>();
}

public class DtoClient
{
    public string Id { get; set; }

    public string Name { get; set; }
    
    public DateOnly DateOfBirth { get; set; }

    public Gender Gender { get; set; }
    
    public DtoMedicalData MedicalData { get; set; }  = new DtoMedicalData();
}

public class DtoMedicalData
{
    public DtoBloodWork Bloodwork { get; set; }  = new DtoBloodWork();
    
    public DtoQuestionnaire Questionnaire { get; set; } = new DtoQuestionnaire();
}

public class DtoBloodWork
{
    public DtoCholesterol Cholesterol { get; set; } = new DtoCholesterol();
    
    public int BloodSugar { get; set; }
    
    public DtoBloodPressure BloodPressure { get; set; }  = new DtoBloodPressure();
}

public class DtoBloodPressure
{
    public int Systolic  { get; set; }
    
    public int Diastolic  { get; set; }
}

public class DtoCholesterol
{
    public int Total  { get; set; }
    
    public int Hdl  { get; set; }

    public int Ldl  { get; set; }
}

public class DtoQuestionnaire
{
    public int ExerciseWeeklyMinutes { get; set; }
    public string SleepQuality { get; set; }
    public string StressLevels { get; set; } 
    public string DietQuality { get; set; }
}
