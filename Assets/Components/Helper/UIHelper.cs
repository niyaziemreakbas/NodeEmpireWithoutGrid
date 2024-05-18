using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHelper : MonoBehaviour
{
    [SerializeField ]
    private GameObject[] prompts;


    [SerializeField]
    private TMP_Text modeUpdateText;
    [SerializeField]
    private float fadeDuration;
    private float fadeKat;
    private bool modeTextActivated;
    private Color originModeColor;
    Color tempColor;
    public static UIHelper Instance{ get; private set; }   
   
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    private void Start() {
        originModeColor=modeUpdateText.color;
        fadeKat=originModeColor.a/fadeDuration;
    }
    private void Update() {
        if (modeTextActivated)
        {
         
            tempColor.a-=Time.deltaTime*fadeKat;
            modeUpdateText.color=tempColor;
              if (tempColor.a<=0)
            {
                modeTextActivated=false;
                modeUpdateText.gameObject.SetActive(false);
            }
        }    
    }
    public void ShowModeUpdate(int mode){
        modeUpdateText.gameObject.SetActive(true);
        modeTextActivated=true;
        modeUpdateText.color=originModeColor;
        tempColor=modeUpdateText.color;
        string modeText="";
        switch (mode)
        {
            case 0:
                modeText="Build Node";
                break;
            case 1:
                modeText="Import Export Soldier / Attack Enemy";
                break;
            default:
                break;
        }
        modeUpdateText.text = "Your mode is now "+modeText;
    }
    public void ShowUIPrompt(int id){
        foreach (GameObject item in prompts)
        {
            item.SetActive(false);
        }
        prompts[id].SetActive(true);
    }
}
