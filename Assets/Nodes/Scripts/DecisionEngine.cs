using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DecisionEngine : MonoBehaviour
{
    List<NodeAI> allyNodes=new List<NodeAI>();
    List<NodeAI> attackTargetNodes;
    List<NodeAI> attackTargetNodesAlly;

    List<NodeAI> defendTargetNodes;
    //List<NodeAI> defendTargetNodesAlly;


    int buildCost = 15;

    bool attack = false;

    public int nodeCountUntilAttack;

    public GameObject Node;

    public NodeAI nodePrefab;

    public NodeAI tempNode;
    
    public NodeLine nodeLinePrefab;

    public float lineLength;

    //Kaynaklar� �ekece�imiz sat�r
    public Resource Resource; 



    enum State
    {
        SearchingStone,
        SearchingWater,
        SearchingFood,
        Defence,
        Attack
    }

    Vector3 tempTarget;


    State currentState;
    private void Start() {

        allyNodes.Add(GetComponent<NodeAI>());
        /*
        allyNodes=new List<NodeAI>();
        GoLocation(new Vector3(20,20,0));
        GoLocation(new Vector3(5,20,0));
        */
    }
    private void Update()
    {
        currentState = State.SearchingStone; updateState();
        currentState = State.SearchingWater; updateState();
        currentState = State.SearchingFood; updateState();
        
        if(allyNodes.Count > nodeCountUntilAttack)
        {
            currentState = State.Defence; updateState();
            currentState = State.Attack; updateState();

        }

    }
    void updateState()
    {
        switch (currentState)
        {
            case State.SearchingStone:
                CheckNodesForStone();
                break;
            case State.SearchingWater:
                CheckNodesForWater();
                break;
            case State.SearchingFood:
                CheckNodesForFood();
                break;
            case State.Defence:
                DefenceNode();
                break;
            case State.Attack:
                AttackNode();
                break;

        }
    }

    //Nodelar aras�nda ta�a en yak�n konumu bul
    void CheckNodesForStone()
    {
        NodeAI startNode = tempNode;
        Vector2 currentGoal = Vector2.zero;
        double currentClosestGoalDistance = double.MaxValue;
        foreach (NodeAI node in allyNodes)
        {
            if(currentClosestGoalDistance > node.closestStoneDistance)
            {
                currentClosestGoalDistance = node.closestStoneDistance;
                currentGoal = node.closestStone;
            }
        }
        //Yukar�da bulunuyor ve art�k hedefe gidebilir
        GoLocation(startNode, currentGoal);

    }

    //Nodelar aras�nda suya en yak�n konumu bul
    void CheckNodesForWater()
    {
        NodeAI startNode = tempNode;
        Vector2 currentGoal = Vector2.zero;
        double currentClosestGoalDistance = double.MaxValue;
        foreach (NodeAI node in allyNodes)
        {

            if (currentClosestGoalDistance > node.closestWaterDistance)
            {
                currentClosestGoalDistance = node.closestWaterDistance;
                currentGoal = node.closestWater;
            }
        }
        //Yukar�da bulunuyor ve art�k hedefe gidebilir
        GoLocation(startNode, currentGoal);
    }

    //Nodelar aras�nda yeme�e en yak�n konumu bul
    void CheckNodesForFood()
    {
        NodeAI startNode = tempNode;
        Vector2 currentGoal = Vector2.zero;
        double currentClosestGoalDistance = double.MaxValue;

        foreach (NodeAI node in allyNodes)
        {

            if (currentClosestGoalDistance > node.closestFoodDistance)
            {
                currentClosestGoalDistance = node.closestFoodDistance;
                currentGoal = node.closestFood;
                startNode = node;
            }
        }
        //Yukar�da bulunuyor ve art�k hedefe gidebilir
        GoLocation(startNode, currentGoal);
    }



    //Belli bir konuma node �ek
    void GoLocation(NodeAI node, Vector2 target)
    {
        if(target != Vector2.zero)
        {
            Debug.Log("Target not reachable");
        }

        //Orada tam hedef noktada yeni bir node oluturulana kadar d devam etsin

        generateNextNode(node, target);
    }

    //S�radaki node'u bul
    void generateNextNode(NodeAI startNode, Vector2 targetLoc)
    {
        //Kaynak yeterliyse basacak
        if(Resource.stone < buildCost)
        {
            //continue to collecting
            Debug.Log("Not enough resources to create node");
            return;
        }


        // Hedef konum ile ba�lang�� konumu aras�ndaki fark� hesapla
        Vector2 difference = targetLoc - (Vector2)startNode.transform.position;

        // Bu fark�n b�y�kl���n� hesapla
        float distance = difference.magnitude;

        if(distance <= 5f)
        {
            //Next node target loca instantiate ve ��k
            Debug.Log("final location reached");
            //Instantiate Object at targetLoc
            CreateNode(targetLoc, startNode);

        }
        else
        {


            // Fark vekt�r�n� normalle�tirerek birim vekt�r elde et
            Vector2 direction = difference.normalized;

            // Belirli bir mesafeye (�rne�in, 5 birim) �arp ve yeni noktay� hesapla
            Vector2 nextNode = (Vector2)startNode.transform.position + direction * lineLength;

            //Instantiate Object at nextNode
            NodeAI newNode=CreateNode(nextNode,startNode);

            //Next node target loc y�n�nde ama 5 birim uzakl���ndaki konuma instantiate
            generateNextNode(newNode, targetLoc);
        }

    }
    
    void AttackNode()
    {
        int randomIndex = Random.Range(0, attackTargetNodes.Count);
        //ConnectEnemyToAttack(attackTargetNodes[randomIndex].GetComponent<Node>(), attackTargetNodesAlly[randomIndex].GetComponent<Node>());
        
    }
    void DefenceNode()
    {
        int randomIndex = Random.Range(0, defendTargetNodes.Count);
        defendTargetNodes[randomIndex].GetComponent<Node>();

    }
    
    public void ConnectEnemyToAttack(NodeAI attackerNode, Node attackedNode){
        foreach (NodeAI item in allyNodes)
        {   

            if (item.GetComponent<Node>().Equals(attackedNode))
            {
                return;
            }
        }
        
        Node nodeAttackerNode=attackerNode.GetComponent<Node>();

        NodeLine newNodeLine=Instantiate(nodeLinePrefab);
                    newNodeLine.InitializeNodeLine();
                    newNodeLine.SetColor(Color.red);
                    newNodeLine.AppendNodeToLine(attackedNode.transform.position);
                    newNodeLine.AppendNodeToLine(nodeAttackerNode.transform.position);
    
                        nodeAttackerNode.SetEnemyNode(attackedNode);
                        nodeAttackerNode.SetIsConnectedToEnemy(true);
                        nodeAttackerNode.SetEnemyNodeLine(newNodeLine);

                        attackedNode.SetEnemyNode(nodeAttackerNode);
                        attackedNode.SetIsConnectedToEnemy(true);
                        attackedNode.SetEnemyNodeLine(newNodeLine);

    }

    public NodeAI CreateNode(Vector3 position, NodeAI backNode){

            NodeAI newNode= Instantiate(nodePrefab);
            Node newNodeNode=newNode.GetComponent<Node>();
            newNode.transform.position=position;
            NodeLine newNodeLine=Instantiate(nodeLinePrefab);
            allyNodes.Add(newNode);
    
            newNodeLine.InitializeNodeLine();
            newNodeLine.AppendNodeToLine(newNode.transform.position);
            newNodeLine.AppendNodeToLine(backNode.transform.position);
            
            newNodeNode.SetBackNode(backNode.GetComponent<Node>());
            newNodeNode.SetBackNodeLine(newNodeLine);
            newNodeNode.SetBuilded(true);
            backNode.GetComponent<Node>().AddNextNode(newNodeNode);

            return newNode;
    }

}
