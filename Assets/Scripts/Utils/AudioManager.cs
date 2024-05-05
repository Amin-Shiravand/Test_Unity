using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class AudioManager : Singleton<AudioManager>
{
    private GameObject audioObjectSourceInstance;
    private AudioSource audioSourceInstance;
    private AudioClip buttonClick;
    private AudioClip match;
    private AudioClip error;
    private AudioClip lose;

    public override void Init()
    {
        if( IsInit )
        {
            return;
        }
        base.Init();
        
        buttonClick = GameResourceManager.Instance.LoadAudioClip("Click");
        match = GameResourceManager.Instance.LoadAudioClip("Match");
        error = GameResourceManager.Instance.LoadAudioClip("Error");
        lose = GameResourceManager.Instance.LoadAudioClip("Lose");
        audioObjectSourceInstance = new GameObject("AudioSourceInstance");
        audioSourceInstance = audioObjectSourceInstance.AddComponent<AudioSource>();
        Debug.Assert(buttonClick != null, "ButtonClick clip is null");             
        Debug.Assert(match != null, "Match clip is null");           
        Debug.Assert(error != null, "Error clip is null");  
        Debug.Assert(audioSourceInstance != null, "AudioSource is null");
        Debug.Assert(lose != null, "Lose is null");
    }
    
    public void PlayButton()
    {
        audioSourceInstance.PlayOneShot(buttonClick);
    }
    
    public void PlayError()
    {
        audioSourceInstance.PlayOneShot(error);
    }
    
    public void PlayMatchPairs()
    {
        audioSourceInstance.PlayOneShot(match);
    }
    
    public void LooseGame()
    {
        audioSourceInstance.PlayOneShot(lose);
    }
}