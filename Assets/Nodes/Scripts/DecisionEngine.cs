using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
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

    public GameObject botUI;

    public TextMeshProUGUI stone;
    public TextMeshProUGUI food;
    public TextMeshProUGUI water;

    int buildCost = 100;

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
        Attack,
        Idle
    }


    State currentState;

    private void Start() {
        currentState = State.Idle;
        if(allyNodes.Count == 0)
        {
            tempNode = this.GetComponent<NodeAI>();
        }
        allyNodes.Add(GetComponent<NodeAI>());
        //Invoke("CheckNodesForStone()",5f);

        botUI.SetActive(false);

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

    void chooseResourceGoal()
    {
        int stone = Mathf.RoundToInt(GetComponent<Resource>().stone);
        int water = Mathf.RoundToInt(GetComponent<Resource>().water);
        int food = Mathf.RoundToInt(GetComponent<Resource>().food);

        // En az olan değeri bulmak için minimumu hesaplayın
        int minValue = Mathf.Min(stone, water, food);

        if (minValue == stone && (stone <= water && stone <= food))
        {
            currentState = State.SearchingStone;
        }
        if (minValue == water && (water <= stone && water <= food))
        {
            currentState = State.SearchingWater;
        }
        if (minValue == food && (food <= stone && food <= water))
        {
            currentState = State.SearchingFood;
        }

        Debug.Log("changed Resource : " + currentState);

    }

    void updateText()
    {
        if(botUI.activeInHierarchy)
        {
            stone.text = GetComponent<Resource>().stone.ToString();
            food.text = GetComponent<Resource>().food.ToString();
            water.text = GetComponent<Resource>().water.ToString();
        }
    }

    Vector2 centerPoint;

    private void Update()
    {
        if (botUI.activeInHierarchy) { updateText(); }

        // Fare tıklamasını kontrol eder
        if (Input.GetMouseButtonDown(0))
        {
            // Fare pozisyonunu ekrandan dünya koordinatlarına çevirir
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            // Eğer tıklanan obje bu obje ise
            if (hit.collider != null && hit.collider.gameObject.CompareTag("EnemyNode"))
            {
                // Aktif etmek istediğiniz objeyi aktif hale getirir
                if (botUI != null && !botUI.activeInHierarchy)
                {
                    botUI.SetActive(true);
                }
                else if (botUI != null && botUI.activeInHierarchy)
                {
                    botUI.SetActive(false);
                }
                updateText();
            }
        }



        // Under Attack if()
        
        if(nodeCountUntilAttack > 5 )
        {

        }

        if (currentState == State.Idle)
        {
            Invoke("chooseResourceGoal", 5f);
            Invoke("updateState", 5f);
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


    private IEnumerator coroutine;

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

            Debug.Log("Reached Location");
            currentState = State.Idle;
        }
        else
        {
            //Instantiate Object at nextNode
            generateNextNodeDelay(10f, CreateNode(newPoint, startNode), targetLoc);
            

        }

    }

    IEnumerator generateNextNode(float waitTime, NodeAI startNode, Vector2 targetLoc)
    {
        yield return new WaitForSeconds(waitTime);
        generateNextNode(startNode, targetLoc);
    }
    public void generateNextNodeDelay(float waitTime, NodeAI startNode, Vector2 targetLoc)
    {
        StartCoroutine(generateNextNode(waitTime, startNode, targetLoc));
    }

    IEnumerator CreateNode(float waitTime, Vector3 position, NodeAI backNode)
    {
        yield return new WaitForSeconds(waitTime);
        CreateNode(position, backNode);
    }
    
    public void StartCreateNode(float waitTime, Vector3 position, NodeAI backNode)
    {
        StartCoroutine(CreateNode(waitTime, position, backNode));
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
