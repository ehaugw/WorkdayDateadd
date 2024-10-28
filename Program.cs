using DateTimeWorkdayExtensions;

Console.WriteLine("Start of dotnet unit tests for adding work days");

var workdayManager = new WorkdayManager(new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
workdayManager.AddHoliday(new DateOnly(1, 5, 17));
workdayManager.AddHoliday(new DateOnly(2004, 5, 27));

Console.WriteLine(new DateTime(2004, 5, 24, 19, 3, 0).AddWorkingDays(workdayManager, 44));
Console.WriteLine(new DateTime(2004, 5, 24, 18, 5, 0).AddWorkingDays(workdayManager, -5.5f));
Console.WriteLine(new DateTime(2004, 5, 24, 18, 3, 0).AddWorkingDays(workdayManager, -6.7470217f));

Console.WriteLine("End of dotnet unit tests for adding work days");
