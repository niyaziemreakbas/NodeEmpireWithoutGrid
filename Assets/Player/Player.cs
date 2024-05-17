using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    int stone = 0;
    int gold = 0;

    public TextMeshProUGUI stoneValue;
    public TextMeshProUGUI goldValue;

    private void Update()
    {
        goldValue.text = gold.ToString();
        stoneValue.text = gold.ToString();
    }

}
