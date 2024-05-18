using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI stoneValue;
    public TextMeshProUGUI foodValue;
    public TextMeshProUGUI waterValue;
    private void FixedUpdate()
    {
        UpdateResourceText();
    }

    void UpdateResourceText()
    {
        stoneValue.text = Resource.instance.GetStone().ToString();
        foodValue.text = Resource.instance.GetFood().ToString();
        waterValue.text = Resource.instance.GetWater().ToString();
    }
}
