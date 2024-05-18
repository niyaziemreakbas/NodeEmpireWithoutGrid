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

    public float closestStoneDistance;
    public float closestFoodDistance;
    public float closestWaterDistance;

    public Node backNode;
    private List<Node> nextNodes;

    private int soldierCount = 0;
    private NodeResources nodeResources;

    Vector2 centerPoint;

    private bool builded;

    // Belirli bir merkez nokta ve yar��ap ile dairenin i�indeki en yak�n tag'ine sahip nesnenin pozisyonunu d�ner
    public Vector2 FindNearestTargetInCircle(float radius, string target)
    {
        centerPoint = transform.position;
        Vector2 closestPoint = Vector2.zero;
        int sourceLayerMask = LayerMask.GetMask("Source");

        /*        
                // Daire i�inde tag'ine sahip olan Collider2D'leri al
                Collider2D[] hits = Physics2D.OverlapCircleAll(centerPoint, radius);
                float nearestDistance = Mathf.Infinity;

                */
        RaycastHit2D hit = Physics2D.CircleCast(centerPoint, radius, Vector2.zero, Mathf.Infinity, sourceLayerMask);
        if (hit.collider != null && hit.collider.CompareTag(target))
        {
            Debug.Log("Found a hit");
            closestPoint = Physics2D.ClosestPoint(centerPoint, hit.collider);
        }

        /*
        // Bulunan Collider2D'ler aras�nda tag'ine sahip olanlar� kontrol et
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag(target))
            {
                // Nesnenin pozisyonunu al
                Vector2 targetPosition = hit.transform.position;

                // Bu nesnenin merkez noktaya olan uzakl���n� hesapla
                float distance = Vector2.Distance(centerPoint, targetPosition);

                // E�er bu nesne, �u ana kadar bulunan en yak�n nesne ise, onu kaydet
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = hit.gameObject;
                }
            }
        }
        */


        
        // E�er daire i�inde hi� tag'ine sahip nesne yoksa, Vector2.zero d�ner
        return closestPoint;
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

    //public Vector2 currentNodePoint; // Belirli bir nokta
    public float searchRadius = 5.0f; // Arama yar��ap�

    private void Start()
    {
		nextNodes=new List<Node>();
        SetTextPosition();


        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "food" nesnesinin pozisyonunu al
        closestFood = FindNearestTargetInCircle(searchRadius, "Food");

        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "food" nesnesinin pozisyonunu al
        closestStone = FindNearestTargetInCircle(searchRadius, "Stone");

        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "food" nesnesinin pozisyonunu al
        closestWater = FindNearestTargetInCircle(searchRadius, "Water");

        closestWaterDistance = (closestWater != Vector2.zero) ? Vector2.Distance(centerPoint, closestWater) : Mathf.Infinity;
        closestStoneDistance = (closestStone != Vector2.zero) ? Vector2.Distance(centerPoint, closestStone) : Mathf.Infinity;
        closestFoodDistance = (closestFood != Vector2.zero) ? Vector2.Distance(centerPoint, closestFood) : Mathf.Infinity;

        Debug.Log("Nearest food position in circle: " + closestFood + ", Distance: " + closestFoodDistance);
        Debug.Log("Nearest stone position in circle: " + closestStone + ", Distance: " + closestStoneDistance);
        Debug.Log("Nearest water position in circle: " + closestWater + ", Distance: " + closestWaterDistance);

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
