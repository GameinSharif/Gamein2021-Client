using System;

[Serializable]
public class CustomDateTime
{
    public CustomDate date;
    public CustomTime time;

    public CustomDateTime(DateTime dateTime)
    {
        this.date = new CustomDate(dateTime.Year, dateTime.Month, dateTime.Day);
        this.time = new CustomTime(dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond * 1_000_000);
    }

    public override string ToString()
    {
        return date.day.ToString().PadLeft(2, '0') + "/" + date.month.ToString().PadLeft(2, '0') + "/" + date.year.ToString().PadLeft(4, '0');
    }
}

[Serializable]
public class CustomDate
{
    public int year;
    public int month;
    public int day;

    public CustomDate(int year, int month, int day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
    }

    override
    public string ToString()
    {
        return year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00");
    }
}

[Serializable]
public class CustomTime
{
    public int hour;
    public int minute;
    public int second;
    public int nano;

    public CustomTime(int hour, int minute, int second, int nano)
    {
        this.hour = hour;
        this.minute = minute;
        this.second = second;
        this.nano = nano;
    }
}