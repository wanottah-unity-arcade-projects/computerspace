
using UnityEngine;
using UnityEngine.UI;


// Audio Sound Effects Volume Controller v2020.08.31


public class SFXVolumeController : MonoBehaviour
{
    // reference to audio controller script
    private AudioController audioController;

    private Text sfxVolumeControlText;


    
    private void Start()
    {
        audioController = AudioController.instance;

        sfxVolumeControlText = GetComponent<Text>();

        GetComponentInParent<Slider>().onValueChanged.AddListener(SFXVolumeControl);
    }


    public void SFXVolumeControl(float sfxVolume)
    {
        audioController.SetSFXVolume(sfxVolume);

        sfxVolumeControlText.text = (sfxVolume * 10).ToString("0");
    }


} // end of class
