using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public Resource mainNodeResource;

    public Node nodePrefab;
    public NodeLine nodeLinePrefab;
    public float maxDistanceFromNode;
    public float minDistanceFromNode;
    public LayerMask nodeMask;

    public Transform nodesParent;
    public Transform nodeLinesParent;

    Vector3 worldPosition;
    private Node instantiatedNode;
    private Node parentNode;
    private NodeLine newNodeLine;
    private int mode=0;
    public int totalMode=3;

    private int nodeBuildingCost = 200;
    
    public bool modChangeAllow;
private void Start() {
    modChangeAllow=false;
    UIHelper.Instance.ShowUIPrompt(0);
}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !modChangeAllow)
        {
            Debug.Log("mod değişti");
            mode++;
            mode%=totalMode;
            UIHelper.Instance.ShowModeUpdate(mode);
        }

        if (mode==0)
        {
            if (Input.GetMouseButtonDown(0) && !modChangeAllow)
            {
                
                

                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z =   0;
                Collider2D nodes=GetClosestCollider(worldPosition);
                if (nodes != null)
                {
                    if (mainNodeResource.GetStone() >= nodeBuildingCost)
                    {
                      
                    modChangeAllow =true;
                    UIHelper.Instance.ShowUIPrompt(1);
                    parentNode=nodes.GetComponent<Node>();

                    instantiatedNode= Instantiate(nodePrefab,nodesParent);

                    instantiatedNode.SetBackNode(parentNode);

                    parentNode.AddNextNode(instantiatedNode);
            
                    newNodeLine=SetLineBetweenParentNode(instantiatedNode);

                    }else{
                        UIHelper.Instance.ShowWarningText("You need 200 stones to build it",Color.red);
                    }
                }
        }
        else if (Input.GetMouseButtonDown(0) && modChangeAllow){
            UIHelper.Instance.ShowUIPrompt(0);
            modChangeAllow=false;
            
            if (instantiatedNode!=null)  
            {
                 mainNodeResource.SpendStone(nodeBuildingCost);
                 instantiatedNode.SetBuilded(true);
                 AudioManager.instance.NodeConstructedSF();
            }
           parentNode=null;
           instantiatedNode=null;
           newNodeLine=null;

        } else if (modChangeAllow){
            if (parentNode!=null)
            {
                if (Input.GetMouseButtonDown(1) && newNodeLine != null && instantiatedNode != null)
                {
                    UIHelper.Instance.ShowUIPrompt(0);
                    modChangeAllow=false;
                    Destroy(newNodeLine.gameObject);
                    Destroy(instantiatedNode.gameObject);
                } else 
                if (newNodeLine != null && instantiatedNode != null){
                
                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z=0;
                Vector2 diff=worldPosition-parentNode.transform.position;
                float distance=diff.magnitude;
                if (distance<=maxDistanceFromNode && distance>=minDistanceFromNode)
                {
                    instantiatedNode.transform.position=worldPosition;
                }
                else if (distance>maxDistanceFromNode)
                {
                    Vector3 newPos=worldPosition-parentNode.transform.position;
                    newPos.Normalize();
                    instantiatedNode.transform.position=newPos*maxDistanceFromNode+parentNode.transform.position;
                }else if(distance<minDistanceFromNode){
                    Vector3 newPos=worldPosition-parentNode.transform.position;
                    newPos.Normalize();
                    instantiatedNode.transform.position=newPos*minDistanceFromNode+parentNode.transform.position;
                }
                instantiatedNode.SetTextPosition();
                if (newNodeLine!=null)
                {   
                newNodeLine.UpdateLastPosition(instantiatedNode.transform.position);
                }
                }
            }
        }

        }
        else if (mode==1){

            if (Input.GetMouseButtonDown(0) && !modChangeAllow)
            {
               RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero,Mathf.Infinity,nodeMask);
                if(hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("PlayerNode"))
                    {
                    modChangeAllow=true;
                    UIHelper.Instance.ShowUIPrompt(1);
                    instantiatedNode=hit.collider.GetComponent<Node>();
                    if (instantiatedNode.GetExportNodeLine()!=null)
                    {
                        newNodeLine=instantiatedNode.GetExportNodeLine();
                    }else
                    {
                    newNodeLine=Instantiate(nodeLinePrefab,nodeLinesParent);
                    }
                    newNodeLine.InitializeNodeLine();
                    newNodeLine.SetColor(Color.yellow);
                    newNodeLine.AppendNodeToLine(instantiatedNode.transform.position);
                    newNodeLine.AppendNodeToLine(instantiatedNode.transform.position);
                    }
                }
                
            } else if(Input.GetMouseButtonDown(0)&&modChangeAllow){
             
                UIHelper.Instance.ShowUIPrompt(0);
                modChangeAllow=false;
                if (instantiatedNode!=null && newNodeLine!=null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero,Mathf.Infinity,nodeMask);
                    if(hit.collider != null)
                    {
                        
                        Node hittedNode=hit.collider.GetComponent<Node>();
                        newNodeLine.UpdateLastPosition(hittedNode.transform.position);

                        Vector2 diff=hittedNode.transform.position-instantiatedNode.transform.position;
                        float distance=diff.magnitude;
                        if (distance<=5.5f)
                        {
                          
                            if (hittedNode.gameObject.CompareTag("EnemyNode"))
                            {

                                newNodeLine.SetColor(Color.red);

                                instantiatedNode.SetEnemyNode(hittedNode);
                                instantiatedNode.SetIsConnectedToEnemy(true);
                                instantiatedNode.SetEnemyNodeLine(newNodeLine);
                            
                                hittedNode.SetEnemyNode(instantiatedNode);
                                hittedNode.SetIsConnectedToEnemy(true);
                                hittedNode.SetEnemyNodeLine(newNodeLine);
                                
                            }else if (hittedNode.gameObject.CompareTag("PlayerNode")){

                                newNodeLine.SetColor(Color.green);
                                instantiatedNode.SetTransferNode(hittedNode);
                                instantiatedNode.SetIsExporting(true);
                                instantiatedNode.SetExportLine(newNodeLine);

                                hittedNode.SetTransferNode(instantiatedNode);
                                hittedNode.SetIsImporting(true);

                            }
                  
                        }else{
                            // MESAFE ÇOK UZUN
                            UIHelper.Instance.ShowWarningText("The connection you're trying to build is too long",Color.yellow);
                            Destroy(newNodeLine.gameObject);
                        }
                    // set enemy next node

                }else if(newNodeLine!=null){
                    Destroy(newNodeLine.gameObject);
                }
                newNodeLine=null;    
                instantiatedNode=null; 
                }     
            } else if (modChangeAllow)
            {

                if (Input.GetMouseButtonDown(1) && newNodeLine != null && instantiatedNode != null)
                {
                    
                UIHelper.Instance.ShowUIPrompt(0);
                    modChangeAllow=false;
                    instantiatedNode.ResetExport();

                Destroy(newNodeLine.gameObject);
                }
                if (instantiatedNode!=null && newNodeLine!=null)
                {
                        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        worldPosition.z=0;
                        newNodeLine.UpdateLastPosition(worldPosition);
                }
            }
            
                
            


        }
    }
    private Collider2D GetClosestCollider(Vector3 pos){
        Collider2D closestCollider=null;
        Collider2D[] nodes=Physics2D.OverlapCircleAll(worldPosition,maxDistanceFromNode,nodeMask);
        float distance=9999;
        foreach (Collider2D item in nodes)
        {
            float curDistance=Vector3.Distance(item.transform.position,pos);
            if (curDistance<=distance && item.gameObject.CompareTag("PlayerNode"))
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
