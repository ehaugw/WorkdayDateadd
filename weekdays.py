def is_leap_year(year: int) -> bool:
    return (year % 4 == 0) and (not (year % 100 == 0) or year % 400 == 0)


def get_days_in_month(year: int, month: int) -> int:
    """Return the number of days in a month for any year"""
    zero_indexed_month: int = month - 1

    base: int = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][zero_indexed_month]

    if zero_indexed_month == 1 and is_leap_year(year):
        return base + 1

    return base


def get_week_day_as_int(year: int, month: int, day: int) -> int:
    "Return the week day of any date from 1st of Janauary 0000 and onwards, where Monday is 0 and Sunday is 6"""
    assert year >= 0, "this has not been tested for negative years"

    total_modifier: int = 5             # because 1st of January 0000 was a Saturday
    total_modifier += int((365 + 1/4 - 1/100 + 1/400) * year)   # this is the exact number of days in a year
    if (year > 0):                      # 0000 was a leap year, but multiplaying the faction above by 0 won't reflect that
        total_modifier += 1

    for m in range(1, month):
        total_modifier += get_days_in_month(year, m)

    return (total_modifier + day - 1) % 7     # -1 because we use zero indexed, monday is 0


if __name__ == "__main__":
    # test leap year rules
    assert is_leap_year(0) == True, "0000 is a leap year"
    assert is_leap_year(1900) == False, "1900 is not a leap year"
    assert is_leap_year(2000) == True, "2000 is a leap year"
    assert is_leap_year(2024) == True, "2024 is a leap year"
    assert is_leap_year(2025) == False, "2025 is not a leap year"

    # test days in month, uses leap year, so we don't need to test for leap year edge cases here too
    assert get_days_in_month(2024, 1) == 31, "there are 31 days in any January"
    assert get_days_in_month(2024, 2) == 29, "there are 29 days in leap year February"
    assert get_days_in_month(2024, 3) == 31, "there are 31 days in any March"
    assert get_days_in_month(2024, 11) == 30, "there are 30 days in any November"
    assert get_days_in_month(2025, 2) == 28, "there are 28 days in non-leap year February"
    
    # test week day at any date
    assert get_week_day_as_int(0000, 1, 1) == 5, "1st of January 0000 is a Saturday"
    assert get_week_day_as_int(1, 1, 1) == 0, "1st of January 0000 is a Saturday"
    assert get_week_day_as_int(2024, 1, 1) == 0, "1st of Janaury 2024 is a Monday"
    assert get_week_day_as_int(2024, 1, 2) == 1, "2nd of Janaury 2024 is a Tuesday"
    assert get_week_day_as_int(2024, 1, 31) == 2, "31st of Janaury 2024 is a Wednesday"
    assert get_week_day_as_int(2024, 10, 28) == 0, "28th of October 2024 is a Monday"
    assert get_week_day_as_int(2024, 2, 1) == 3, "1st of February 2024 is a Thursday"
