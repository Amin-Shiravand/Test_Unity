using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class AudioManager : Singleton<AudioManager>
{
    private GameObject audioObjectSourceInstance;
    private AudioSource audioSourceInstance;
    private AudioClip ButtonClick;
    private AudioClip Match;
    private AudioClip Error;

    public override void Init()
    {
        base.Init();

        ButtonClick = GameResourceManager.Instance.LoadAudioClip("Click");
        Match = GameResourceManager.Instance.LoadAudioClip("Match");
        Error = GameResourceManager.Instance.LoadAudioClip("Error");
        audioObjectSourceInstance = new GameObject("AudioSourceInstance");
        audioSourceInstance = audioObjectSourceInstance.AddComponent<AudioSource>();
        Debug.Assert(ButtonClick != null, "ButtonClick clip is null");             
        Debug.Assert(Match != null, "Match clip is null");           
        Debug.Assert(Error != null, "Error clip is null");  
        Debug.Assert(audioSourceInstance != null, "AudioSource is null");
    }
    
    public void PlayButton()
    {
        audioSourceInstance.PlayOneShot(ButtonClick);
    }
    
    public void PlayError()
    {
        audioSourceInstance.PlayOneShot(Error);
    }
    
    public void PlayMatchPairs()
    {
        audioSourceInstance.PlayOneShot(Match);
    }
}