using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class give_ability : MonoBehaviour
{
    [SerializeField] private int change;
    private KirbyControl kirby;
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out kirby);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerDamaged")) && kirby.change != change)
        {
            kirby.change = change;
            kirby.anim.SetTrigger("copy"); //억지로 능력 재생 애니메이션
            kirby.copyChange();
        }
    }
}
