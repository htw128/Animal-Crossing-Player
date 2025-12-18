using System;
using UnityEngine;

public class OCMusicController : MonoBehaviour
{
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
        AkUnitySoundEngine.PostEvent("Set_State_Sunny", gameObject);
        AkUnitySoundEngine.PostEvent("Play_City", gameObject);
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
