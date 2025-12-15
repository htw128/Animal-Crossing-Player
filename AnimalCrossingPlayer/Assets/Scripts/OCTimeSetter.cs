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
        UpdateTimeDisplay(OCGlobalService.Instance.Now);
        UpdateTimeToWwise(OCGlobalService.Instance.Now);

    }

    private void UpdateTimeToWwise(DateTime now)
    {
        int hour = now.Hour;
        int minute = now.Minute;

        float timeToWwise = hour + minute / 60f;

        AkUnitySoundEngine.SetRTPCValue("Time", timeToWwise);
    }

    private void UpdateTimeDisplay(DateTime now)
    {
        string format;

        if ((bool)twelveHourToggle && twelveHourToggle.isOn)
        {
            format = "yyyy年M月d日 tt h:mm:ss";
            timerDisplay.text = now.ToString(format, CultureInfo.CurrentCulture);
        }
        else
        {
            format = "yyyy年M月d日 H:mm:ss";
            timerDisplay.text = now.ToString(format, CultureInfo.CurrentCulture);
        }
    }
}
