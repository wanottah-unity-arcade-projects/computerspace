
using UnityEngine;
using UnityEngine.UI;


// Audio Music Volume Controller v2020.08.31


public class MusicVolumeController : MonoBehaviour
{
    // reference to audio controller script
    private AudioController audioController;

    private Text musicVolumeControlText;


    // Start is called before the first frame update
    private void Start()
    {
        audioController = AudioController.instance;

        musicVolumeControlText = GetComponent<Text>();

        GetComponentInParent<Slider>().onValueChanged.AddListener(MusicVolumeControl);
    }


    public void MusicVolumeControl(float musicVolume)
    {
        audioController.SetMusicVolume(musicVolume);

        musicVolumeControlText.text = (musicVolume * 10).ToString("0");
    }


} // end of class
