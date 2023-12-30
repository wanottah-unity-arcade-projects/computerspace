
using UnityEngine;
using UnityEngine.UI;


// Audio Master Volume Controller v2020.08.31


public class MasterVolumeController : MonoBehaviour
{
    // reference to audio controller script
    private AudioController audioController;

    private Text masterVolumeControlText;


    // Start is called before the first frame update
    private void Start()
    {
        audioController = AudioController.instance;

        masterVolumeControlText = GetComponent<Text>();

        GetComponentInParent<Slider>().onValueChanged.AddListener(MasterVolumeControl);
    }


    public void MasterVolumeControl(float masterVolume)
    {
        audioController.SetMasterVolume(masterVolume);

        masterVolumeControlText.text = (masterVolume * 10).ToString("0");
    }


} // end of class
