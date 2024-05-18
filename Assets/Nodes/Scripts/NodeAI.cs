using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.ObjectChangeEventStream;
using static UnityEngine.GraphicsBuffer;

public class NodeAI : MonoBehaviour
{
    public Vector2 closestStone;
    public Vector2 closestFood;
    public Vector2 closestWater;

    public float closestStoneDistance;
    public float closestFoodDistance;
    public float closestWaterDistance;

    public NodeLine nodeLinePrefab;




    // List<Vector2> attackTargets;
    // List<Vector2> attackTargetsAlly;


    // List<Vector2> defendTargets;

    Vector2 centerPoint;

    float sourceSearchRadius = 10.0f; // Arama yar��ap�
    float targetSearchRadius = 5.0f; // Arama yar��ap�


    /// Belirli bir merkez nokta ve yar��ap ile dairenin i�indeki en yak�n tag'ine sahip nesnenin pozisyonunu d�ner
    public void FindNearestTargetInCircle(float radius, string target, string layer)
    {


        float closestDistance = Mathf.Infinity;
        /*
        RaycastHit2D[] hits = Physics2D.CircleCastAll(centerPoint, radius, Vector2.zero, Mathf.Infinity, sourceLayerMask);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag(target))
            {
                Vector2 point = Physics2D.ClosestPoint(centerPoint, hit.collider);
                float distance = Vector2.Distance(centerPoint, point);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;
                }
            }
        }

        if (target == "Food")
        {
            closestFood = closestPoint;
        }
        else if (target == "Stone")
        {
            closestStone = closestPoint;
        }
        else if (target == "Water")
        {
            closestWater = closestPoint;
        }
        */
        centerPoint = transform.position;
        Vector2 closestPoint = Vector2.zero;
        int sourceLayerMask = LayerMask.GetMask(layer);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(centerPoint, radius, Vector2.zero, Mathf.Infinity, sourceLayerMask);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.point != null && hit.collider.CompareTag(target))
            {
                Debug.Log("Found a hit" + target);
                Debug.Log("hit point  : " + hit.point);

                closestPoint = hit.point;
            }
            if (target == "Food")
            {
                closestFood = closestPoint;
            }
            else if (target == "Stone")
            {
                Debug.Log("nodeAı içinde  : " + closestStone);
                Debug.Log("nodeAı içinde closest  : " + closestPoint);

                closestStone = closestPoint;
            }
            else if (target == "Water")
            {
                closestWater = closestPoint;
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

        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "taş" nesnesinin pozisyonunu al
        FindNearestTargetInCircle(sourceSearchRadius, "Stone", "Source");

        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "food" nesnesinin pozisyonunu al
        FindNearestTargetInCircle(sourceSearchRadius, "Food", "Source");


        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "su" nesnesinin pozisyonunu al
        FindNearestTargetInCircle(sourceSearchRadius, "Water", "Source");


        closestWaterDistance = (closestWater != Vector2.zero) ? Vector2.Distance(centerPoint, closestWater) : Mathf.Infinity;
        closestStoneDistance = (closestStone != Vector2.zero) ? Vector2.Distance(centerPoint, closestStone) : Mathf.Infinity;
        closestFoodDistance = (closestFood != Vector2.zero) ? Vector2.Distance(centerPoint, closestFood) : Mathf.Infinity;

        Debug.Log("Nearest food position in circle: " + closestFood + ", Distance: " + closestFoodDistance);
        Debug.Log("Nearest stone position in circle: " + closestStone + ", Distance: " + closestStoneDistance);
        Debug.Log("Nearest water position in circle: " + closestWater + ", Distance: " + closestWaterDistance);
    }

    //Add new Targets
    private void OnTriggerEnter(Collider other)
    {
        /*
        // Giriş yapan objenin etiketini kontrol edebilirsiniz
        if (other.CompareTag("EnemyNode"))
        {
            //Kendi node u bulmak 
            

        }
        */
        if (other.CompareTag("Node"))
        {
            if(this.gameObject.GetComponent<Node>().soldierCount <= other.gameObject.GetComponent<Node>().soldierCount)
            {
                //savun
                Debug.Log("Savunacak adam Location : ");


                //defendTargets.Add(this.GetComponent<Vector2>());
                //defendTargetsAlly.Add(this.GetComponent<Vector2>());
            }
            else
            {
                //saldır
                Debug.Log("Saldırılacak adam Location : ");
                ConnectEnemyToAttack(this.GetComponent<NodeAI>(), other.GetComponent<Node>());

                //attackTargets.Add(other.GetComponent<Vector2>());

                //attackTargetsAlly.Add(this.GetComponent<Vector2>());

            }
        }
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
