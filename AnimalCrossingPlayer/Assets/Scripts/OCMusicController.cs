using UnityEngine;

public class OCMusicController : MonoBehaviour
{
    public void UpdateVolume(float volume)
    {
        AkUnitySoundEngine.SetRTPCValue("Volume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 100f);
        AkUnitySoundEngine.SetRTPCValue("Volume", volume);
        AkUnitySoundEngine.PostEvent("Play_City", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
