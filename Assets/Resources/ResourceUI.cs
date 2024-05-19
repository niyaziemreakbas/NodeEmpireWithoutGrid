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
        stoneValue.text = Mathf.RoundToInt(mainNodeResource.GetStone()).ToString();
        foodValue.text = Mathf.RoundToInt(mainNodeResource.GetFood()).ToString();
        waterValue.text = Mathf.RoundToInt(mainNodeResource.GetWater()).ToString();
    }
}
