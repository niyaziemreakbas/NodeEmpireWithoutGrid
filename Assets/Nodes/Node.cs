using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

 

    bool foodSource;

    bool waterSource;

        Vector2 nodeVector2;


    private Node backNode;
    public NodeLine currentNodeLine;
    public int nodeLineIndex;


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

    public bool CheckLastOnLine(){
        return currentNodeLine.IsLastNodeOnLine(nodeLineIndex);
    }
    public NodeLine GetNodeLine(){
        return currentNodeLine;
    }
    public int GetNodeLineIndex(){
        return nodeLineIndex;
    }
    public void SetNodeLine(NodeLine nodeLine){
        currentNodeLine=nodeLine;

    }
    public void SetNodeLineIndex(int nodeIndex){
        nodeLineIndex=nodeIndex;
    }
    
    public Node GetBackNode(){
        return backNode;
    }
    
    public void SetBackNode(Node newBackNode){
        backNode=newBackNode;
    }
    
}
