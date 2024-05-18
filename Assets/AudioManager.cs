using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool SoundOn = true;

    #region clips
    [SerializeField] private AudioClip simpleButtonClickSF;
    [SerializeField] private AudioClip explosionSF;
    [SerializeField] private AudioClip nodeConstructedSF;
    #endregion

    private AudioSource audioSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(audioSource);
    }

    public void SimpleButtonClickSF()
    {
        if(SoundOn)
        {
            audioSource.PlayOneShot(simpleButtonClickSF);
        }
        
    }

    public void ExplosionSF()
    {
        if(SoundOn)
        {
            audioSource.PlayOneShot(explosionSF);
        }
    }

    public void NodeConstructedSF()
    {
        if(SoundOn)
        {
            audioSource.PlayOneShot(nodeConstructedSF);
        }
    }
}
