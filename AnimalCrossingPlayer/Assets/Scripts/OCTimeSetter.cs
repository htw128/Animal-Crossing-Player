using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OCTimeSetter : MonoBehaviour
{
    public Text timerDisplay;
    public Toggle twelveHourToggle;

    private bool isPaused;

    // Update is called once per frame
    void Update()
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
}
