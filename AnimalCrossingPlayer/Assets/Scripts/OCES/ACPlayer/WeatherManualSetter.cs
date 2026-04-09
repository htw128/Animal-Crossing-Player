using Shogoki.TTP.Picker;
using UnityEngine;
using UnityEngine.UI;

namespace OCES.ACPlayer
{
    public class WeatherManualSetter : MonoBehaviour
    {
        private Toggle _manualToggle;
        private Button _applyButton;
        private Dropdown _weatherDropDown;
        private Picker _picker;
        private GameObject _manualOptions;

        //private bool _isPickerInitialized; 先留一下，要是两个功能以后还没有用到就删掉
    
        private void Awake()
        {
            _manualToggle = transform.Find("ManualToggle").GetComponent<Toggle>();
            _manualOptions = transform.Find("ManualOptions").gameObject;
            _applyButton = transform.Find("ManualOptions/Apply").GetComponent<Button>();
            _weatherDropDown = transform.Find("ManualOptions/WeatherPicker").GetComponent<Dropdown>();
            _picker = transform.Find("ManualOptions/TimePicker").GetComponent<Picker>();
        }

        private void OnEnable()
        {
            _applyButton.onClick.AddListener(OnApplyButtonClicked);
        
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _manualToggle.onValueChanged.AddListener(SetManualState); 
        }
        

        private void OnDisable()
        {
            _applyButton.onClick.RemoveListener(OnApplyButtonClicked);
        }

        private void OnDestroy()
        {
            _manualToggle.onValueChanged.RemoveListener(SetManualState);
        }
    
        private void SetManualState(bool value)
        {
            GlobalService.Instance.IsManual = value;
            _manualOptions.SetActive(value);
            _weatherDropDown.value = (int)GlobalService.Instance.WeatherState;

            if (!value)
            {
                GlobalService.Instance.RestoreWeatherFromCache();
            }
        }
    
        private void OnApplyButtonClicked()
        {
            GlobalService.Instance.SetWeatherState((GlobalService.WeatherStates)(_weatherDropDown.value));
            GlobalService.Instance.SetManualTime(_picker.PickerValue);
        }
    }
}
