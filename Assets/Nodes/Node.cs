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

    private bool builded;

    // Belirli bir merkez nokta ve yar��ap ile dairenin i�indeki en yak�n "food" tag'ine sahip nesnenin pozisyonunu d�ner
    public Vector2 FindNearestFoodInCircle(Vector2 centerPoint, float radius)
    {
        // Daire i�inde "food" tag'ine sahip olan Collider2D'leri al
        Collider2D[] hits = Physics2D.OverlapCircleAll(centerPoint, radius);
        GameObject nearestFood = null;
        float nearestDistance = Mathf.Infinity;

        // Bulunan Collider2D'ler aras�nda "food" tag'ine sahip olanlar� kontrol et
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Food"))
            {
                // Nesnenin pozisyonunu al
                Vector2 foodPosition = hit.transform.position;

                // Bu nesnenin merkez noktaya olan uzakl���n� hesapla
                float distance = Vector2.Distance(centerPoint, foodPosition);

                // E�er bu nesne, �u ana kadar bulunan en yak�n nesne ise, onu kaydet
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestFood = hit.gameObject;
                }
            }
        }

        // En yak�n "food" tag'ine sahip nesnenin pozisyonunu d�ner
        if (nearestFood != null)
        {
            return nearestFood.transform.position;
        }

        // E�er daire i�inde hi� "food" tag'ine sahip nesne yoksa, Vector2.zero d�ner
        return Vector2.zero;
    }

    // Debug ama�l�, sahnede daireyi �izer
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5.0f); // �rne�in 5 birimlik bir daire
    }

    private void Awake()
    {
        builded=false;
        nodeResources = GetComponent<NodeResources>();
        Canvas canvas= FindObjectOfType<Canvas>();
        soldierCountText.transform.SetParent(canvas.transform,false);   
    }

    public Vector2 currentNodePoint; // Belirli bir nokta
    public float searchRadius = 5.0f; // Arama yar��ap�

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
    }

    private void Update()
    {
        currentNodePoint = transform.position;



        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "food" nesnesinin pozisyonunu al
        Vector2 nearestFoodPosition = FindNearestFoodInCircle(currentNodePoint, searchRadius);

        // En yak�n "food" nesnesinin pozisyonunu ekrana yazd�r
        Debug.Log("Nearest food position in circle: " + nearestFoodPosition);

        // En yak�n "food" nesnesinin pozisyonunu ekrana yazd�r
        Debug.Log("Current position in circle: " + currentNodePoint);

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
    private void OnDestroy() {
        if(soldierCountText != null)
        {
            Destroy(soldierCountText.gameObject);
        }
        
    }
    public void SetBuilded(bool builded){
        this.builded=builded;
    }
}
