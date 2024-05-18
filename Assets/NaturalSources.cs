using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalSources : MonoBehaviour
{
    [SerializeField] private SourceType sourceType;
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("PlayerNode") || other.gameObject.CompareTag("EnemyNode"))
        {
            other.gameObject.GetComponent<NodeResources>().SetResourceProductionSpeed(sourceType);
        }
    }
}

public enum SourceType
{
    None,
    Water,
    Food,
    Stone
}
