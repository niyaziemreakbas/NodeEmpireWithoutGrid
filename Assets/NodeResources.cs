using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeResources : MonoBehaviour
{
    #region resource amounts
    private float food = 0.5f;
    private float water = 0.5f;
    private float stone = 0.5f;
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
                //print("water:" + water);
                break;

            case SourceType.Food:
                resource.GainFood(food);
                //print("food:" + food);
                break;

            case SourceType.Stone:
                resource.GainStone(stone);
                //print("stone:" + stone);
                break;
            default:
                break;
        }
    }

    public void SetResourceProductionSpeed(SourceType type)
    {
       sourceType = type;   
    }
        







}
