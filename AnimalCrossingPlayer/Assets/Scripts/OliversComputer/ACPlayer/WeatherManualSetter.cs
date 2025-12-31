using Shogoki.TTP.Picker;
using UnityEngine;
using UnityEngine.UI;

namespace OliversComputer.ACPlayer
{
    public class WeatherManualSetter : MonoBehaviour
    {
        private Toggle _manualToggle;
        private Button _applyButton;
        private Dropdown _weatherDropDown;
        private Picker _picker;
        private GameObject _manualOptions;

        private bool _isPickerInitialized;
    
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
        void Start()
        {
            _manualToggle.onValueChanged.AddListener(delegate { SetManualState(_manualToggle);}); }


        // Update is called once per frame
        void Update()
        {
        }

        private void OnDisable()
        {
            _applyButton.onClick.RemoveListener(OnApplyButtonClicked);
        }

        private void OnDestroy()
        {
            _manualToggle.onValueChanged.RemoveListener(delegate { SetManualState(_manualToggle);});
        }
    
        private void SetManualState(Toggle manualToggle)
        {
            GlobalService.Instance.IsManual = _manualToggle.isOn;
            _manualOptions.SetActive(_manualToggle.isOn);
            _weatherDropDown.value = (int)GlobalService.Instance.WeatherState;

            if (!_manualToggle.isOn)
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
