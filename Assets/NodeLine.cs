using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLine : MonoBehaviour
{
    LineRenderer line;
    public List<Vector3> nodePositions;
    void Start()
    {
        line=GetComponent<LineRenderer>();
        SetLinesByNodes();
    }
    public void InitializeNodeLine(List<Vector3> positions){
        line=GetComponent<LineRenderer>();
        nodePositions=positions;
        SetLinesByNodes();
    }
    public void AppendNodeToLine(Vector3 pos){
        nodePositions.Add(pos);
        SetLinesByNodes();
    }

    public bool IsLastNodeOnLine(int nodeIndex){
        return nodePositions.Count-1==nodeIndex;
    }
    
    public void SetLinesByNodes(){
        line.positionCount=nodePositions.Count;
        Vector3[] positions=new Vector3[nodePositions.Count];
        for (int i = 0; i < nodePositions.Count; i++)
        {
            positions[i]=nodePositions[i];
        }
        line.SetPositions(positions);
    }

}
