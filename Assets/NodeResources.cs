using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeResources : MonoBehaviour
{
    #region resource amounts
    public int food = 5;
    public int water = 5;
    public int stone = 5;
    #endregion

    private SourceType sourceType;


    private void FixedUpdate() // 2 times per second
    {
        
        GainResources();

    }

    public void GainResources()
    {
        switch (sourceType)
        {
            case SourceType.Water:
                Resource.instance.GainWater(water);
                print("water"+water);
                break;

            case SourceType.Food:
                Resource.instance.GainFood(food);
                print("food"+food);
                break;

            case SourceType.Stone:
                Resource.instance.GainStone(stone);
                break;
            default:
                break;
        }
    }

    public void SetResourceProductionSpeed(SourceType type)
    {
       sourceType = type;   
    }
        

    public void TrainSoldier(int foodToTrainSoldier, int waterToTrainSoldier)
    {
        Resource.instance.SpendWater(waterToTrainSoldier);
        Resource.instance.SpendFood(foodToTrainSoldier);
    }

    public bool CanTrainSoldier(int food , int water)
    {
        return Resource.instance.GetFood() >= food && Resource.instance.GetWater() >= water;
    }



}
