using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncStar : MonoBehaviour
{
    [SerializeField] int type = 0; //0�� �پ�ٴϴ� ��, 1�� �� ���� �ִ� ��
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
                destination = kirby.transform.position; //��ǥ ����
                Vector3 inhaleSpeed = new Vector3(0, 0, 0);
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref inhaleSpeed, 0.1f); //��ǥ�������� �̵�
                coll.isTrigger = true;
                gameObject.layer = 10; //�̵��ϴ� ���� ���̾� �ٸ� ������ ó�� -> �浹 ���ø� ���� 
                if (gameObject.transform.position.x <= destination.x + 0.04f && gameObject.transform.position.x >= destination.x - 0.04f) //��������
                {
                    gameObject.SetActive(false);
                    kirby.GetComponent<KirbyControl>().anim.SetBool("isInhale", true);
                }
            }
            else
            {
                if (kirby.GetComponent<KirbyControl>().change != 0)
                {
                    //���� �� �Ķ��
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
