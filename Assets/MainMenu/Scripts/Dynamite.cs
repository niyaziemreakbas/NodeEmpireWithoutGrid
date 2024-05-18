using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public delegate void ExplosionHandler(MainMenuTower tower);
    public static event ExplosionHandler OnDynamiteExploded;

    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private Vector3 offSet;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddTorque(Random.Range(-0.5f,0.5f),ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Node"))
        {
            OnDynamiteExploded?.Invoke(collision.gameObject.GetComponent<MainMenuTower>());
            Instantiate(explosionEffect, transform.position + offSet , Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
