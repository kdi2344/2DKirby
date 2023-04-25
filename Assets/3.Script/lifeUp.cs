using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeUp : MonoBehaviour
{
    private KirbyControl kirby;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out kirby);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerDamaged"))
        {
            kirby.SoundPlay("Lifeup");
            Destroy(gameObject);
        }
        GameManager._instance.life += 1;
    }
}
