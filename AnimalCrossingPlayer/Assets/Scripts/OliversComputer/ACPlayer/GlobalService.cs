using System;
using System.Collections;
using System.Globalization;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.Networking;

namespace OliversComputer.ACPlayer
{
    public class GlobalService : MonoBehaviour
    {
        public static GlobalService Instance { get; private set; }
    
        public DateTime Now { get; private set; }
        public enum WeatherStates
        {
            None = -1,
            Sunny = 0,
            Rainy,
            Snowy
        }
    
        public WeatherStates WeatherState { get; private set; }
        public WeatherResponse CurrentWeather;
        public QuickSaveWriter CacheWriter;
        public QuickSaveReader CacheReader;
    
        public event Action<WeatherStates> OnWeatherChanged;
        public event Action<WeatherResponse> OnGetNewWeather;
        public event Action<UnityWebRequest> OnGetWeatherFailed;
    
        public static bool HasInstance => Instance != null;
    
        [HideInInspector]
        public bool IsManual;
        public int targetFrameRate = 60;
        [HideInInspector]
        public int ManualHour { get; private set; }
        public string WeatherServiceAPIKey;

        private int _lastHour;
        private const string _cachedRespondKey = "CachedRespond";
        private const string _cachedUnixTimeKey = "CachedUnixTime";
        private WeatherStates _lastWeatherState = WeatherStates.None;
    
        public void UpdateWeatherState(bool isForceUpdate = false)
        {
            string url = $"https://api.weatherapi.com/v1/current.json?key={WeatherServiceAPIKey}&q=auto:ip&lang=zh_cmn";

            if (CacheReader.Exists(_cachedRespondKey) && CacheReader.Exists(_cachedUnixTimeKey) && !isForceUpdate)
            {
                long _timeElapsed = DateTimeOffset.Now.ToUnixTimeSeconds() - CacheReader.Read<long>(_cachedUnixTimeKey);
                if (_timeElapsed <= 2700)
                {
                    BetterDebug.Log("命中缓存");
                    CurrentWeather = CacheReader.Read<WeatherResponse>(_cachedRespondKey);
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
                Destroy(gameObject);
                return;
            }

            Instance = this;
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
                UpdateTimeToWwise(Now.Hour, Now.Minute);
            }
            else
            {
                UpdateTimeToWwise(ManualHour, 1);
            }

            if (_lastHour != Now.Hour)
            {
                UpdateWeatherState();
                _lastHour = Now.Hour;

                if (!IsManual)
                {
                    ManualHour = Now.Hour;
                }
            
            }
        }
    
        private void UpdateTimeToWwise(int hour, int minute)
        {
            float timeToWwise = hour + minute / 60f;

            AkUnitySoundEngine.SetRTPCValue("Time", timeToWwise);
        }

        private IEnumerator GetWeatherAsync(string url)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            request.timeout = 10;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                CurrentWeather = JsonUtility.FromJson<WeatherResponse>(request.downloadHandler.text);
                CacheWriter.Write(_cachedRespondKey, CurrentWeather)
                    .Write(_cachedUnixTimeKey, DateTimeOffset.Now.ToUnixTimeSeconds())
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
            string text = response.current.condition.text;

            if (text.Contains("雨"))
            {
                return WeatherStates.Rainy;
            }

            return text.Contains("雪") ? WeatherStates.Snowy : WeatherStates.Sunny;
        }
    
        public void SetWeatherState(WeatherStates newState)
        {
            if (_lastWeatherState == newState)
                return;

            _lastWeatherState = newState;
            WeatherState = newState;
            BetterDebug.Log($"切换天气状态至{WeatherState.ToString()}");

            OnWeatherChanged?.Invoke(newState);
        }

        public void SetManualTime(int newTime)
        {
            if (ManualHour == newTime) return;

            ManualHour = newTime;
            BetterDebug.Log($"已收到手动时间,当前为{ManualHour}");
        }

        #region 天气数据类
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
        #endregion
    }
}
