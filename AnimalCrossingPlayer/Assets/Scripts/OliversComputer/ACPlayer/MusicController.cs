using System;
using UnityEngine;

namespace OliversComputer.ACPlayer
{
    public class MusicController : MonoBehaviour
    {
        public AK.Wwise.Event BellEvent;
        
        public void PlayThemeSong(bool isPreview = false)
        {
            throw new NotImplementedException();
        }
        
        public void UpdateVolume(float volume)
        {
            AkUnitySoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.VOLUME, volume);
            GlobalService.Instance.CacheWriter.Write("MusicVolume", volume).Commit();
        }

        private void OnEnable()
        {
            GlobalService.Instance.OnWeatherChanged += SetWwiseWeatherState;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
            float volume = GlobalService.Instance.CacheReader.Exists("MusicVolume") 
                ? GlobalService.Instance.CacheReader.Read<float>("MusicVolume") : 100f;
            AkUnitySoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.VOLUME, volume);
            AkUnitySoundEngine.PostEvent(AK.EVENTS.SET_STATE_SUNNY, gameObject);
            AkUnitySoundEngine.PostEvent(AK.EVENTS.PLAY_CITY, gameObject);
        }

        private void OnDisable()
        {
            if (GlobalService.HasInstance)
            {
                GlobalService.Instance.OnWeatherChanged -= SetWwiseWeatherState;
            }
        }

        private void SetWwiseWeatherState(GlobalService.WeatherStates newState)
        {
            uint eventID;

            switch (newState)
            {
                case GlobalService.WeatherStates.Rainy:
                    eventID = AK.EVENTS.SET_STATE_RAINY;
                    break;
            
                case GlobalService.WeatherStates.Snowy:
                    eventID = AK.EVENTS.SET_STATE_SNOWY;
                    break;
            
                case GlobalService.WeatherStates.None:
                case GlobalService.WeatherStates.Sunny:
                default:
                    eventID = AK.EVENTS.SET_STATE_SUNNY;
                    break;
            }
            AkUnitySoundEngine.PostEvent(eventID, gameObject);
        }
    }
}
