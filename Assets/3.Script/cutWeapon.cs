using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutWeapon : MonoBehaviour
{
    [SerializeField] private float maxVel = 2;

    private bool kirbyLeft;
    private float timer;
    private float waitingTime;

    private Rigidbody2D rigid;

    private float speed = 1.15f;

    private GameObject kirby;
    private Vector3 destination;
    private SpriteRenderer spriteRenderer;
    public bool isgone = false;
    [SerializeField]private float range = 1f;

    private void Awake()
    {
        timer = 0.0f;
        waitingTime = 0.1f;

        TryGetComponent(out rigid);
        kirby = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = kirby.GetComponent<KirbyControl>().spriteRenderer;
        if (spriteRenderer.flipX == true) //왼쪽
        {
            kirbyLeft = true;
            destination = kirby.transform.position + new Vector3(-2.5f, 0, 0);
        }
        else
        {
            kirbyLeft = false;
            destination = kirby.transform.position + new Vector3(2.5f, 0, 0);
        }
        Invoke("DestroyCut", 3f);
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (!isgone)
        {
            Vector3 speed = new Vector3(0, 0, 0);
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref speed, 0.12f);
        }
        if (!isgone && (transform.position.x >= destination.x - range * 0.5f && transform.position.x <= destination.x + range * 0.5f))
        {
            isgone = true;
            if (kirbyLeft)//왼쪽이면
            {
                rigid.velocity = new Vector2(0.3f, 0);
            }
            else //오른쪽이면
            {
                rigid.velocity = new Vector2(-0.3f, 0);
            }
        }
        if (isgone && timer > waitingTime)
        {
            timer = 0;
            float x = rigid.velocity.x * speed;
            rigid.velocity = new Vector2(x, rigid.velocity.y);
            if (!kirbyLeft && rigid.velocity.x >= maxVel)
            {
                rigid.velocity = new Vector2(maxVel, rigid.velocity.y);
            }
            if (kirbyLeft && rigid.velocity.x <= maxVel * -1)
            {
                rigid.velocity = new Vector2(maxVel * -1, rigid.velocity.y);
            }
        }
    }
    private void DestroyCut()
    {
        Destroy(gameObject);
    }
}
