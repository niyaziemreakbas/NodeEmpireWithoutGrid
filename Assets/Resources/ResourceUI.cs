using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI stoneValue;
    public TextMeshProUGUI goldValue;
    private void FixedUpdate()
    {
        goldValue.text = Resource.instance.GetGold().ToString();
        stoneValue.text = Resource.instance.GetStone().ToString();
    }
}
