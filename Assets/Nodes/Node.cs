using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int foodToTrainSoldier = 2;
    public int waterToTrainSoldier = 2;
    public TextMeshProUGUI soldierCountText;
    public Vector3 offSet;



    public Node backNode;
    private List<Node> nextNodes;
    private NodeLine backNodeLine;

    public int soldierCount = 0;
    private NodeResources nodeResources;

    Vector2 centerPoint;

    private bool builded;

    public bool isConnectedOnEnemy;
    public Node enemyNode;
    public NodeLine enemyLine;

    private void Awake()
    {
        builded=false;
        nodeResources = GetComponent<NodeResources>();
        Canvas canvas= FindObjectOfType<Canvas>();
        soldierCountText.transform.SetParent(canvas.transform,false);   
    }

    //public Vector2 currentNodePoint; // Belirli bir nokta


    private void Start()
    {
		nextNodes=new List<Node>();
        SetTextPosition();


    }

    void FixedUpdate()
    {
        
        if (!builded)
        {
            return;
        }

        if (nodeResources.CanTrainSoldier(foodToTrainSoldier,waterToTrainSoldier))
        {
            nodeResources.TrainSoldier(foodToTrainSoldier,waterToTrainSoldier);
            soldierCount++;
            UpdateSoldierCountText();
        }

        if (isConnectedOnEnemy)
        {
            if (soldierCount<=0)
            {
                Destroy(enemyLine.gameObject);
                Destroy(gameObject);
            }

            if (enemyLine==null)
            {
            
                isConnectedOnEnemy=false;
                enemyNode=null;
            
            }else{
                soldierCount--;
            }
            
            
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
    public void SetTextPosition(){
        soldierCountText.transform.position = Camera.main.WorldToScreenPoint(transform.position + offSet);
    }
    public void SetEnemyNode(Node node){
        enemyNode=node;
    }
    public void SetEnemyNodeLine(NodeLine nodeLine){
        enemyLine=nodeLine;
    }
    public void SetIsConnectedToEnemy(bool state){
        isConnectedOnEnemy=state;
    }


    private void OnDestroy() {
        
        foreach (Node item in nextNodes)
        {
            Destroy(item.gameObject);
        }

        Destroy(backNodeLine.gameObject);
        
        if(soldierCountText != null)
        {
            Destroy(soldierCountText.gameObject);
        }

        
    }
    public void SetBuilded(bool builded){
        this.builded=builded;
    }
    public void SetBackNodeLine(NodeLine nodeLine){
        this.backNodeLine=nodeLine;
    }
}
