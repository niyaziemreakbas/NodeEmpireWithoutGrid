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

    public void InitializeNodeLine(){
        line=GetComponent<LineRenderer>();
        nodePositions=new List<Vector3>();
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

    
    public void SetLinesByNodes(){
        line.positionCount=nodePositions.Count;
        Vector3[] positions=new Vector3[nodePositions.Count];
        for (int i = 0; i < nodePositions.Count; i++)
        {
            positions[i]=nodePositions[i];
        }
        line.SetPositions(positions);
    }

    public void UpdateLastPosition(Vector3 pos){
        if(nodePositions.Count==0) return;
        nodePositions[0]=pos;
        line.SetPosition(0,pos);
    }

}
