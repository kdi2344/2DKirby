using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    public Rigidbody2D rigid;
    private int playerLayer;
    private int platformLayer;
    private bool fallGround;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        platformLayer= LayerMask.NameToLayer("platform");
    }

    public void IgnoreLayerTrue()
    {
        Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
    }
    public void IgnoreLayerFalse()
    {
        Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
    }
    public void startCo()
    {
        StartCoroutine(LayerOpenClose());
    }
    public IEnumerator LayerOpenClose()
    {
        fallGround = true;
        IgnoreLayerTrue();
        yield return new WaitForSeconds(0.2f);
        fallGround = false;
    }
}
