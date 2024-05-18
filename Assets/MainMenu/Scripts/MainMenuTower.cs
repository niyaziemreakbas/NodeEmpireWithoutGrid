using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTower : MonoBehaviour
{


    [SerializeField] private List<GameObject> damageLevels;
    [SerializeField] private GameObject collapsedTower;
    
    int damageLevel = 0;

    private void OnEnable()
    {
        Dynamite.OnDynamiteExploded += MainMenuTower_OnDynamiteExploded;
    }

    private void MainMenuTower_OnDynamiteExploded(MainMenuTower tower)
    {
        if (tower != this) return;

        AudioManager.instance.ExplosionSF();

        if (damageLevel >= damageLevels.Count)
        {
            
            Instantiate(collapsedTower, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            return;
        }

        damageLevels[damageLevel].SetActive(true);
        damageLevel++;
    }

    private void OnDisable()
    {
        Dynamite.OnDynamiteExploded -= MainMenuTower_OnDynamiteExploded;
    }
}
