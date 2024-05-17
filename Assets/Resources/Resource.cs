using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public static Resource instance;


    int stone = 0;
    int gold = 0;

    private void Awake()
    {
        instance = this;
    }

    public void GainStone(int stone)
    {
        this.stone += stone;
    }

    public int GetStone()
    {
        return stone;
    }

    public int GetGold()
    {
        return gold;
    }

}
