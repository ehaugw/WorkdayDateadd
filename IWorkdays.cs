using System;

public interface IWorkdays {
    public TimeOnly GetWorkdayStart();
    public TimeOnly GetWorkdayEnd();
    public int SecondsSinceStart(TimeOnly time);
    public int WorkdayLengthInSeconds();
}
