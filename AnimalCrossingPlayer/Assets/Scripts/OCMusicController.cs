using System;
using UnityEngine;

public class OCMusicController : MonoBehaviour
{
    public int SwitchTime;

    private bool _clearedThisHour;
    private bool _switchedThisHour;
    
    public void UpdateVolume(float volume)
    {
        AkUnitySoundEngine.SetRTPCValue("Volume", volume);
        OCGlobalService.Instance.CacheWriter.Write("MusicVolume", volume).Commit();
    }
    
    
    private void OnEnable()
    {
        OCGlobalService.Instance.OnWeatherChanged += SetWwiseWeatherState;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float volume = OCGlobalService.Instance.CacheReader.Exists("MusicVolume") 
            ? OCGlobalService.Instance.CacheReader.Read<float>("MusicVolume") : 100f;
        AkUnitySoundEngine.SetRTPCValue("Volume", volume);
        AkUnitySoundEngine.PostEvent("Play_City", gameObject);
    }

    private void Update()
    {
        int minute = OCGlobalService.Instance.Now.Minute;
        int second = OCGlobalService.Instance.Now.Second;

        if (minute == 59 && second >= 59 && !_clearedThisHour)
        {
            AkUnitySoundEngine.PostEvent("Set_State_None", gameObject);
            _clearedThisHour = true;
            _switchedThisHour = false;
        }

        if (minute == 0 && second >= SwitchTime && !_switchedThisHour)
        {
            SetWwiseWeatherState(OCGlobalService.Instance.WeatherState);
            _clearedThisHour = false;
            _switchedThisHour = true;
        }
    }

    private void OnDisable()
    {
        if (OCGlobalService.HasInstance)
        {
            OCGlobalService.Instance.OnWeatherChanged -= SetWwiseWeatherState;
        }
    }

    private void SetWwiseWeatherState(OCGlobalService.WeatherStates newState)
    {
        string eventName = newState switch
        {
            OCGlobalService.WeatherStates.None => "Set_State_None",
            OCGlobalService.WeatherStates.Rainy => "Set_State_Rainy",
            OCGlobalService.WeatherStates.Snowy => "Set_State_Snowy",
            _ => "Set_State_Sunny"
        };

        AkUnitySoundEngine.PostEvent(eventName, gameObject);
    }

}
