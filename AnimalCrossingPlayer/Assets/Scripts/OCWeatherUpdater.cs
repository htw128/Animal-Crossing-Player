using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OCWeatherUpdater : MonoBehaviour
{
    public int RefreshCooldownTime = 60;
    
    private int _timerDisplay;
    private int _lastSecond;
    private Text _weatherConditionBox;
    private Text _cityNameBox;
    private Text _refreshButtonText;
    private Button _refreshButton;
    private bool _isRefreshing;
    private bool _isRefreshFailed;
    
    public void OnRefreshButtonClicked()
    {
        OCGlobalService.Instance.UpdateWeatherState(true);
        _refreshButton.enabled = false;
        _refreshButtonText.fontSize = 34;
        _refreshButtonText.color = new Color(0.6f, 0.6f, 0.6f);
        
        _weatherConditionBox.fontSize = 70;
        _weatherConditionBox.text = "获取天气中……";
        _cityNameBox.text = "获取位置中……";
        
        _isRefreshing = true;
    }

    private void Awake()
    {
        _weatherConditionBox = transform.Find("Weather State").GetComponent<Text>();
        _cityNameBox = transform.Find("City").GetComponent<Text>();
        _refreshButton = transform.Find("Refresh").GetComponent<Button>();
        _refreshButtonText = transform.Find("Refresh/ButtonText").GetComponent<Text>();
        _timerDisplay = RefreshCooldownTime;
    }

    private void Start()
    {
        OCGlobalService.Instance.OnGetNewWeather += OnGetNewWeather;
        OCGlobalService.Instance.OnGetWeatherFailed += OnGetWeatherFailed;
    }

    // Update is called once per frame
    void Update()
    {
        if (OCGlobalService.Instance.CurrentWeather == null || _isRefreshing)
        {
            _weatherConditionBox.fontSize = 70;
            _weatherConditionBox.text = "获取天气中……";
            _cityNameBox.text = "获取位置中……";
        }
        else if (OCGlobalService.Instance.CurrentWeather != null && !_isRefreshFailed)
        {
            _weatherConditionBox.text = OCGlobalService.Instance.CurrentWeather.current.condition.text;
            _cityNameBox.text = OCGlobalService.Instance.CurrentWeather.location.name;
        }
        
        if (_lastSecond != OCGlobalService.Instance.Now.Second 
            && !_refreshButton.enabled 
            && !_isRefreshing)
        {
            _timerDisplay--;
            _refreshButtonText.text = $"已刷新({_timerDisplay})";
            _lastSecond = OCGlobalService.Instance.Now.Second;
            
        }

        if (_timerDisplay == 0 && !_refreshButton.enabled)
        {
            _refreshButton.enabled = true;
            _refreshButtonText.fontSize = 68;
            _refreshButtonText.color = new Color(0.2f,0.2f,0.2f);
            _refreshButtonText.text = "刷新";
            _timerDisplay = RefreshCooldownTime;
        }
        
    }

    private void OnDisable()
    {
        if (!OCGlobalService.HasInstance) return;
        OCGlobalService.Instance.OnGetNewWeather -= OnGetNewWeather;
        OCGlobalService.Instance.OnGetWeatherFailed -= OnGetWeatherFailed;
    }

    private void OnGetNewWeather(OCGlobalService.WeatherResponse newResponse)
    {
        _isRefreshing = false;
        _isRefreshFailed = false;
        _weatherConditionBox.fontSize = 70;
    }

    private void OnGetWeatherFailed(UnityWebRequest request)
    {
        _isRefreshFailed = true;
        _isRefreshing = false;
        _cityNameBox.text = "地球";
        _weatherConditionBox.fontSize = 40;
        _weatherConditionBox.text = "获取天气信息失败，将继续播放当前天气\n请稍后再试 :(";
    }

}
