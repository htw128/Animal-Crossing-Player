using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace OliversComputer.ACPlayer
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
            float volume = GlobalService.Instance.CacheReader.Exists("MusicVolume") 
                ? GlobalService.Instance.CacheReader.Read<float>("MusicVolume") : 100f;
            VolumeSlider.value = volume;
            TextBox.text = volume.ToString(CultureInfo.CurrentCulture);
        }
    }
}
