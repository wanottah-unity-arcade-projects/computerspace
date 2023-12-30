
using System;
using UnityEngine;
using UnityEngine.Audio;


// Audio Controller v2020.08.31


/// <summary>
/// AudioController.cs
/// Adapted from '2D Platformer Sci-Fi Game in Unity'
/// by Aaron @ Thinkbot Labs
/// </summary>


//
// created: 22/03/2019
//


// custom audio controller class
[Serializable] public class Sound
{
    // reference to AudioMixer group component
    public AudioMixerGroup audioMixerGroup;

    // reference to AudioSource component
    private AudioSource audioSource;

    // name of audio clip to play
    public string audioClipName;

    // reference to audio clip
    public AudioClip audioClip;

    // audio volume
    public float audioVolume;

    // audio pitch
    public float audioPitch;

    // loop audio
    public bool loopAudio;

    // play audio clip on Awake
    public bool playOnAwake;


    // set audio source
    public void SetAudioSource(AudioSource _audioSource)
    {
        // audioSource
        audioSource = _audioSource;

        audioSource.clip = audioClip;

        audioSource.pitch = audioPitch;

        audioSource.volume = audioVolume;

        audioSource.playOnAwake = playOnAwake;

        audioSource.loop = loopAudio;

        // audioMixer group
        audioSource.outputAudioMixerGroup = audioMixerGroup;
    }


    // play audio
    public void PlayAudio()
    {
        audioSource.Play();
    }


    // stop audio
    public void StopAudio()
    {
        audioSource.Stop();
    }
}


public class AudioController : MonoBehaviour
{
    // reference to audio controller script
    public static AudioController instance;


    // create the singleton
    private void Awake()
    {
        CreateSingleton();
    }


    private void CreateSingleton()
    {
        // if the singleton instance already exists
        if (instance != null)
        {
            // then destroy the instance
            Destroy(gameObject);
        }

        // otherwise . . .
        else
        {
            // create the singleton instance
            instance = this;

            // and set to 'DontDestroyOnLoad'
            DontDestroyOnLoad(gameObject);
        }
    }


    // reference to AudioMixer component
    public AudioMixer audioMixer;

    // create an array for the audio clips
    [SerializeField] private Sound[] audioClipArray = null;


    private void Start()
    {
        // loop through the audio clip array
        for (int audioClips = 0; audioClips < audioClipArray.Length; audioClips++)
        {
            // create a game object for each audio clip
            GameObject audioClipGameObject = new GameObject("Audio Clip: " + audioClips + " " + audioClipArray[audioClips].audioClipName);

            // parent the audio clips under the 'GameController' game object
            audioClipGameObject.transform.SetParent(this.transform);

            // add an 'AudioSource' component to each of the audio clip game objects
            audioClipArray[audioClips].SetAudioSource(audioClipGameObject.AddComponent<AudioSource>());
        }
    }


    // play audio clip
    public void PlayAudioClip(string _audioClipName)
    {
        // loop through the audio clip array
        for (int audioClips = 0; audioClips < audioClipArray.Length; audioClips++)
        {
            // if we have found the audio clip to play
            if (audioClipArray[audioClips].audioClipName == _audioClipName)
            {
                // play the audio clip
                audioClipArray[audioClips].PlayAudio();

                // and return
                return;
            }
        }

    }


    // stop audio
    public void StopAudioClip(string _audioClipName)
    {
        // loop through the audio clip array
        for (int audioClips = 0; audioClips < audioClipArray.Length; audioClips++)
        {
            // if we have found the audio clip to play
            if (audioClipArray[audioClips].audioClipName == _audioClipName)
            {
                // play the audio clip
                audioClipArray[audioClips].StopAudio();

                // and return
                return;
            }
        }
    }



    // set Master volume
    public void SetMasterVolume(float masterVolumeLevel)
    {
        audioMixer.SetFloat("Master Volume Control", Mathf.Log10(masterVolumeLevel) * 20);
    }


    // set Music volume
    public void SetMusicVolume(float musicVolumeLevel)
    {
        audioMixer.SetFloat("Music Volume Control", Mathf.Log10(musicVolumeLevel) * 20);
    }


    // set SFX volume
    public void SetSFXVolume(float sfxVolumeLevel)
    {
        audioMixer.SetFloat("SFX Volume Control", Mathf.Log10(sfxVolumeLevel) * 20);
    }


} // end of class
