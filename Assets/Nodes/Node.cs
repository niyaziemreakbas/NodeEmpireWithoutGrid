using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    bool foodSource;

    bool waterSource;


    public Transform raycastOrigin;
    public float raycastDistance = 1.0f;

    private void Start()
    {
        raycastOrigin = transform;
    }

    void Update()
    {
        // Raycast �izgisi �iz
        Ray ray = new Ray(raycastOrigin.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // �arp��ma tespit edildi
            if (hit.collider.CompareTag("Water"))
            {
                Debug.Log("Su bulundu");
                waterSource = true;
            }
            // �arp��ma tespit edildi
            else if (hit.collider.CompareTag("Food"))
            {
                Debug.Log("Yemek bulundu");
                foodSource = true;
            }
        }
    }
}
