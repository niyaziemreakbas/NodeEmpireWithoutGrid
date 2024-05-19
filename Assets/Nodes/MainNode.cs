using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainNode : MonoBehaviour
{


    public int foodToTrainSoldier = 10;
    public int waterToTrainSoldier = 10;
    private Resource resources;
    private Node thisNode;


    private void Awake()
    {
        resources = GetComponent<Resource>();   
        thisNode = GetComponent<Node>();    
    }


    private void FixedUpdate()
    {
        if (CanTrainSoldier())
        {
            TrainSoldier();
            thisNode.soldierCount++;
            thisNode.UpdateSoldierCountText();
        }
    }


    public void TrainSoldier()
    {
        resources.SpendWater(waterToTrainSoldier);
        resources.SpendFood(foodToTrainSoldier);
    }

    public bool CanTrainSoldier()
    {
        return resources.GetFood() >= foodToTrainSoldier && resources.GetWater() >= waterToTrainSoldier;
    }
    private void OnDestroy() {
        UIHelper.Instance.ShowLoseScreen();
    }
}
