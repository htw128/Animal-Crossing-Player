using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OCTimeSetter : MonoBehaviour
{
    public Text timerDisplay;
    public Toggle twelveHourToggle;

    private void Start()
    {
        twelveHourToggle.onValueChanged.AddListener(delegate{OnToggleValueChanged();});
        
        bool isSaved = OCGlobalService.Instance.CacheReader.Exists("isTwelveHour");

        if (!isSaved)
        {
            OCGlobalService.Instance.CacheWriter.Write("isTwelveHour", twelveHourToggle.isOn).Commit();
        }
        else
        {
            twelveHourToggle.isOn = OCGlobalService.Instance.CacheReader.Read<bool>("isTwelveHour");
        }
    }

    private void Update()
    {
        string format;

        if ((bool)twelveHourToggle && twelveHourToggle.isOn)
        {
            format = "yyyy年M月d日 tt h:mm:ss";
            timerDisplay.text = OCGlobalService.Instance.Now.ToString(format, CultureInfo.CurrentCulture);
        }
        else
        {
            format = "yyyy年M月d日 H:mm:ss";
            timerDisplay.text = OCGlobalService.Instance.Now.ToString(format, CultureInfo.CurrentCulture);
        }

    }

    private void OnDestroy()
    {
        twelveHourToggle.onValueChanged.RemoveListener(delegate { OnToggleValueChanged(); });
    }

    private void OnToggleValueChanged()
    {
        OCGlobalService.Instance.CacheWriter.Write("isTwelveHour", twelveHourToggle.isOn).Commit();
    }
}
