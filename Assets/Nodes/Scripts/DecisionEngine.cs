using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionEngine : MonoBehaviour
{
    List<NodeAI> allyNodes;
    List<NodeAI> targetNodes;

    int buildCost = 15;

    bool attack = false;

    public int nodeCountUntilAttack;

    public GameObject Node;

    public NodeAI nodePrefab;

    public NodeAI tempNode;
    
    public NodeLine nodeLinePrefab;

    //Kaynaklar� �ekece�imiz sat�r
    public NodeResources nodeResources; 

    enum State
    {
        SearchingStone,
        SearchingWater,
        SearchingFood,

        MovingToLocation,
        Defending,
        Attacking
    }

    Vector3 tempTarget;


    State currentState;
    private void Start() {
        /*
        allyNodes=new List<NodeAI>();
        allyNodes.Add(GetComponent<NodeAI>());
        GoLocation(new Vector3(20,20,0));
        GoLocation(new Vector3(5,20,0));
        */
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
                CheckNodesForWater();
                break;
            case State.MovingToLocation:
                //GoLocation(tempTarget);
                break;
            case State.Defending:
                CheckNodesForEnemies();
                break;
            case State.Attacking:
                CheckNodesForTarget();
                break;
        }
    }

    //Nodelar aras�nda ta�a en yak�n konumu bul
    void CheckNodesForStone()
    {
        Vector2 currentGoal;
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

    }

    //Nodelar aras�nda suya en yak�n konumu bul
    void CheckNodesForWater()
    {
        Vector2 currentGoal;
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
        if(nodeResources.stone < buildCost)
        { 
            //continue to 
        }


        // Hedef konum ile ba�lang�� konumu aras�ndaki fark� hesapla
        Vector2 difference = targetLoc - (Vector2)startNode.transform.position;

        // Bu fark�n b�y�kl���n� hesapla
        float distance = difference.magnitude;

        if(distance <= 5f)
        {
            //Next node target loca instantiate ve ��k
            Debug.Log("okey");
            //Instantiate Object at targetLoc
           

        }
        else
        {


            // Fark vekt�r�n� normalle�tirerek birim vekt�r elde et
            Vector2 direction = difference.normalized;

            // Belirli bir mesafeye (�rne�in, 5 birim) �arp ve yeni noktay� hesapla
            Vector2 nextNode = (Vector2)startNode.transform.position + direction * 5f;

            //Instantiate Object at nextNode
            
            NodeAI newNode= Instantiate(nodePrefab);
            newNode.transform.position=nextNode;
            NodeLine newNodeLine=Instantiate(nodeLinePrefab);
            allyNodes.Add(newNode);
            newNodeLine.InitializeNodeLine();
            newNodeLine.AppendNodeToLine(newNode.transform.position);
            newNodeLine.AppendNodeToLine(startNode.transform.position);
            
            newNode.GetComponent<Node>().SetBackNode(startNode.GetComponent<Node>());
            newNode.GetComponent<Node>().SetBackNodeLine(newNodeLine);

            //Next node target loc y�n�nde ama 5 birim uzakl���ndaki konuma instantiate
            generateNextNode(newNode, targetLoc);
        }

    }

    void createNewNode(Vector2 location)
    {
        //convert v2 to v3
        Vector3 vector3 = new Vector3(location.x, location.y);
        Instantiate(Node, vector3, Quaternion.identity);
    }

    //D��man yak�nsa savun
    void CheckNodesForEnemies()
    {

    }

    //Kaynaklar�n yeterince iyiyse sald�r
    void CheckNodesForTarget()
    {

    }

    void attackControl()
    {
        if(allyNodes.Count < nodeCountUntilAttack)
        {
            attack = true;
        }

    }
    
    /*
    public NodeAI GetClosestNode(Vector2 target){
        NodeAI currentNode = null;
        float currentClosestGoalDistance = float.MaxValue;
        foreach (NodeAI node in allyNodes)
        {
            Vector2 diff=target - (Vector2)node.transform.position;
            float distance = diff.magnitude;
            if (currentClosestGoalDistance >= distance)
            {
                currentClosestGoalDistance = node.closestFoodDistance;
                currentNode = node;
            }
        }
        return currentNode;
    }
    */
}
