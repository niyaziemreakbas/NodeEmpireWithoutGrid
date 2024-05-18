using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    public static Action OnTowerDestroyed;
    public float destroyTime = 1f;

    private void Start()
    {
        

        Destroy(this.gameObject, destroyTime);

    }

    private void OnDestroy()
    {
        if (this.gameObject.CompareTag("CollapsedNode"))
        {
            OnTowerDestroyed?.Invoke();
        }
    }
}
