using System;
using System.Collections;
using System.Globalization;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.Networking;
using OCES.ACPlayer.Data;

namespace OCES.ACPlayer
{
    public class GlobalService : MonoBehaviour
    {
        internal static GlobalService Instance { get; private set; }
    
        internal DateTime Now { get; private set; }
        internal MusicController Music { get; private set; }
        internal enum WeatherStates
        {
            None = -1,
            Sunny = 0,
            Rainy,
            Snowy
        }
    
        internal WeatherStates WeatherState { get; private set; }
        public WeatherResponse CurrentWeather;
        public QuickSaveWriter CacheWriter;
        public QuickSaveReader CacheReader;
    
        internal event Action<WeatherStates> OnWeatherChanged;
        public event Action<WeatherResponse> OnGetNewWeather;
        public event Action<UnityWebRequest> OnGetWeatherFailed;
    
        public static bool HasInstance => Instance != null;
        
        internal bool IsManual;
        public int targetFrameRate = 60; 
        public int ManualHour { get; private set; }
        public string WeatherServiceAPIKey;

        private int _lastHour;
        private WeatherStates _lastWeatherState = WeatherStates.None;
        [Tooltip("到整点后，几秒后切换回音乐")][SerializeField] private int m_delaySecond;
        [SerializeField] private int _cacheDurationSecond = 2700;
        private float m_lastTimeToWwise;
        
    
        public void UpdateWeatherState(bool isForceUpdate = false)
        {
            string url = $"https://api.weatherapi.com/v1/current.json?key={WeatherServiceAPIKey}&q=auto:ip&lang=zh_cmn";

            if (CacheReader.Exists(GameConstants.CACHE_KEY_WEATHER) && CacheReader.Exists(GameConstants.CACHE_KEY_UNIX_TIME) && !isForceUpdate)
            {
                long _timeElapsed = DateTimeOffset.Now.ToUnixTimeSeconds() - CacheReader.Read<long>(GameConstants.CACHE_KEY_UNIX_TIME);
                if (_timeElapsed <= _cacheDurationSecond)
                {
                    BetterDebug.Log("命中缓存");
                    CurrentWeather = CacheReader.Read<WeatherResponse>(GameConstants.CACHE_KEY_WEATHER);
                    SetWeatherState(ParseWeatherState(CurrentWeather));
                    return;
                }
            }

            StartCoroutine(GetWeatherAsync(url));
        }
        
        public bool RestoreWeatherFromCache()
        {
            if (CurrentWeather == null)
            {
                BetterDebug.LogWarning("当前没有缓存的天气数据，已恢复到晴天");
                SetWeatherState(WeatherStates.Sunny);
                return false;
            }

            WeatherStates state = ParseWeatherState(CurrentWeather);
            SetWeatherState(state);
            return true;
        }
    
        private void Awake()
        {
            if ((bool)Instance && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            Music = GetComponent<MusicController>();
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = targetFrameRate;
            CultureInfo.CurrentCulture = new CultureInfo("zh-cn");
            Now = DateTime.Now;
            ManualHour = _lastHour = DateTime.Now.Hour;
            CacheWriter = QuickSaveWriter.Create("Root");
            if (!QuickSaveReader.RootExists("Root"))
            {
                CacheWriter.Commit();
            }
            CacheReader = QuickSaveReader.Create("Root");
        }

        private void Start()
        {
            UpdateWeatherState();
        }

        private void Update()
        {
            Now = DateTime.Now;

            if (!IsManual)
            {
                UpdateTimeToWwise(Now.Hour, Now.Minute, Now.Second);
            }
            else
            {
                UpdateTimeToWwise(ManualHour, 1, 1);
            }

            if (_lastHour != Now.Hour)
            {
                UpdateWeatherState();
                _lastHour = Now.Hour;

                if (!IsManual)
                {
                    ManualHour = Now.Hour;
                }
                
                Music.PlayThemeSong(false);
            
            }
        }
    
        private void UpdateTimeToWwise(int hour, int minute, int second)
        {
            float timeToWwise = hour + minute / 60f;

            if ((minute == 59 && second >= 55) || (minute == 0 && second <= m_delaySecond))
            {
                timeToWwise = -1f;
            }

            if (Mathf.Approximately(m_lastTimeToWwise, timeToWwise)) return;
            AkUnitySoundEngine.SetRTPCValue(AK.GAME_PARAMETERS.TIME, timeToWwise);
            m_lastTimeToWwise = timeToWwise;
        }

        private IEnumerator GetWeatherAsync(string url)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            request.timeout = 10;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                CurrentWeather = JsonUtility.FromJson<WeatherResponse>(request.downloadHandler.text);
                CacheWriter.Write(GameConstants.CACHE_KEY_WEATHER, CurrentWeather)
                    .Write(GameConstants.CACHE_KEY_UNIX_TIME, DateTimeOffset.Now.ToUnixTimeSeconds())
                    .Commit();
                SetWeatherState(ParseWeatherState(CurrentWeather));
                OnGetNewWeather?.Invoke(CurrentWeather);
                BetterDebug.Log($"当前天气实际：{CurrentWeather.current.condition.text}");
            }
            else
            {
                OnGetWeatherFailed?.Invoke(request);
                BetterDebug.LogError($"获取天气信息失败：{request.error}");
            }
        
        }

        private WeatherStates ParseWeatherState(WeatherResponse response)
        {
            if (response?.current?.condition?.text == null)
            {
                BetterDebug.LogWarning("天气数据为空，默认使用晴天。");
                return WeatherStates.Sunny;
            }
            
            string text = response.current.condition.text;

            if (text.Contains("雨"))
            {
                return WeatherStates.Rainy;
            }

            return text.Contains("雪") ? WeatherStates.Snowy : WeatherStates.Sunny;
        }
    
        internal void SetWeatherState(WeatherStates newState)
        {
            if (_lastWeatherState == newState)
                return;

            _lastWeatherState = newState;
            WeatherState = newState;
            BetterDebug.Log($"切换天气状态至{WeatherState.ToString()}");

            OnWeatherChanged?.Invoke(newState);
        }

        internal void SetManualTime(int newTime)
        {
            if (ManualHour == newTime) return;

            ManualHour = newTime;
            BetterDebug.Log($"已收到手动时间,当前为{ManualHour}");
        }
    }
}
