using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DecisionEngine : MonoBehaviour
{
    public List<NodeAI> allyNodes=new List<NodeAI>();
    List<NodeAI> attackTargetNodes;
    List<NodeAI> attackTargetNodesAlly;

    List<NodeAI> defendTargetNodes;
    //List<NodeAI> defendTargetNodesAlly;


    int buildCost = 15;

    bool attack = false;

    public int nodeCountUntilAttack = 5;

    public GameObject Node;

    public NodeAI nodePrefab;

    NodeAI tempNode;
    
    public NodeLine nodeLinePrefab;

    public float lineLength = 5;



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
    private void Update() {
        if(allyNodes.Count == 0)
        {
            tempNode = this.GetComponent<NodeAI>();
        }
        allyNodes.Add(GetComponent<NodeAI>());
        CheckNodesForStone();
        DelayedMethod();
        CheckNodesForFood();
        DelayedMethod();

        CheckNodesForWater();
        DelayedMethod();


    }

    IEnumerator DelayedMethod()
    {
        // 2 saniye bekle
        yield return new WaitForSeconds(2f);
    }

    /*
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
    */


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
                Debug.Log("if içine gşrdi");
                Debug.Log("stone mesafesi : " + node.closestStone);

                currentClosestGoalDistance = node.closestStoneDistance;
                currentGoal = node.closestStone;
            }
        }
        Debug.Log("currentGoal : " + currentGoal);
        
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

        Vector2 startNodeLoc = startNode.transform.position;

        //Kaynak yeterliyse basacak
        if (GetComponent<Resource>().stone < buildCost)
        {
            //continue to collecting
            Debug.Log("Not enough resources to create node");
            return;
        }

        // İki nokta arasındaki yön vektörünü hesapla
        Vector2 direction = targetLoc - startNodeLoc;

        float distance = (targetLoc - startNodeLoc).magnitude;

        // Yön vektörünü normalleştir (birim vektör)
        direction.Normalize();

        // Normalleştirilmiş yön vektörünü mesafe ile çarp
        Vector2 moveVector = direction * lineLength;

        // Yeni noktayı hesapla
        Vector2 newPoint = startNodeLoc + moveVector;

        if(distance <= 5f)
        {

            //Instantiate Object at targetLoc
            CreateNode(targetLoc, startNode);

        }
        else
        {

            //Instantiate Object at nextNode
            CreateNode(newPoint,startNode);


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
        Debug.Log("Create Node çağrıldı : "  +position);
        NodeAI newNode = Instantiate(nodePrefab);
        Node newNodeNode = newNode.GetComponent<Node>();
        newNode.transform.position = position;
        NodeLine newNodeLine = Instantiate(nodeLinePrefab);
        allyNodes.Add(newNode);

        newNodeLine.InitializeNodeLine();
        newNodeLine.AppendNodeToLine(newNode.transform.position);
        newNodeLine.AppendNodeToLine(backNode.transform.position);

        newNodeNode.SetBackNode(backNode.GetComponent<Node>());
        newNodeNode.SetBackNodeLine(newNodeLine);
        newNodeNode.SetBuilded(true);
        backNode.GetComponent<Node>().AddNextNode(newNodeNode);

        GetComponent<Resource>().stone -= buildCost;

        return newNode;
    }

}
