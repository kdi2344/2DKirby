using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncStar : MonoBehaviour
{
    [SerializeField] int type = 0; //0은 뛰어다니는 별, 1은 문 위에 있는 별
    [SerializeField] float speed = 90f;
    public Rigidbody2D rb;
    public Vector2 dir;
    private GameObject kirby;
    public int change = 0;
    private Vector3 destination;
    private PolygonCollider2D coll;
    public bool isInhaled = false;
    private void Start()
    {
        TryGetComponent(out coll);
        if (type == 0)
        {
            kirby = GameObject.Find("Kirby");
            change = kirby.GetComponent<KirbyControl>().change;
            kirby.GetComponent<KirbyControl>().change = 0;
            rb = GetComponent<Rigidbody2D>();
            if (kirby.GetComponent<KirbyControl>().spriteRenderer.flipX == true)
            {
                dir = new Vector2(1, 1).normalized;
            }
            else
            {
                dir = new Vector2(-1, 1).normalized;
            }
            coll.isTrigger = true;
            rb.AddForce(dir * speed * 10f);
            Invoke("remove", 5f);
        }
    }

    private void Update()
    {
        if (type == 0)
        {
            if (isInhaled)
            {
                if (rb.velocity.x > 0 || rb.velocity.y > 0)
                {
                    rb.velocity = new Vector3(0, 0, 0);
                }
                destination = kirby.transform.position; //목표 지점
                Vector3 inhaleSpeed = new Vector3(0, 0, 0);
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref inhaleSpeed, 0.1f); //목표지점까지 이동
                coll.isTrigger = true;
                gameObject.layer = 10; //이동하는 동안 레이어 다른 곳으로 처리 -> 충돌 무시를 위해 
                if (gameObject.transform.position.x <= destination.x + 0.04f && gameObject.transform.position.x >= destination.x - 0.04f) //먹혔으면
                {
                    gameObject.SetActive(false);
                    kirby.GetComponent<KirbyControl>().anim.SetBool("isInhale", true);
                }
            }
            else
            {
                if (kirby.GetComponent<KirbyControl>().change != 0)
                {
                    //변신 된 후라면
                    Destroy(gameObject);
                }
                coll.isTrigger = false;
            }
        }
        else if(type == 1)
        {
            coll.isTrigger = true;
        }
        
    }

    private void remove() 
    {
        Destroy(gameObject);
    }


}
