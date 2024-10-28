from datetime import datetime, time, timedelta


class WorkdayManager:
    workday_start = None
    workday_end = None
    additional_holidays = None

    def __init__(self, workday_start, workday_end):
        self.additional_holidays  = []
        self.set_workday_start(workday_start)
        self.set_workday_end(workday_end)

    def set_workday_start(self, workday_start):
        self.workday_start = workday_start

    def set_workday_end(self, workday_end):
        self.workday_end = workday_end

    def add_holiday(self, year, month, day):
        assert year != 0, "year 0 can not be represented in python DateTime"
        assert year != 1, "year 1 is used to indicate recurring holidays, if this is your intention, use -1 to make it"
        "clear"
        if year == -1:
            year = 1

        self.additional_holidays.append(datetime(year=year, month=month, day=day))

    def workday_length_seconds(self):
        return self.seconds_since_start(self.workday_end)

    def seconds_since_start(self, time):
        return (time.hour - self.workday_start.hour) * 3600 \
            + (time.minute - self.workday_start.minute) * 60

    def is_holiday(self, date: datetime) -> bool:
        if date.weekday() >= 5:     # saturday or sunday
            return True

        for additional_holiday in self.additional_holidays:
            if additional_holiday.year == 1 or additional_holiday.year == date.year:
                if additional_holiday.month == date.month and additional_holiday.day == date.day:
                    return True

        return False


def add_working_days(start_date: datetime, working_days: float, workday_manager):
    assert start_date.microsecond == 0, "task does not require microsecond accuracy"
    assert working_days != 0, "unspecified behaviour"

    result_date: datetime = start_date

    # disregard time before workday start
    if result_date.time() < workday_manager.workday_start:
        result_date = datetime.combine(result_date.date(), workday_manager.workday_start)

    # disregard time after workday end
    if result_date.time() > workday_manager.workday_end:
        result_date = datetime.combine(result_date.date(), workday_manager.workday_end)

    # add the seconds passed into the workin day to the working days to be added and set time to start of day
    # this is required if the fraction of the initially passed working day plus the fraction part of working days
    # to be added is greater than one
    working_days += workday_manager.seconds_since_start(result_date.time()) \
        / workday_manager.workday_length_seconds()
    result_date = result_date.combine(result_date.date(), workday_manager.workday_start)

    while working_days >= 1:
        result_date += timedelta(days=1)
        if not workday_manager.is_holiday(result_date):
            working_days -= 1

    while working_days < 0:
        result_date += timedelta(days=-1)
        if not workday_manager.is_holiday(result_date):
            working_days += 1

    if working_days > 0:
        result_date = result_date + timedelta(seconds=workday_manager.workday_length_seconds() * working_days)

        #  if result_date.second > 30:
        #      result_date = result_date + timedelta(minutes=1)

        result_date = result_date.replace(second=0, microsecond=0)  # specs ignores seconds and microseconds

    return result_date


if __name__ == "__main__":
    print("Start of Python unit tests for adding work days")

    workday_manager: WorkdayManager = WorkdayManager(workday_start=time(hour=8), workday_end=time(hour=16))
    workday_manager.add_holiday(year=-1, month=5, day=17)
    workday_manager.add_holiday(year=2004, month=5, day=27)

    # EASE OF DEVELOPMENT CASES
    # test first base case from task, with whole work days
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=19, minute=3),
        working_days=44,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=7, day=27, hour=8)

    # test second base case from task, with whole days, at beginning of work day
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=8, minute=0),
        working_days=-1,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=5, day=21, hour=8, minute=0)     # loops through a weekend

    # test second base case from task, with whole days, at mid of work day
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=12, minute=0),
        working_days=-1,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=5, day=21, hour=12, minute=0)     # loops through a weekend

    # test second base case from task, with whole days, at any working time
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=13, minute=12),
        working_days=-1,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=5, day=21, hour=13, minute=12)     # loops through a weekend

    # test second base case from task, with float days within working hours
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=16, minute=0),
        working_days=-1.125,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=5, day=21, hour=15, minute=0)     # loops through a weekend

    # test second base case from task, with float days from after working hours into same day working hours
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=17, minute=0),
        working_days=-1.125,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=5, day=21, hour=15, minute=0)     # loops through a weekend

    # unspecified, but I consider the day to HAVE ended at 16:00, which means one working day before is same day 08:00
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=16, minute=0),
        working_days=-1,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=5, day=24, hour=8, minute=0)     # loops through a weekend


    # SPEC TEST CASES
    # test zeroth base case from task
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=18, minute=5),
        working_days=-5.5,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=5, day=14, hour=12, minute=0)

    # test first base case from task
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=19, minute=3),
        working_days=44.723656,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=7, day=27, hour=13, minute=47)

    # test second base case from task is wrong
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=18, minute=3),
        working_days=-6.7470217,
        workday_manager=workday_manager
    ) != datetime(year=2004, month=5, day=13, hour=10, minute=2)
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=18, minute=3),
        working_days=-6.7470217,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=5, day=13, hour=10, minute=1)
    
    seconds_to_subtract: float = 0.7470217 * 8 * 3600
    seconds_into_day = 16 * 3600 - seconds_to_subtract
    minutes_into_hour = (seconds_into_day % 3600) / 60
    #  print(minutes_into_hour)
    #  > 1.4295840000000075
    # this is less than one and a half minute, which means that no matter if you round down or to closest whole minute
    #, the answer should be 10:01, rather than 10:02

    # test third base case from task
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=8, minute=3),
        working_days=12.782709,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=6, day=10, hour=14, minute=18)

    # test fourth base case from task
    assert add_working_days(
        start_date=datetime(year=2004, month=5, day=24, hour=7, minute=3),
        working_days=8.276628,
        workday_manager=workday_manager
    ) == datetime(year=2004, month=6, day=4, hour=10, minute=12)
    print("End of Python unit tests for adding work days")
