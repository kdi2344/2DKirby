using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadZone : MonoBehaviour
{
    private KirbyControl kirby;
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out kirby);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerDamaged"))
        {
            //kirby.dieAnimation();
            GameManager._instance.currentHP = 0;
        }
    }
}
