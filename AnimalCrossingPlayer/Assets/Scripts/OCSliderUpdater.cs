using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OCSliderUpdater : MonoBehaviour
{
    public Slider VolumeSlider;
    public Text TextBox;
    
    public void UpdateText()
    {
        TextBox.text = VolumeSlider.value.ToString(CultureInfo.CurrentCulture);
    }

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 100f);
        VolumeSlider.value = volume;
        TextBox.text = volume.ToString(CultureInfo.CurrentCulture);
    }
}
