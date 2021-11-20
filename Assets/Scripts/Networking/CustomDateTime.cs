using System;
using System.Collections.Generic;

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

    public DateTime ToDateTime()
    {
        return new DateTime(date.year, date.month, date.day, time.hour, time.minute, time.second);
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

    private bool isLeap(int y)
    {
        if (y%100 != 0 && y%4 == 0 || y %400 == 0)
            return true;
     
        return false;
    }
    private int offsetDays(int d, int m, int y)
    {
        int offset = d;
     
        switch (m - 1)
        {
        case 11:
            offset += 30;
            break;
        case 10:
            offset += 31;
            break;
        case 9:
            offset += 30;
            break;
        case 8:
            offset += 31;
            break;
        case 7:
            offset += 31;
            break;
        case 6:
            offset += 30;
            break;
        case 5:
            offset += 31;
            break;
        case 4:
            offset += 30;
            break;
        case 3:
            offset += 31;
            break;
        case 2:
            offset += 28;
            break;
        case 1:
            offset += 31;
            break;
        }
     
        if (isLeap(y) && m > 2)
            offset += 1;
     
        return offset;
    }
    private List<int> RevoffsetDays(int offset, int y, List<int> m2d2)
    {
        int[] month = { 0, 31, 28, 31, 30, 31, 30,
                          31, 31, 30, 31, 30, 31 };
     
        if (isLeap(y))
            month[2] = 29;
     
        int i;
        for (i = 1; i <= 12; i++)
        {
            if (offset <= month[i])
                break;
            offset = offset - month[i];
        }
        m2d2.Add(i);
        m2d2.Add(offset);
        return m2d2;
    }
    private List<int> AddDays(int d1, int m1, int y1, int x)
    {
        int offset1 = offsetDays(d1, m1, y1);
        int remDays = isLeap(y1)?(366-offset1):(365-offset1);
        int y2, offset2;
        if (x <= remDays)
        {
            y2 = y1;
            offset2 = offset1 + x;
        }
        else
        {
            x -= remDays;
            y2 = y1 + 1;
            int y2days = isLeap(y2)?366:365;
            while (x >= y2days)
            {
                x -= y2days;
                y2++;
                y2days = isLeap(y2)?366:365;
            }
            offset2 = x;
        }
        List<int> m2d2 = new List<int>();
        List<int> date = RevoffsetDays(offset2, y2, m2d2);
        date.Add(y2);
        return date;
    }
    
    public CustomDate AddDays(int days)
    {
        List<int> date = AddDays(day, month, year, days);
        CustomDate customDate = new CustomDate(date[2], date[1], date[0]);
        return customDate;
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