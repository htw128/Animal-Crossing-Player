using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace OCES.ACPlayer
{
    public class SliderUpdater : MonoBehaviour
    {
        public Slider VolumeSlider;
        public Text TextBox;
    
        public void UpdateText()
        {
            TextBox.text = VolumeSlider.value.ToString(CultureInfo.CurrentCulture);
        }

        private void Start()
        {
            float volume = GlobalService.Instance.CacheReader.Exists(GameConstants.CACHE_KEY_VOLUME) 
                ? GlobalService.Instance.CacheReader.Read<float>(GameConstants.CACHE_KEY_VOLUME) : 100f;
            VolumeSlider.value = volume;
            TextBox.text = volume.ToString(CultureInfo.CurrentCulture);
        }
    }
}
