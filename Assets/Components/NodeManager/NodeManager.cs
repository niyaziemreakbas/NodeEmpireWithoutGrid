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

    private void SetLineBetweenParentNode(){
                

    }






    private void BuildNode(){
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z=0;
        Collider2D nodes=Physics2D.OverlapCircle(worldPosition,distanceFromNode,nodeMask);

        if (nodes!=null)
        {

            Node parentNode=nodes.GetComponent<Node>();
            Debug.Log(parentNode.gameObject.name);
            NodeLine parentNodeLine=parentNode.GetNodeLine();

            Node newNode=Instantiate(nodePrefab);
            newNode.transform.position=worldPosition;


            // main node degilse diye bi kontrol gelecek
            if (parentNode.CompareTag("MainNode"))
            {
                NodeLine newNodeLine=Instantiate(nodeLinePrefab);
                newNode.SetNodeLine(newNodeLine);
                newNode.SetNodeLineIndex(parentNode.GetNodeLineIndex()+1);
                List<Node> newNodes=new List<Node>();
                newNodes.Add(parentNode);
                newNodeLine.InitializeNodeLine(newNodes);
                newNodeLine.AppendNodeToLine(newNode);

            }else {

            // check if last vertex on line
            if (parentNode.CheckLastOnLine())
            {
                // append to node line
                newNode.SetNodeLine(parentNodeLine);
                newNode.SetNodeLineIndex(parentNode.GetNodeLineIndex()+1);

                newNode.GetNodeLine().AppendNodeToLine(newNode);

            }else{

                // create new NodeLine
                NodeLine newNodeLine=Instantiate(nodeLinePrefab);
                newNode.SetNodeLineIndex(parentNode.GetNodeLineIndex()+1);
                newNodeLine.InitializeNodeLine(parentNodeLine.GetNodesToIndex(parentNode.GetNodeLineIndex()));
                newNodeLine.AppendNodeToLine(newNode);
            }

            }
        }
    }
}
