using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLine : MonoBehaviour
{
    LineRenderer line;
    public List<Node> nodes;
    void Start()
    {
        line=GetComponent<LineRenderer>();
        SetLinesByNodes();
    }
    public void InitializeNodeLine(List<Node> newNodes){
        line=GetComponent<LineRenderer>();
        nodes=newNodes;
        SetLinesByNodes();
    }
    public void AppendNodeToLine(Node node){
        
        nodes.Add(node);
        SetLinesByNodes();
    }

    public bool IsLastNodeOnLine(int nodeIndex){
        return nodes.Count-1==nodeIndex;
    }
    
    public List<Node> GetNodesToIndex(int index){
        List<Node> newNodes=new List<Node>();
        for (int i = 0; i < index+1; i++)
        {
            newNodes.Add(nodes[i]);
        }
        return newNodes;
    }

    public void SetLinesByNodes(){
        line.positionCount=nodes.Count;
        Vector3[] positions=new Vector3[nodes.Count];
        for (int i = 0; i < nodes.Count; i++)
        {
            positions[i]=nodes[i].transform.position;
        }
        line.SetPositions(positions);
    }

}
