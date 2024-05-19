using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{


    public float stone = 0;
    public float food = 0;
    public float water = 0;

    private void Start() {
        GetComponent<Node>().SetBuilded(true);
    }

    public void GainStone(float stone)
    {
        this.stone += stone;
    }

    public void GainFood(float food)
    {
        this.food += food;
    }

    public void GainWater(float water)
    {
        this.water += water;
    }

    public void SpendStone(float stone)
    {
        this.stone -= stone;
    }

    public void SpendFood(float food)
    {
        this.food -= food;
    }

    public void SpendWater(float water)
    {
        this.water -= water;
    }

    public float GetStone()
    {
        return stone;
    }

    public float GetFood()
    {
        return food;
    }

    public float GetWater()
    {
        return water;
    }

}
