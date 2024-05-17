using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeResources : MonoBehaviour
{
    private const int resourcePerSecond = 1;

    #region multipliers
    private int foodM = 1;
    private int waterM = 1;
    private int stoneM = 1;
    #endregion

    #region resources
    private int food = 0 ;
    private int water = 0;
    #endregion

    
    private void FixedUpdate() // 2 times per second
    {
        GainResources();
    }

    public void GainResources()
    {
        Resource.instance.GainStone(resourcePerSecond * stoneM);
        food += resourcePerSecond * foodM;
        water += resourcePerSecond * waterM;

    }

    public void SetResourceProductionSpeed(SourceType type)
    {
        switch (type)
        {
            case SourceType.Stone:
                stoneM *= 2;
                break;
            case SourceType.Water:
                waterM *= 2;
                break;
            case SourceType.Food:
                foodM *= 2;
                break;
        }
    }

    public void TrainSoldier(int foodToTrainSoldier, int waterToTrainSoldier)
    {
        food -= foodToTrainSoldier;
        water -= waterToTrainSoldier;
    }

    public bool CanTrainSoldier(int food , int water)
    {
        return this.food >= food && this.water >= water;
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
