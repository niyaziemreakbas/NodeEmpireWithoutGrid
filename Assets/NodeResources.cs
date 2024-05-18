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
    private Node mainNode;
    private Resource resource;
    private Node node;

    private void Start()
    {
        node=GetComponent<Node>();
        mainNode = GetComponent<Node>();

        while (mainNode.GetBackNode() != null)
        {
            mainNode = mainNode.GetBackNode();
        }

        resource = mainNode.GetComponent<Resource>();

    }

    private void FixedUpdate()
    {
        if (!node.GetBuilded())
        {
            return;
        }        
        GainResources();

    }

    public void GainResources()
    {
        
        switch (sourceType)
        {
            case SourceType.Water:
                resource.GainWater(water);
                print("water"+water);
                break;

            case SourceType.Food:
                resource.GainFood(food);
                print("food"+food);
                break;

            case SourceType.Stone:
                resource.GainStone(stone);
                print("stone girdi");
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
        resource.SpendWater(waterToTrainSoldier);
        resource.SpendFood(foodToTrainSoldier);
    }

    public bool CanTrainSoldier(int food , int water)
    {
        return resource.GetFood() >= food && resource.GetWater() >= water;
    }



}
