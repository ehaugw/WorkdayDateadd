public interface IHolidays
{
    public void AddHoliday(DateOnly _holiday);
    public bool IsHoliday(DateTime _date);
    public bool IsHoliday(DateOnly _date);
}
