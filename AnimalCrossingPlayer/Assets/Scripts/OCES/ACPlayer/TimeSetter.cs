using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace OCES.ACPlayer
{
    public class TimeSetter : MonoBehaviour
    {
        public Text timerDisplay;
        public Toggle twelveHourToggle;

        private void Start()
        {
            twelveHourToggle.onValueChanged.AddListener(OnToggleValueChanged);
        
            bool isSaved = GlobalService.Instance.CacheReader.Exists(GameConstants.CACHE_KEY_TWELVE_HOUR);

            if (!isSaved)
            {
                GlobalService.Instance.CacheWriter.Write(GameConstants.CACHE_KEY_TWELVE_HOUR, twelveHourToggle.isOn).Commit();
            }
            else
            {
                twelveHourToggle.isOn = GlobalService.Instance.CacheReader.Read<bool>(GameConstants.CACHE_KEY_TWELVE_HOUR);
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
            twelveHourToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            GlobalService.Instance.CacheWriter.Write(GameConstants.CACHE_KEY_TWELVE_HOUR, isOn).Commit();
        }
    }
}
