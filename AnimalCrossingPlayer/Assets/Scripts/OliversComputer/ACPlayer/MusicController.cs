using System;
using OliversComputer.ACPlayer.ThemeSongEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OliversComputer.ACPlayer
{
    public class MusicController : MonoBehaviour
    {
        public AK.Wwise.Event BellEvent;

        private ThemeSongModel m_themeSongModel;
        private Coroutine m_playCoroutine;
        private const int k_systemSampleRate = 48000;


        public void PlayThemeSong(bool isPreview = false)
        {
            //TODO support preview timber
            if (m_playCoroutine != null)
            {
                StopCoroutine(m_playCoroutine);
                m_playCoroutine = null;
            }

            m_playCoroutine =
                StartCoroutine(
                    m_themeSongModel.PlayThemeSong(
                        m_themeSongModel.m_noteValues,
                        () => m_playCoroutine = null,
                        isPreview
                        )
                    );
        }
        
        public void UpdateVolume(float volume)
        {
            AkUnitySoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.VOLUME, volume);
            GlobalService.Instance.CacheWriter.Write("MusicVolume", volume).Commit();
        }

        private void Awake()
        {
            //m_systemSampleRate = AudioSettings.outputSampleRate;
            m_themeSongModel = new ThemeSongModel(gameObject);
        }

        private void OnEnable()
        {
            GlobalService.Instance.OnWeatherChanged += SetWwiseWeatherState;
            BetterDebug.Log($"Current Sample Rate is {k_systemSampleRate}");
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

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                m_themeSongModel.PlaySingleNote(65);
            }
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
