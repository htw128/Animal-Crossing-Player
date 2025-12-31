using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace OliversComputer.ACPlayer
{
    public class TimeSetter : MonoBehaviour
    {
        public Text timerDisplay;
        public Toggle twelveHourToggle;

        private void Start()
        {
            twelveHourToggle.onValueChanged.AddListener(delegate{OnToggleValueChanged();});
        
            bool isSaved = GlobalService.Instance.CacheReader.Exists("isTwelveHour");

            if (!isSaved)
            {
                GlobalService.Instance.CacheWriter.Write("isTwelveHour", twelveHourToggle.isOn).Commit();
            }
            else
            {
                twelveHourToggle.isOn = GlobalService.Instance.CacheReader.Read<bool>("isTwelveHour");
            }
        }

        private void Update()
        {
            string format;

            if ((bool)twelveHourToggle && twelveHourToggle.isOn)
            {
                format = "yyyy年M月d日 tt h:mm:ss";
                timerDisplay.text = GlobalService.Instance.Now.ToString(format, CultureInfo.CurrentCulture);
            }
            else
            {
                format = "yyyy年M月d日 H:mm:ss";
                timerDisplay.text = GlobalService.Instance.Now.ToString(format, CultureInfo.CurrentCulture);
            }

        }

        private void OnDestroy()
        {
            twelveHourToggle.onValueChanged.RemoveListener(delegate { OnToggleValueChanged(); });
        }

        private void OnToggleValueChanged()
        {
            GlobalService.Instance.CacheWriter.Write("isTwelveHour", twelveHourToggle.isOn).Commit();
        }
    }
}
