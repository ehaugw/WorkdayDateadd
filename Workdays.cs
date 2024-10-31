using System;

public class Workdays : IWorkdays {
    private TimeOnly m_workdayStart;
    private TimeOnly m_workdayEnd;

    public Workdays(TimeOnly _workdayStart, TimeOnly _workdayEnd)
    {
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

    public int SecondsSinceStart(TimeOnly _time)
    {
        return (_time.Hour - m_workdayStart.Hour) * 3600 + (_time.Minute - m_workdayStart.Minute) * 60;
    }

    public int WorkdayLengthInSeconds()
    {
        return SecondsSinceStart(m_workdayEnd);
    }
}
