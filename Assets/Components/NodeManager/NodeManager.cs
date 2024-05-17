using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public Node nodePrefab;
    public NodeLine nodeLinePrefab;
    public float distanceFromNode;
    public LayerMask nodeMask;

    Vector3 worldPosition;
    private Node instantiatedNode;
    private Node parentNode;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z =   0;
            Collider2D nodes=GetClosestCollider(worldPosition);
            if (nodes != null)
            {
            parentNode=nodes.GetComponent<Node>();
            instantiatedNode= Instantiate(nodePrefab);

            instantiatedNode.SetBackNode(parentNode);
            parentNode.AddNextNode(instantiatedNode);
            Debug.Log(parentNode.gameObject.name);
            }
        }
        if (Input.GetMouseButton(0)){
            if (parentNode!=null)
            {
                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z=0;
                if (Vector3.Distance(worldPosition,parentNode.transform.position)<=distanceFromNode)
                {
                    instantiatedNode.transform.position=worldPosition;
                }else{
                    Vector3 newPos=worldPosition-parentNode.transform.position;
                    newPos.Normalize();
                    instantiatedNode.transform.position=newPos*distanceFromNode+parentNode.transform.position;
                }
            }
        }
        if (Input.GetMouseButtonUp(0)){
          
            SetLineBetweenParentNode(instantiatedNode); 
           parentNode=null;
           instantiatedNode=null;
        }
    }
    private Collider2D GetClosestCollider(Vector3 pos){
        Collider2D closestCollider=null;
        Collider2D[] nodes=Physics2D.OverlapCircleAll(worldPosition,distanceFromNode,nodeMask);
        float distance=9999;
        foreach (Collider2D item in nodes)
        {
            float curDistance=Vector3.Distance(item.transform.position,pos);
            if (curDistance<=distance)
            {
                closestCollider=item;
                distance=curDistance;
            }
        }
     return closestCollider;
    }

    private void SetLineBetweenParentNode(Node node){
        
            NodeLine newNodeLine=Instantiate(nodeLinePrefab);
        Node curNode = node;
        List<Vector3> positions=new List<Vector3>();
        while (curNode!=null)
        {
            positions.Add(curNode.transform.position);
            curNode=curNode.GetBackNode();
        }
        newNodeLine.InitializeNodeLine(positions);
    }






}
