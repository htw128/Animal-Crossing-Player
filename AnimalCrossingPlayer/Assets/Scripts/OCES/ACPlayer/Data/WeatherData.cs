// 创建新文件：WeatherData.cs

using System;

namespace OCES.ACPlayer.Data
{
    [Serializable]
    public class WeatherResponse
    {
        public Location location;
        public Current current;
    }

    [Serializable]
    public class Location
    {
        public string name;
        public string region;
        public string country;
        public float lat;
        public float lon;
        public string tz_id;
        public long localtime_epoch;
        public string localtime;
    }

    [Serializable]
    public class Current
    {
        public Condition condition;
    }

    [Serializable]
    public class Condition
    {
        public string text;
        public int code;
    }
}