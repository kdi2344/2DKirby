using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireWeapon : MonoBehaviour
{
    Rigidbody2D rigid;
    private bool onGround;
    private GameObject kirby;
    private SpriteRenderer spriteRenderer;
    private float timer;

    private void Awake()
    {
        timer = 0f;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        kirby = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = kirby.GetComponent<KirbyControl>().spriteRenderer;
        if (spriteRenderer.flipX == true) //왼쪽
        {
            rigid.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.left * 2f, ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.right  * 2f, ForceMode2D.Impulse);
        }

    }
    void Start()
    {
        
    }

    private void Update()
    {
        timer += Time.deltaTime;
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        // 시작,방향 색깔

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("ground"));

        //시작,방향,빔 길이,레이어
        //raycasthit 레이어에 해당하는 애만 구별하겠다는거
        //빔에 맞았는지 
        if ((rayHit.collider != null && rayHit.distance < 0.1f))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        if (onGround)
        {
            Destroy(gameObject);
        }
        else
        {
            if (timer > 2f)
            {
                Destroy(gameObject);
            }
        }
    }

}
