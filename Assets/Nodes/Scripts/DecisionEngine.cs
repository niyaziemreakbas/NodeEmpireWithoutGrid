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

    float delayAmount = 5f;

    int buildCost = 100;

    bool attack = false;

    int nodeCountUntilAttack = 5;    

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
        GetComponent<Node>().soldierCount = 20;
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
            case State.Idle:
            default:
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
        else if (minValue == water && (water <= stone && water <= food))
        {
            currentState = State.SearchingWater;
        }
        else if (minValue == food && (food <= stone && food <= water))
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

    private void Update()
    {
        //Bot UI update
        if (botUI.activeInHierarchy) { updateText(); }

        // Bot UI aç kapa
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

        if(allyNodes.Count > 1 && GetComponent<Node>().soldierCount > 20)
        {
            DefenceNode();
        }
        // Under Attack if()

        if (nodeCountUntilAttack < allyNodes.Count )
        {
            currentState = State.Attack;
            updateState();
        }

        if (currentState == State.Idle)
        {
            //Invoke("chooseResourceGoal",delayAmount);
            //Invoke("updateState",delayAmount);
            chooseResourceGoal();
            updateState();
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
                startNode = node;
                currentClosestGoalDistance = node.closestStoneDistance;
                currentGoal = node.closestStone;
            }
        }
        //Debug.Log("current goal : " + currentGoal);
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
                startNode = node;

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
        if(target == Vector2.zero)
        {
            Debug.Log("Target not reachable");
        }

        //Orada tam hedef noktada yeni bir node oluturulana kadar d devam etsin

        generateNextNode(node, target);
    }

    IEnumerator makeDelay()
    {
        yield return new WaitForSeconds(delayAmount);
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

            Debug.Log("Reached Location");
            currentState = State.Idle;
            updateState();
            //Debug.Log("Bekleme başladı ");
            ////makeDelay();
            //Debug.Log("Bekleme bitti ");

            //Invoke("updateState", delayAmount);

        }
        else
        {
            Debug.Log("distance : " + distance);

            CreateNode(newPoint, startNode);

            Invoke("updateState", delayAmount);


        }

    }

    /*
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
    */
    void AttackNode()
    {
        NodeAI attacker;
        Node attacked;

        foreach (NodeAI node in allyNodes)
        {

            if(node.attackTargets.Count != 0)
            {
                int randomIndex = Random.Range(0, node.attackTargets.Count);
                
                attacked = node.attackTargets[randomIndex];
                attacker = node;

                if(attacker.GetComponent<Node>().soldierCount > attacked.soldierCount)
                {
                    ConnectEnemyToAttack(attacker, attacked);
                    attackTargetNodes.Remove(attacked.GetComponent<NodeAI>());
                    Debug.Log("DEleted");
                }

            }

        }
        
    }
    
    void DefenceNode()
    {
        //&& allyNodes[randomIndex].GetComponent<Node>().backNode != null
        int randomIndex = Random.Range(0, allyNodes.Count);
        Debug.Log("seçilenNodes : " + randomIndex);
        if (allyNodes[randomIndex].GetComponent<Node>().backNode.soldierCount != 0 && randomIndex != 0) 
        {
            int randomSoldierCount = Random.Range(0, 5);
            Debug.Log("Asker Gönderiliyor...");
            Invoke("DeleteLine", randomSoldierCount);
            ConnectToNodeExportSoldier(allyNodes[randomIndex].GetComponent<Node>().backNode.GetComponent<NodeAI>(), allyNodes[randomIndex].GetComponent<NodeAI>());
        }

        //defendTargetNodes[randomIndex].GetComponent<Node>();

    }

    void DeleteLine()
    {
        Debug.Log("Silindi");
    }
    
    public void ConnectEnemyToAttack(NodeAI attackerNode, Node attackedNode){
        //kendi dostu ise fonksiyonu sonlandır
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
    
    public void ConnectToNodeExportSoldier(NodeAI exportNode, NodeAI importNode){
        Node nodeExportNode=exportNode.GetComponent<Node>();
        Node nodeImportNode=importNode.GetComponent<Node>();

        NodeLine newNodeLine=Instantiate(nodeLinePrefab);
        newNodeLine.SetColor(Color.green);
        newNodeLine.AppendNodeToLine(nodeExportNode.transform.position);
        newNodeLine.AppendNodeToLine(nodeImportNode.transform.position);

        nodeExportNode.SetTransferNode(nodeImportNode);
        nodeExportNode.SetIsExporting(true);
        nodeExportNode.SetExportLine(newNodeLine);

                        nodeImportNode.SetTransferNode(nodeExportNode);
                        nodeImportNode.SetIsImporting(true);




    }

}
