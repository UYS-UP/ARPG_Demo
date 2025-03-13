using System;
using System.Collections;
using System.Collections.Generic;


public class TimeHelper
{

    private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

    //当前时间戳 毫秒级别
    private static long ClientNow()
    {
        return (DateTime.UtcNow.Ticks - epoch) / 10000;//得到毫秒级别的
    }


    //秒级别
    public static long ClientNowSeconds()
    {
        return (DateTime.UtcNow.Ticks - epoch) / 10000000;//得到秒级别
    }

    public static long Now()
    {
        return ClientNow();
    }

    public static int GetHour()
    {
        return DateTime.Now.Hour;
    }

    public static DateTime TimeStampToDateTime(long timeStamp)
    {

        long begtime = timeStamp * 10000;
        DateTime dt_1970 = new DateTime(1970, 1, 1, 8, 0, 0);
        long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
        long time_tricks = tricks_1970 + begtime;//日志日期刻度
        DateTime dt = new DateTime(time_tricks);//转化为DateTime
        return dt;
    }

    //获取日期 
    public static string GetDate(long timeStamp)
    {
        var d = TimeStampToDateTime(timeStamp);
        return d.ToString("yyyy . MM . dd");// HH:mm
    }

    //获取时分
    public static string GetTime(long timeStamp)
    {
        var d = TimeStampToDateTime(timeStamp);
        return d.ToString("HH ： mm");

    }
    /// <summary>
    /// 将秒数转化为00:00:00格式
    /// </summary>
    /// <param name="time">秒数</param>
    /// <returns>00:00:00</returns>
    public static string ToHourTimeFormat(int time)
    {
        int seconds = time;
        //一小时为3600秒 秒数对3600取整即为小时
        int hour = seconds / 3600;
        //一分钟为60秒 秒数对3600取余再对60取整即为分钟
        int minute = seconds % 3600 / 60;
        //对3600取余再对60取余即为秒数
        seconds = seconds % 3600 % 60;
        //返回00:00:00时间格式
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, seconds);
    }
    /// <summary>
    /// 将秒数转化为00:00格式
    /// </summary>
    /// <param name="time">秒数</param>
    /// <returns>00:00:00</returns>
    public static string TominuteTimeFormat(int time)
    {
        int seconds = time;
        int minute = seconds / 60;
        seconds = seconds % 3600 % 60;
        return string.Format("{0:D2}:{1:D2}", minute, seconds);
    }
}
