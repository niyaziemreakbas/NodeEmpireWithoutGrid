using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class NodeHelper : MonoBehaviour
{   
    public Image image;
    public void SetImage(Sprite sprite){
        image.sprite = sprite;
    }
}
