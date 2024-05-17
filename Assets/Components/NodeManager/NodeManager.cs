using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public Node nodePrefab;
    public NodeLine nodeLinePrefab;
    public float distanceFromNode;
    public LayerMask nodeMask;


    private Node instantiaedNode;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            instantiaedNode= Instantiate(nodePrefab);
        }
        if (Input.GetMouseButton(0)){
             Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z=0;

            nodePrefab=instantiaedNode.transform.position;
        }
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
