using System;

public interface IWorkdayManager {
    public TimeOnly GetWorkdayStart();
    public TimeOnly GetWorkdayEnd();
    public int SecondsSinceStart(TimeOnly time);
    public int WorkdayLengthInSeconds();
    public bool IsHoliday(DateTime date);
    public bool IsHoliday(DateOnly date);
}
