var workdayManager = new WorkdayManager(new TimeOnly(8, 0, 0), new TimeOnly(16, 0, 0));
var calendar = new Calendar(workdayManager);

workdayManager.AddHoliday(new DateOnly(1, 5, 17));
workdayManager.AddHoliday(new DateOnly(2004, 5, 27));

Console.WriteLine(calendar.AddWorkingDays(new DateTime(2004, 5, 24, 19, 3, 0), 44));
Console.WriteLine(calendar.AddWorkingDays(new DateTime(2004, 5, 24, 18, 5, 0), -5.5f));
Console.WriteLine(calendar.AddWorkingDays(new DateTime(2004, 5, 24, 18, 3, 0), -6.7470217f));
