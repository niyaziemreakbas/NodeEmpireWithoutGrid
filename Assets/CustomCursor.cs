using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("girdi");
        print(collision.gameObject.layer);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("çýktý");   
    }
}
