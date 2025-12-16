using UnityEngine;
using UnityEngine.UI;

public class OCWeatherUpdater : MonoBehaviour
{
    
    private int _timer = 60;
    private int _lastSecond;
    private Text _weatherConditionBox;
    private Text _cityNameBox;
    private Button _refreshButton;

    
    public void OnRefreshButtonClicked()
    {
        OCGlobalService.Instance.UpdateWeatherState();
        _refreshButton.enabled = false;
    }

    private void Awake()
    {
        _weatherConditionBox = transform.Find("Weather State").GetComponent<Text>();
        _cityNameBox = transform.Find("City").GetComponent<Text>();
        _refreshButton = transform.Find("Refresh").GetComponent<Button>();
    }


    // Update is called once per frame
    void Update()
    {
        _weatherConditionBox.text = OCGlobalService.Instance.CurrentWeather.current.condition.text;
        _cityNameBox.text = OCGlobalService.Instance.CurrentWeather.location.name;
        
    }

}
