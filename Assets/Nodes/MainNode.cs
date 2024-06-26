using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainNode : MonoBehaviour
{
    static int deadEnemyCounter = 0;

    public int foodToTrainSoldier = 10;
    public int waterToTrainSoldier = 10;
    private Resource resources;
    private Node thisNode;

    private int maxEnemyCount = 4;

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
        if (gameObject.CompareTag("PlayerNode"))
        {
            
            UIHelper.Instance.ShowLoseScreen();

        }else if(gameObject.CompareTag("EnemyNode")){

            if(deadEnemyCounter >= maxEnemyCount)
            {
                UIHelper.Instance.ShowWinScreen();
            }
        
        }
    }
}
