using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalSources : MonoBehaviour
{
    [SerializeField] private SourceType sourceType;
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Node"))
        {
            Debug.Log("worked");
            other.gameObject.GetComponent<NodeResources>().SetResourceProductionSpeed(sourceType);
        }
    }

    

    
}

public enum SourceType
{
    Water,
    Food,
    Stone
}
