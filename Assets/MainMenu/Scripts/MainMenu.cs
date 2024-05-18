using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Sprite SoundOnButtonFrame;
    [SerializeField] private Sprite SoundOnIcon;

    [SerializeField] private Sprite SoundOffButtonFrame;
    [SerializeField] private Sprite SoundOffIcon;


    [SerializeField] private Button SoundOnOffButton;
    [SerializeField] private Image icon;


    bool settingsMenuIsOpen;

    public void OnExitButtonPressed()
    {
        AudioManager.instance.SimpleButtonClickSF();
        Application.Quit();
    }

    public void OnSettingButtonPressed()
    {
        AudioManager.instance.SimpleButtonClickSF();
        SoundOnOffButton.gameObject.SetActive(!settingsMenuIsOpen);
        settingsMenuIsOpen = !settingsMenuIsOpen;
    }

    public void OnSoundOnOffButtonPressed()
    {
        AudioManager.instance.SimpleButtonClickSF();

        if (AudioManager.instance.SoundOn) //if sound is on, turn off
        {
            TurnSoundOff();
            
        }
        else
        {
            TurnSoundOn();
        }
    }

    void TurnSoundOn()
    {
        SoundOnOffButton.image.sprite = SoundOnButtonFrame;
        icon.sprite = SoundOnIcon;

        AudioManager.instance.SoundOn = true;
    }

    void TurnSoundOff()
    {
        SoundOnOffButton.image.sprite = SoundOffButtonFrame;
        icon.sprite = SoundOffIcon;

        AudioManager.instance.SoundOn = false;
    }
}
