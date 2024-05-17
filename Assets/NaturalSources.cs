using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalSources : MonoBehaviour
{
    [SerializeField] private SourceType sourceType;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Node"))
        {
            collision.gameObject.GetComponent<NodeResources>().SetResourceProductionSpeed(sourceType);
        }
    }

    
}

public enum SourceType
{
    Water,
    Food,
    Stone
}
