using System;
using System.Collections;
using System.Collections.Generic;


public class TimeHelper
{

    private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

    //��ǰʱ��� ���뼶��
    private static long ClientNow()
    {
        return (DateTime.UtcNow.Ticks - epoch) / 10000;//�õ����뼶���
    }


    //�뼶��
    public static long ClientNowSeconds()
    {
        return (DateTime.UtcNow.Ticks - epoch) / 10000000;//�õ��뼶��
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
        long tricks_1970 = dt_1970.Ticks;//1970��1��1�տ̶�
        long time_tricks = tricks_1970 + begtime;//��־���ڿ̶�
        DateTime dt = new DateTime(time_tricks);//ת��ΪDateTime
        return dt;
    }

    //��ȡ���� 
    public static string GetDate(long timeStamp)
    {
        var d = TimeStampToDateTime(timeStamp);
        return d.ToString("yyyy . MM . dd");// HH:mm
    }

    //��ȡʱ��
    public static string GetTime(long timeStamp)
    {
        var d = TimeStampToDateTime(timeStamp);
        return d.ToString("HH �� mm");

    }
    /// <summary>
    /// ������ת��Ϊ00:00:00��ʽ
    /// </summary>
    /// <param name="time">����</param>
    /// <returns>00:00:00</returns>
    public static string ToHourTimeFormat(int time)
    {
        int seconds = time;
        //һСʱΪ3600�� ������3600ȡ����ΪСʱ
        int hour = seconds / 3600;
        //һ����Ϊ60�� ������3600ȡ���ٶ�60ȡ����Ϊ����
        int minute = seconds % 3600 / 60;
        //��3600ȡ���ٶ�60ȡ�༴Ϊ����
        seconds = seconds % 3600 % 60;
        //����00:00:00ʱ���ʽ
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, seconds);
    }
    /// <summary>
    /// ������ת��Ϊ00:00��ʽ
    /// </summary>
    /// <param name="time">����</param>
    /// <returns>00:00:00</returns>
    public static string TominuteTimeFormat(int time)
    {
        int seconds = time;
        int minute = seconds / 60;
        seconds = seconds % 3600 % 60;
        return string.Format("{0:D2}:{1:D2}", minute, seconds);
    }
}
