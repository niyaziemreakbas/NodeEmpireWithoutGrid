using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public static Resource instance;


    int stone = 0;
    int food = 0;
    int water = 0;

    private void Awake()
    {
        instance = this;
    }

    public void GainStone(int stone)
    {
        this.stone += stone;
    }

    public void GainFood(int food)
    {
        this.food += food;
    }

    public void GainWater(int water)
    {
        this.water += water;
    }

    public void SpendStone(int stone)
    {
        this.stone -= stone;
    }

    public void SpendFood(int food)
    {
        this.food -= food;
    }

    public void SpendWater(int water)
    {
        this.water -= water;
    }

    public int GetStone()
    {
        return stone;
    }

    public int GetFood()
    {
        return food;
    }

    public int GetWater()
    {
        return water;
    }

}
