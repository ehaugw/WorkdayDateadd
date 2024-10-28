namespace DateTimeWorkdayExtensions
{
    public static class DateTimeWorkdayExtension
    {
        private static DateTime SetTimeOnly(this DateTime _date, TimeOnly _time)
        {
            return new DateTime(_date.Year, _date.Month, _date.Day, _time.Hour, _time.Minute, _time.Second);
        }

        public static void AddWorkingDays(this DateTime _date, IWorkdayManager _workdayManager, float _workingDays)
        {
            // assert start_date.microsecond == 0, "task does not require microsecond accuracy"
            // assert working_days != 0, "unspecified behaviour"
            float daysToAdd = _workingDays;

            // disregard time before workday start
            DateTime resultDate = _date;
            if (TimeOnly.FromDateTime(resultDate) < _workdayManager.GetWorkdayStart())
            {
                resultDate = resultDate.SetTimeOnly(_workdayManager.GetWorkdayStart());
            }

            // disregard time after workday end
            if (TimeOnly.FromDateTime(resultDate) > _workdayManager.GetWorkdayEnd())
            {
                resultDate = resultDate.SetTimeOnly(_workdayManager.GetWorkdayEnd());
            }

            // add the seconds passed into the workin day to the working days to be added and set time to start of day
            // this is required if the fraction of the initially passed working day plus the fraction part of working
            // days to be added is greater than one
            daysToAdd += ((float)_workdayManager.SecondsSinceStart(TimeOnly.FromDateTime(resultDate)))
                / ((float)_workdayManager.WorkdayLengthInSeconds());
            resultDate = resultDate.SetTimeOnly(_workdayManager.GetWorkdayStart());

            while (daysToAdd >= 1)
            {
                resultDate = resultDate.AddDays(1);
                if (!_workdayManager.IsHoliday(resultDate))
                {
                    daysToAdd -= 1;
                }
            }

            while (daysToAdd < 0)
            {
                resultDate = resultDate.AddDays(-1);
                if (!_workdayManager.IsHoliday(resultDate))
                {
                    daysToAdd += 1;
                }
            }

            if (daysToAdd > 0)
            {
                resultDate = resultDate.AddSeconds(_workdayManager.WorkdayLengthInSeconds() * daysToAdd);
            }
            resultDate = resultDate.SetTimeOnly(new TimeOnly(resultDate.Hour, resultDate.Minute, 0));
        }
    }
}
