using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    //�ɷµ��� change ���� �ٲٱ�
    public int change = 1;

    Rigidbody2D rigid;
    [SerializeField] float speed = 1f;
    private int nextMove = 0;//�ൿ��ǥ�� ������ ����
    public Animator anim;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float thinkTime = 3f;
    private CircleCollider2D circleCollider;

    private KirbyControl kirby;
    private Vector3 destination;

    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask whatIsLayer;
    [SerializeField]Vector3 attack = new Vector3(0.3f, 0, 0);
    private Vector3 AttackRange;

    public bool isInhaled = false;

    public bool findKirby = false;

    private float timer;
    private int waitingTime;
    private bool isAttack = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        TryGetComponent(out anim);
        TryGetComponent(out spriteRenderer);
        TryGetComponent(out circleCollider);
        Invoke("Think", thinkTime);
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out kirby);
        AttackRange = attack;

        timer = 0.0f;
        waitingTime = 3;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (anim.GetBool("isDead") == true)
        {
            gameObject.SetActive(false);
        }
        timer += Time.deltaTime;

        MyCollisions();
        if (findKirby && !isInhaled) //Ŀ�� ���� ������ �ִ� ���� �����ϱ� && �������� ���� �ƴ϶�� ������ �ֱ�
        {
            anim.SetBool("isWalking", false);
            if (timer > waitingTime)
            {
                Attack(); //������ �������� �����ϱ� -> �ٸ� ��ũ��Ʈ�� �������� ���� ¥��
                timer = 0;
            }
            else
            {
                anim.SetBool("isAttack", false);
            }
        }
        else if (!isAttack) //Ŀ�� ���� ���� �ȿ� ���� �������� �ƴϸ� ���ƴ���
        {
            anim.SetBool("isAttack", false);

            if (isInhaled) //�������� ���̸� 
            {
                BeingInhaled();
            }
            else
            {
                destination = gameObject.transform.position;
                MyCollisions();
                anim.SetBool("isBeingInhaled", false);
                gameObject.layer = 7;
                rigid.gravityScale = 1;
                circleCollider.isTrigger = false;
            }
            //�� �������θ� �˾Ƽ� �����̰�
            if (!isInhaled) //���ԵǴ� ���� �ƴҶ��� ���� �� ����
            {
                if (nextMove == -1 || nextMove == 1)
                {
                    anim.SetBool("isWalking", true);
                    if (nextMove == 1)
                    {
                        AttackRange.x = attack.x;
                        transform.localScale = new Vector3(1, 1, 1);
}
                    else if (nextMove == -1)
                    {
                        AttackRange.x = -attack.x;
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                else
                {
                    anim.SetBool("isWalking", false);
                }
                rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);//�������� ���ϱ� -1, y���� 0�� ������ ū�ϳ�!
            }
            else
            {
                nextMove = 0;
            }

            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            // ����,���� ����

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 0.5f, LayerMask.GetMask("ground"));


            if (rayHit.collider == null)
            {
                nextMove = nextMove * (-1);
                CancelInvoke();
                Invoke("Think", thinkTime);
            }
        }
    }

    //�ൿ��ǥ�� �ٲ��� �Լ� ���� --> ����Ŭ���� Ȱ�� 

    void Think()
    {
        nextMove = Random.Range(-1, 2); //-1�̸� ����, 0�̸� ���߱�,1�̸� �����������̵�

        Invoke("Think", thinkTime);//���
    }

    public void OnDamaged()
    {
        // �����ϰ� ����
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //���� �ٲ��ֱ�
        spriteRenderer.flipY = true;
        //�ݶ��̴� ��Ȱ��ȭ
        circleCollider.enabled = false;
        //die effect jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        
        //dead���� �ε����� ���� �ϴ� �� �����
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }

    public void BeingInhaled()
    {
        anim.SetBool("isBeingInhaled", true);
        destination = kirby.transform.position; //��ǥ ����
        Vector3 speed =  new Vector3(0, 0, 0); 
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref speed, 0.1f); //��ǥ�������� �̵�
        gameObject.layer = 10; //�̵��ϴ� ���� ���̾� �ٸ� ������ ó�� -> �浹 ���ø� ���� 
        rigid.gravityScale = 0; //�̵��ϴ� ������ ���߷�
        circleCollider.isTrigger = true;
        if (gameObject.transform.position.x <= destination.x + 0.09f && gameObject.transform.position.x >= destination.x - 0.09f) //��������
        {
            gameObject.SetActive(false);
            kirby.anim.SetBool("isInhale", true);
        }
    }


    void MyCollisions() //������ Ŀ�� �ִ��� Ȯ�� �� ����
    {
        Collider2D hitColliders = Physics2D.OverlapBox(transform.position + AttackRange, size, 0, whatIsLayer);
        if (hitColliders != null)
        {
            findKirby = true;
        }
        else
        {
            findKirby = false;
        }

    }
    private void OnDrawGizmos() //���� ���� ������ ���̰�
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + AttackRange, size);
    }

    private void Attack()
    {
        anim.SetBool("isAttack", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetBool("isDead", true);
    }

    IEnumerator CheckAnimationState()
    {
        while (true) //�������̸� 
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                isAttack = true;
            }
            else
            {
                isAttack = false;
            }
            yield return null;
        }

    }
}
