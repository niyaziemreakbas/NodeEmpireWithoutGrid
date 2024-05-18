using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableMenu : MonoBehaviour
{
    public int towerStartingCount = 4;

    [SerializeField] private GameObject dynamitePrefab;
    [SerializeField] private GameObject towerSpawnPoints;
    [SerializeField] private GameObject towerPrefab;


    private void OnEnable()
    {
        DestroyItself.OnTowerDestroyed += CreateTower;
    }

    private void OnDisable()
    {
        DestroyItself.OnTowerDestroyed -= CreateTower;
    }

    private void Start()
    {
        for(int i = 0; i < towerStartingCount ; i++)
        {
            CreateTower();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Instantiate(dynamitePrefab, mousePosition ,Quaternion.identity);
        }
    }

    public void CreateTower()
    {

        Vector3 pos = towerSpawnPoints.transform.GetChild(Random.Range(0, towerSpawnPoints.transform.childCount)).transform.position;
        Instantiate(towerPrefab,pos, Quaternion.identity);
    }
}
