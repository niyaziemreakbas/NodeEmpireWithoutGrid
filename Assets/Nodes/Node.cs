using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    bool foodSource;

    bool waterSource;

    Vector2 nodeVector2;

    private void Start()
    {
        nodeVector2 = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(nodeVector2, Vector2.zero, Mathf.Infinity, layerMask);

    }



    void Update()
    {
        Ray ray = new Ray(raycastOrigin.position, Vector3.down);
        RaycastHit hit;

        
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Çarpýþma tespit edildi
            if (hit.collider.CompareTag("Water"))
            {
                Debug.Log("Su bulundu");
                waterSource = true;
            }
            // Çarpýþma tespit edildi
            else if (hit.collider.CompareTag("Food"))
            {
                Debug.Log("Yemek bulundu");
                foodSource = true;
            }
        }
        
    }
}
