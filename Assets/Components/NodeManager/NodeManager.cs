using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public Node nodePrefab;
    public NodeLine nodeLinePrefab;
    public float distanceFromNode;
    public LayerMask nodeMask;
    public LayerMask enemyNodeMask;

    public Transform nodesParent;
    public Transform nodeLinesParent;

    Vector3 worldPosition;
    private Node instantiatedNode;
    private Node parentNode;
    private NodeLine newNodeLine;
    private int mode=0;
    public int totalMode=3;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            mode++;
            mode%=totalMode;
        }
        if (mode==0)
        {
         
            if (Input.GetMouseButtonDown(0))
            {
                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z =   0;
                Collider2D nodes=GetClosestCollider(worldPosition);
                if (nodes != null)
                {
                    parentNode=nodes.GetComponent<Node>();
                    instantiatedNode= Instantiate(nodePrefab,nodesParent);

                    instantiatedNode.SetBackNode(parentNode);

                    parentNode.AddNextNode(instantiatedNode);
            
                    newNodeLine=SetLineBetweenParentNode(instantiatedNode); 
                }
            }
        if (Input.GetMouseButton(0)){
            if (parentNode!=null)
            {
                if (Input.GetMouseButtonDown(1) && newNodeLine != null && instantiatedNode != null)
                {
                Destroy(newNodeLine.gameObject);
                Destroy(instantiatedNode.gameObject);
                } else if (newNodeLine != null && instantiatedNode != null){
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
                instantiatedNode.SetTextPosition();
                if (newNodeLine!=null)
                {   
                newNodeLine.UpdateLastPosition(instantiatedNode.transform.position);
                }
                }
            }
        }
        if (Input.GetMouseButtonUp(0)){
            if (instantiatedNode!=null)
            {
            instantiatedNode.SetBuilded(true);
            }
           parentNode=null;
           instantiatedNode=null;
           newNodeLine=null;
        }
        }else if (mode==1){
            if (Input.GetMouseButtonDown(0))
            {
               RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero,Mathf.Infinity,nodeMask);
                if(hit.collider != null)
                {
                    instantiatedNode=hit.collider.GetComponent<Node>();
                    newNodeLine=Instantiate(nodeLinePrefab,nodeLinesParent);
                    newNodeLine.InitializeNodeLine();
                    newNodeLine.SetColor(Color.red);
                    newNodeLine.AppendNodeToLine(instantiatedNode.transform.position);
                    newNodeLine.AppendNodeToLine(instantiatedNode.transform.position);
                }
                
            }
            if (Input.GetMouseButton(0))
            {
                if (instantiatedNode!=null && newNodeLine!=null)
                {
                        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        worldPosition.z=0;
                        newNodeLine.UpdateLastPosition(worldPosition);



                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero,Mathf.Infinity,enemyNodeMask);
                if(hit.collider != null)
                {
                    Node enemyNode=hit.collider.GetComponent<Node>();
                    newNodeLine.UpdateLastPosition(enemyNode.transform.position);


                    // set enemy next node

                }else if(newNodeLine!=null){
                    Destroy(newNodeLine.gameObject);
                }
                newNodeLine=null;    
                instantiatedNode=null;      
            }
            
                
            


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

    private NodeLine SetLineBetweenParentNode(Node node){
        
        NodeLine newNodeLine=Instantiate(nodeLinePrefab,nodeLinesParent);

        newNodeLine.InitializeNodeLine();
        newNodeLine.AppendNodeToLine(node.transform.position);
        newNodeLine.AppendNodeToLine(node.GetBackNode().transform.position);
        node.SetBackNodeLine(newNodeLine);
        return newNodeLine;
    }






}
