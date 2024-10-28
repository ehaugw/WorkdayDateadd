using System;

public class WorkdayManager : IWorkdayManager {
    private TimeOnly m_workdayStart;
    private TimeOnly m_workdayEnd;
    private List<DateOnly> m_additionalHolidays;

    public WorkdayManager(TimeOnly _workdayStart, TimeOnly _workdayEnd)
    {
        m_additionalHolidays = new List<DateOnly>{};
        m_workdayStart = _workdayStart;
        m_workdayEnd = _workdayEnd;
    }

    public TimeOnly GetWorkdayStart()
    {
        return m_workdayStart;
    }

    public TimeOnly GetWorkdayEnd()
    {
        return m_workdayEnd;
    }

    public void AddHoliday(DateOnly holiday)
    {
        // warning about year 1
        m_additionalHolidays.Add(holiday);
    }

    public int SecondsSinceStart(TimeOnly time)
    {
        return (time.Hour - m_workdayStart.Hour) * 3600 + (time.Minute - m_workdayStart.Minute) * 60;
    }

    public int WorkdayLengthInSeconds()
    {
        return SecondsSinceStart(m_workdayEnd);
    }

    public bool IsHoliday(DateTime date)
    {
        return IsHoliday((DateOnly.FromDateTime(date)));
    }

    public bool IsHoliday(DateOnly date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        foreach (var holiday in m_additionalHolidays)
        {
            if (
                    (holiday.Year == 1 || holiday.Year == date.Year)
                    && holiday.Month == date.Month
                    && holiday.Day == date.Day
               )
            {
                return true;
            }
        }

        return false;
    }
}
