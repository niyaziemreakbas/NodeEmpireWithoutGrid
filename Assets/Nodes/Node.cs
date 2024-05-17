using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

 

    bool foodSource;

    bool waterSource;

    Vector2 nodeVector2;

    public Vector2 closestStone;
    public Vector2 closestFood;
    public Vector2 closestWater;

    public double closestStoneDistance;
    public double closestFoodDistance;
    public double closestWaterDistance;

    private Node backNode;
    private List<Node> nextNodes;
    private void Start()
    {
        nodeVector2 = transform.position;
       // RaycastHit2D hit = Physics2D.Raycast(nodeVector2, Vector2.zero, Mathf.Infinity, layerMask);
        
        
    }



    void Update()
    {
        // Ray ray = new Ray(raycastOrigin.position, Vector3.down);
        // RaycastHit hit;

        
        // if (Physics.Raycast(ray, out hit, raycastDistance))
        // {
        //     // �arp��ma tespit edildi
        //     if (hit.collider.CompareTag("Water"))
        //     {
        //         Debug.Log("Su bulundu");
        //         waterSource = true;
        //     }
        //     // �arp��ma tespit edildi
        //     else if (hit.collider.CompareTag("Food"))
        //     {
        //         Debug.Log("Yemek bulundu");
        //         foodSource = true;
        //     }
        // }


        
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
}
