using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class healItem : MonoBehaviour
{
    [SerializeField] private int hpHeal;
    private KirbyControl kirby;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out kirby);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerDamaged"))
        {
            kirby.SoundPlay("Heal");
            Destroy(gameObject);
        }
        GameManager._instance.currentHP += hpHeal;
    }
}
