using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class healItem : MonoBehaviour
{
    [SerializeField] private int hpHeal;

    private void Awake()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerDamaged"))
        {
            Destroy(gameObject);
        }
        GameManager._instance.currentHP += hpHeal;
    }
}
