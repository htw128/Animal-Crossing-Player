using System;
using UnityEngine;

public class OCMusicController : MonoBehaviour
{
    public void UpdateVolume(float volume)
    {
        AkUnitySoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.VOLUME, volume);
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
        AkUnitySoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.VOLUME, volume);
        AkUnitySoundEngine.PostEvent(AK.EVENTS.SET_STATE_SUNNY, gameObject);
        AkUnitySoundEngine.PostEvent(AK.EVENTS.PLAY_CITY, gameObject);
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
        uint eventID;

        switch (newState)
        {
            case OCGlobalService.WeatherStates.Rainy:
                eventID = AK.EVENTS.SET_STATE_RAINY;
                break;
            
            case OCGlobalService.WeatherStates.Snowy:
                eventID = AK.EVENTS.SET_STATE_SNOWY;
                break;
            
            case OCGlobalService.WeatherStates.None:
            case OCGlobalService.WeatherStates.Sunny:
            default:
                eventID = AK.EVENTS.SET_STATE_SUNNY;
                break;
        }
        AkUnitySoundEngine.PostEvent(eventID, gameObject);
    }

}
