﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ObjectChangeEventStream;

public class NodeAI : MonoBehaviour
{
    public Vector2 closestStone;
    public Vector2 closestFood;
    public Vector2 closestWater;

    public float closestStoneDistance;
    public float closestFoodDistance;
    public float closestWaterDistance;

    Vector2 centerPoint;

    public float searchRadius = 5.0f; // Arama yar��ap�

    /// Belirli bir merkez nokta ve yar��ap ile dairenin i�indeki en yak�n tag'ine sahip nesnenin pozisyonunu d�ner
    public Vector2 FindNearestTargetInCircle(float radius, string target)
    {
        centerPoint = transform.position;
        Vector2 closestPoint = Vector2.zero;
        int sourceLayerMask = LayerMask.GetMask("Source");

        RaycastHit2D hit = Physics2D.CircleCast(centerPoint, radius, Vector2.zero, Mathf.Infinity, sourceLayerMask);
        if (hit.collider != null && hit.collider.CompareTag(target))
        {
            Debug.Log("Found a hit");
            closestPoint = Physics2D.ClosestPoint(centerPoint, hit.collider);
        }



        // E�er daire i�inde hi� tag'ine sahip nesne yoksa, Vector2.zero d�ner
        return closestPoint;
    }

    // Debug ama�l�, sahnede daireyi �izer
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius); // �rne�in 5 birimlik bir daire
    }

    private void Start()
    {
        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "food" nesnesinin pozisyonunu al
            closestFood = FindNearestTargetInCircle(searchRadius, "Food");

        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "food" nesnesinin pozisyonunu al
        closestStone = FindNearestTargetInCircle(searchRadius, "Stone");

        // NearestFoodFinder script'ini kullanarak daire i�indeki en yak�n "food" nesnesinin pozisyonunu al
        closestWater = FindNearestTargetInCircle(searchRadius, "Water");

        closestWaterDistance = (closestWater != Vector2.zero) ? Vector2.Distance(centerPoint, closestWater) : Mathf.Infinity;
        closestStoneDistance = (closestStone != Vector2.zero) ? Vector2.Distance(centerPoint, closestStone) : Mathf.Infinity;
        closestFoodDistance = (closestFood != Vector2.zero) ? Vector2.Distance(centerPoint, closestFood) : Mathf.Infinity;

        Debug.Log("Nearest food position in circle: " + closestFood + ", Distance: " + closestFoodDistance);
        Debug.Log("Nearest stone position in circle: " + closestStone + ", Distance: " + closestStoneDistance);
        Debug.Log("Nearest water position in circle: " + closestWater + ", Distance: " + closestWaterDistance);
    }

}
