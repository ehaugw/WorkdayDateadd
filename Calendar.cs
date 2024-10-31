using DateTimeExtensions;

public class Calendar
{
    IWorkdays m_workdays;
    IHolidays m_holidays;

    public Calendar(IWorkdays _workdays, IHolidays _holidays)
    {
        m_workdays = _workdays;
        m_holidays = _holidays;
    }

    public DateTime AddWorkingDays(DateTime _date, float _workingDays)
    {
        // assert start_date.microsecond == 0, "task does not require microsecond accuracy"
        // assert working_days != 0, "unspecified behaviour"
        float daysToAdd = _workingDays;

        // disregard time before workday start
        DateTime resultDate = _date;
        if (TimeOnly.FromDateTime(resultDate) < m_workdays.GetWorkdayStart())
        {
            resultDate = resultDate.SetTimeOnly(m_workdays.GetWorkdayStart());
        }

        // disregard time after workday end
        if (TimeOnly.FromDateTime(resultDate) > m_workdays.GetWorkdayEnd())
        {
            resultDate = resultDate.SetTimeOnly(m_workdays.GetWorkdayEnd());
        }

        // add the seconds passed into the workin day to the working days to be added and set time to start of day
        // this is required if the fraction of the initially passed working day plus the fraction part of working
        // days to be added is greater than one
        daysToAdd += ((float)m_workdays.SecondsSinceStart(TimeOnly.FromDateTime(resultDate)))
            / ((float)m_workdays.WorkdayLengthInSeconds());
        resultDate = resultDate.SetTimeOnly(m_workdays.GetWorkdayStart());

        // while there are days to add, add them, and only subtract them from daysToAdd if the new date is not a
        // holiday
        while (daysToAdd >= 1)
        {
            resultDate = resultDate.AddDays(1);
            if (!m_holidays.IsHoliday(resultDate))
            {
                daysToAdd -= 1;
            }
        }

        // while there are days to add backwards, add them, and only subtract them from daysToAdd if the new date
        // is not a holiday
        while (daysToAdd < 0)
        {
            resultDate = resultDate.AddDays(-1);
            if (!m_holidays.IsHoliday(resultDate))
            {
                daysToAdd += 1;
            }
        }

        // add the last fraction of a workday as a working day duration * that fraction to the date, which starts
        // at working day start
        if (daysToAdd > 0)
        {
            resultDate = resultDate.AddSeconds(m_workdays.WorkdayLengthInSeconds() * daysToAdd);
        }
        resultDate = resultDate.SetTimeOnly(new TimeOnly(resultDate.Hour, resultDate.Minute, 0));

        return resultDate;
    }
}
