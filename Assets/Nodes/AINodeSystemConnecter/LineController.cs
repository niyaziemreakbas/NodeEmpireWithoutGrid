using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform[] positions;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();



    }

    void drawLine(Transform[] positions)
    {
        lineRenderer.positionCount = positions.Length;
        this.positions = positions;
    }
}
