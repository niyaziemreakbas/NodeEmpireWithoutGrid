using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePlacement : MonoBehaviour
{
    [SerializeField] private CustomCursor customCursor;

    public Sprite sprite;

    public GameObject house;

    bool isBuilding;

    public LayerMask layerMask;



    public void BuyBuilding()
    {
        isBuilding = true;

        customCursor.gameObject.SetActive(true);

        customCursor.GetComponent<SpriteRenderer>().sprite = sprite;

        Cursor.visible = false;
    }

    private void Update()
    {
        if (isBuilding)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider == null)
            {
                if (Input.GetMouseButton(0))
                {
                    Instantiate(house, mousePosition, Quaternion.identity);
                    customCursor.gameObject.SetActive(false);
                    Cursor.visible = true;
                    isBuilding = false;
                }
            }
        }

    }
}
