using UnityEngine;
using UnityEngine.UI;

public class OCWeatherUpdater : MonoBehaviour
{
    
    private int _timerDisplay = 60;
    private int _lastSecond;
    private Text _weatherConditionBox;
    private Text _cityNameBox;
    private Text _refreshButtonText;
    private Button _refreshButton;
    
    public void OnRefreshButtonClicked()
    {
        OCGlobalService.Instance.UpdateWeatherState(true);
        _refreshButton.enabled = false;
        _refreshButtonText.fontSize = 34;
        _refreshButtonText.text = $"已刷新\n{_timerDisplay}";

    }

    private void Awake()
    {
        _weatherConditionBox = transform.Find("Weather State").GetComponent<Text>();
        _cityNameBox = transform.Find("City").GetComponent<Text>();
        _refreshButton = transform.Find("Refresh").GetComponent<Button>();
        _refreshButtonText = transform.Find("Refresh/ButtonText").GetComponent<Text>();
    }


    // Update is called once per frame
    void Update()
    {
        _weatherConditionBox.text = OCGlobalService.Instance.CurrentWeather.current.condition.text;
        _cityNameBox.text = OCGlobalService.Instance.CurrentWeather.location.name;
        
        if (_lastSecond != OCGlobalService.Instance.Now.Second && !_refreshButton.enabled)
        {
            _timerDisplay--;
            _refreshButtonText.text = $"已刷新\n{_timerDisplay}";
            _lastSecond = OCGlobalService.Instance.Now.Second;
            
        }

        if (_timerDisplay == 0 && !_refreshButton.enabled)
        {
            _refreshButton.enabled = true;
            _refreshButtonText.fontSize = 68;
            _refreshButtonText.text = "刷新";
            _timerDisplay = 60;
        }
        
    }

}
