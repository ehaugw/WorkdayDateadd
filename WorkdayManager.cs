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

    public void AddHoliday(DateOnly _holiday)
    {
        // warning about year 1
        m_additionalHolidays.Add(_holiday);
    }

    public int SecondsSinceStart(TimeOnly _time)
    {
        return (_time.Hour - m_workdayStart.Hour) * 3600 + (_time.Minute - m_workdayStart.Minute) * 60;
    }

    public int WorkdayLengthInSeconds()
    {
        return SecondsSinceStart(m_workdayEnd);
    }

    public bool IsHoliday(DateTime _date)
    {
        return IsHoliday((DateOnly.FromDateTime(_date)));
    }

    public bool IsHoliday(DateOnly _date)
    {
        // weekends are not working days
        if (_date.DayOfWeek == DayOfWeek.Saturday || _date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        // iterate all additional holidays
        foreach (var holiday in m_additionalHolidays)
        {
            // return true if a recurring holiday, or one of the same year shares the current day and month
            if (
                    (holiday.Year == 1 || holiday.Year == _date.Year)
                    && holiday.Month == _date.Month
                    && holiday.Day == _date.Day
               )
            {
                return true;
            }
        }

        // if no holiday cule has been hit, return false because this is a regular day
        return false;
    }
}
