using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int foodToTrainSoldier = 2;
    public int waterToTrainSoldier = 2;
    public TextMeshProUGUI soldierCountText;
    public Vector3 offSet;

    public Vector2 closestStone;
    public Vector2 closestFood;
    public Vector2 closestWater;

    public double closestStoneDistance;
    public double closestFoodDistance;
    public double closestWaterDistance;

    public Node backNode;
    private List<Node> nextNodes;

    private int soldierCount = 0;
    private NodeResources nodeResources;
    private void Awake()
    {
        nodeResources = GetComponent<NodeResources>();
    }

    private void Start()
    {
		 nextNodes=new List<Node>();
        soldierCountText.transform.position = Camera.main.WorldToScreenPoint(transform.position + offSet);
    }

    void FixedUpdate()
    {
        if (nodeResources.CanTrainSoldier(foodToTrainSoldier,waterToTrainSoldier))
        {
            nodeResources.TrainSoldier(foodToTrainSoldier,waterToTrainSoldier);
            soldierCount++;
            UpdateSoldierCountText();
        }
    }

    void UpdateSoldierCountText()
    {
        soldierCountText.text = soldierCount.ToString();    
    }
    
    public Node GetBackNode(){
        return backNode;
    }
    
    public void SetBackNode(Node newBackNode){
        backNode=newBackNode;
    }
    public List<Node> GetNextNodes(){
        return nextNodes;
    }
    public void SetNextNode(List<Node> newNextNodes){
        nextNodes=newNextNodes;
    }
    public void AddNextNode(Node nextNode){
        nextNodes.Add(nextNode);
    }
}
