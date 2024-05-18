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

    public Resource mainNodeResource;
    private void FixedUpdate()
    {
        UpdateResourceText();
    }

    void UpdateResourceText()
    {
        stoneValue.text = mainNodeResource.GetStone().ToString();
        foodValue.text = mainNodeResource.GetFood().ToString();
        waterValue.text = mainNodeResource.GetWater().ToString();
    }
}
