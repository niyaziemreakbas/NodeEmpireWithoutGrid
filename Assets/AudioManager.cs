using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool SoundOn = true;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SimpleButtonClickSF()
    {

    }
}
