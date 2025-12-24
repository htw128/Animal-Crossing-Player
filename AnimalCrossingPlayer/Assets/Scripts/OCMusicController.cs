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
        uint eventID = newState switch
        {
            OCGlobalService.WeatherStates.None => AK.EVENTS.SET_STATE_SUNNY,
            OCGlobalService.WeatherStates.Rainy => AK.EVENTS.SET_STATE_RAINY,
            OCGlobalService.WeatherStates.Snowy => AK.EVENTS.SET_STATE_SNOWY,
            _ => AK.EVENTS.SET_STATE_SUNNY
        };

        AkUnitySoundEngine.PostEvent(eventID, gameObject);
    }

}
