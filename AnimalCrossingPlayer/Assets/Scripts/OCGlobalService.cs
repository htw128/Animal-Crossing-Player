using System;
using System.Globalization;
using UnityEngine;

public class OCGlobalService : MonoBehaviour
{
    public static OCGlobalService Instance { get; private set; }
    
    public DateTime Now { get; private set; }
    public WeatherStates WeatherState { get; private set; }
    public enum WeatherStates
    {
        None = 0,
        Sunny = 1,
        Rain,
        Snow
    }
    
    public int targetFrameRate = 60;
    
    private void Awake()
    {
        if ((bool)Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        CultureInfo.CurrentCulture = new CultureInfo("zh-cn");
    }

    private void Update()
    {
        Now = DateTime.Now;
        UpdateTimeToWwise(Now);
    }
    
    private void UpdateTimeToWwise(DateTime now)
    {
        int hour = now.Hour;
        int minute = now.Minute;

        float timeToWwise = hour + minute / 60f;

        AkUnitySoundEngine.SetRTPCValue("Time", timeToWwise);
    }
}
