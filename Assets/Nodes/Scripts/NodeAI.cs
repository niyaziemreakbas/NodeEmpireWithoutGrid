using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeAI : MonoBehaviour
{
    public Vector2 closestStone;
    public Vector2 closestFood;
    public Vector2 closestWater;

    public float closestStoneDistance;
    public float closestFoodDistance;
    public float closestWaterDistance;

    public NodeLine nodeLinePrefab;


    public List<Node> attackTargets = new List<Node>();

    // List<Vector2> attackTargetsAlly;


    // List<Vector2> defendTargets;

    Vector2 centerPoint;

    float sourceSearchRadius = 10.0f; // Arama yar��ap�
    float targetSearchRadius = 5.0f; // Arama yar��ap�


    /// Belirli bir merkez nokta ve yar��ap ile dairenin i�indeki en yak�n tag'ine sahip nesnenin pozisyonunu d�ner
    public void FindNearestTargetInCircle(float radius, string target, string layer)
    {
        centerPoint = transform.position;
        Vector2 closestPoint = Vector2.zero;
        int sourceLayerMask = LayerMask.GetMask(layer);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(centerPoint, radius, Vector2.zero, Mathf.Infinity, sourceLayerMask);
        Debug.Log("Hit Counts : " +  hits.Length);


        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("Hit Name : " + hit.collider.tag);
            if (hit.point != null && hit.collider.CompareTag(target))
            {

                closestPoint = hit.point;
            }
                
            if (hit.collider.CompareTag(target)){
                    
                if (target == "Food")
                {
                    closestFood = closestPoint;
                    Debug.Log("Bulundu Food");
                }
                else if (target == "Stone")
                {

                    closestStone = closestPoint;
                }
                else if (target == "Water")
                {
                    closestWater = closestPoint;
                }
                else if (target == "PlayerNode")
                {
                    Node currentNode = GetComponent<Node>();
                    while (currentNode.GetBackNode() != null)
                    {
                            currentNode = currentNode.GetBackNode();
                    }

                    currentNode.gameObject.GetComponent<DecisionEngine>().attackTargetNodes.Add(hit.collider.GetComponent<Node>());
                    Debug.Log("Düşman Node bulundu");
                    
                    attackTargets.Add(hit.collider.GetComponent<Node>());
                }
            
            }
        }

    }

    // Debug ama�l�, sahnede daireyi �izer
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sourceSearchRadius); // �rne�in 5 birimlik bir daire

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetSearchRadius); // �rne�in 5 birimlik bir daire
    }

    private void Start()
    {
        FindNearestTargetInCircle(targetSearchRadius, "PlayerNode", "Node");

        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "taş" nesnesinin pozisyonunu al
        FindNearestTargetInCircle(sourceSearchRadius, "Stone", "Source");

        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "food" nesnesinin pozisyonunu al
        FindNearestTargetInCircle(sourceSearchRadius, "Food", "Source");


        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "su" nesnesinin pozisyonunu al
        FindNearestTargetInCircle(sourceSearchRadius, "Water", "Source");



        closestWaterDistance = (closestWater != Vector2.zero) ? Vector2.Distance(centerPoint, closestWater) : Mathf.Infinity;
        closestStoneDistance = (closestStone != Vector2.zero) ? Vector2.Distance(centerPoint, closestStone) : Mathf.Infinity;
        closestFoodDistance = (closestFood != Vector2.zero) ? Vector2.Distance(centerPoint, closestFood) : Mathf.Infinity;

       // Debug.Log("Nearest food position in circle: " + closestFood + ", Distance: " + closestFoodDistance);
       // Debug.Log("Nearest stone position in circle: " + closestStone + ", Distance: " + closestStoneDistance);
       // Debug.Log("Nearest water position in circle: " + closestWater + ", Distance: " + closestWaterDistance);
    }


    public void ConnectEnemyToAttack(NodeAI attackerNode, Node attackedNode)
    {
        /*
        foreach (NodeAI item in allyNodes)
        {

            if (item.GetComponent<Node>().Equals(attackedNode))
            {
                return;
            }
        }
        */
        Node nodeAttackerNode = attackerNode.GetComponent<Node>();

        NodeLine newNodeLine = Instantiate(nodeLinePrefab);
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


}
