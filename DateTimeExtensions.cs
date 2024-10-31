namespace DateTimeExtensions
{
    public static class DateTimeExtension
    {
        public static DateTime SetTimeOnly(this DateTime _date, TimeOnly _time)
        {
            return new DateTime(_date.Year, _date.Month, _date.Day, _time.Hour, _time.Minute, _time.Second);
        }
    }
}
