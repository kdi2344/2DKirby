using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] int hp = 1; //�⺻ ���ʹ� �ǰ� 1, �ƴϸ� ��ġ��
    private int currentHp;

    //�ɷµ��� change ���� �ٲٱ�
    public int change = 1;
    public int stage = 1; //���ֱ� �� ����
    public int type = 0;
    [SerializeField] private bool onGround = true;

    Rigidbody2D rigid;
    [SerializeField] float speed = 1f;
    [SerializeField] private int nextMove = 0;//�ൿ��ǥ�� ������ ����
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
        currentHp = hp;
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
        if (type == 10)
        {
            if (isInhaled) BeingInhaled();
            else
            {
                BeOriginalLayer();
                return;
            }
        }
        else
        {
            if (gameObject.layer == 14 && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Die")) //����ó���� �Ǿ��µ� die animation�� �ƴϸ� ����
            {
                return;
            }
            else if (anim.GetBool("isBeingInhaled"))
            {
                anim.SetBool("isWalking", false);
                rigid.velocity = Vector2.zero;
                nextMove = 0;
            }
            else
            {
                anim.SetBool("isDead", false);
                timer += Time.deltaTime;
                StartCoroutine("CheckAnimationState");
            }
            if (rigid.velocity.y == 0)
            {
                onGround = true;
            }
            else
            {
                onGround = false;
            }

            MyCollisions();
            if (findKirby && !isInhaled) //Ŀ�� ���� ������ �ִ� ���� �����ϱ� && �������� ���� �ƴ϶�� ������ �ֱ�
            {
                BeOriginalLayer();
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

                //�� �������θ� �˾Ƽ� �����̰�
                if (!isInhaled && !isAttack) //���ԵǴ� ���� �ƴҶ��� ���� �� ����
                {
                    BeOriginalLayer();
                    destination = gameObject.transform.position;
                    MyCollisions();
                    anim.SetBool("isBeingInhaled", false);
                    rigid.gravityScale = 1;
                    circleCollider.isTrigger = false;
                    if (nextMove == -1 || nextMove == 1)
                    {
                        anim.SetBool("isWalking", true);
                        if (nextMove == 1) //������ ����
                        {
                            Debug.DrawRay(rigid.position, new Vector3(0.15f, 0, 0), new Color(1, 1, 0));
                            RaycastHit2D frontCheck = Physics2D.Raycast(rigid.position, Vector3.right, 0.15f, LayerMask.GetMask("ground")); //������ �浹 
                            if (frontCheck.collider != null)
                            {
                                nextMove = -1;
                            }

                            AttackRange.x = attack.x;
                            transform.localScale = new Vector3(1, 1, 1);
                        }
                        else if (nextMove == -1)
                        {
                            Debug.DrawRay(rigid.position, new Vector3(-0.15f, 0, 0), new Color(1, 1, 0));
                            RaycastHit2D frontCheck = Physics2D.Raycast(rigid.position, Vector3.left, 0.15f, LayerMask.GetMask("ground")); //������ �浹 
                            if (frontCheck.collider != null)
                            {
                                nextMove = 1;
                            }
                            AttackRange.x = -attack.x;
                            transform.localScale = new Vector3(-1, 1, 1);
                        }
                    }
                    else
                    {
                        anim.SetBool("isWalking", false);
                    }

                    if (type == 0) //�ɾ�ٴϴ� Ÿ���� �ɾ�ٴϰ� 
                    {
                        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y); //�������� ���ϱ� -1, y���� 0�� ������ ū�ϳ�! 
                    }

                    else if (type == 1) //������ �ϴ� Ÿ���� �¿�� ����
                    {
                        if (nextMove == 1 && onGround) //�������� ���� 
                        {
                            rigid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
                            rigid.AddForce(Vector2.left, ForceMode2D.Impulse);
                            CancelInvoke();
                            nextMove = 0;
                            Invoke("Think", thinkTime);
                            anim.SetTrigger("jump");
                        }
                        else if (nextMove == -1 && onGround) //���������� ���� 
                        {
                            rigid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
                            rigid.AddForce(Vector2.right, ForceMode2D.Impulse);
                            CancelInvoke();
                            nextMove = 0;
                            Invoke("Think", thinkTime);
                            anim.SetTrigger("jump");
                        }
                    }
                }
                else
                {
                    BeingInhaled();
                    nextMove = 0;
                }

                // �Ʒ��� raycast�׷��� ������ Ȯ��
                Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
                Debug.DrawRay(frontVec, Vector3.down * 0.2f, new Color(0, 1, 0));

                RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down * 0.2f, 0.2f, LayerMask.GetMask("ground")); //������ �浹 
                RaycastHit2D platHit = Physics2D.Raycast(frontVec, Vector3.down * 0.2f, 0.2f, LayerMask.GetMask("platform"));

                if (rayHit.collider == null)
                {
                    if (platHit.collider == null)
                    {
                        nextMove = nextMove * (-1);
                        CancelInvoke();
                        Invoke("Think", thinkTime);
                    }
                    
                }
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
        currentHp -= 1;
        Invoke("BeingRed", 0.1f);
        anim.SetBool("isDead", true);
        gameObject.layer = 14;
        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            Invoke("BeOriginalLayer", 0.2f); //0.5�ʰ� ����
        }
        //0.1�ʰ� ���������� �ٲ��
    }
    

    private void Pause()  //�������� ���ٰ� ����
    {
        rigid.velocity = Vector2.zero;
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }

    public void BeingInhaled()
    {
        isAttack = false;
        if (isInhaled)
        {
            if (type != 10)
            {
                gameObject.transform.Find("Weapon").gameObject.SetActive(false);
                anim.SetBool("isBeingInhaled", true);
                circleCollider.isTrigger = true;
                rigid.gravityScale = 0; //�̵��ϴ� ������ ���߷�
            }
            destination = kirby.transform.position; //��ǥ ����
            Vector3 speed = new Vector3(0, 0, 0);
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref speed, 0.1f); //��ǥ�������� �̵�
            gameObject.layer = 10; //�̵��ϴ� ���� ���̾� �ٸ� ������ ó�� -> �浹 ���ø� ���� 
            if (gameObject.transform.position.x <= destination.x + 0.09f && gameObject.transform.position.x >= destination.x - 0.09f) //��������
            {
                gameObject.SetActive(false);
                kirby.anim.SetBool("isInhale", true);
                kirby.anim.SetBool("isStartInhale", false);
            }
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
        //Debug.Log("enemycontrol on trigger enter" + collision.name);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Strong") && type != 10)
        {
            anim.SetBool("isAttack", false);
            int dirc = transform.position.x - collision.transform.position.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 1) * 0.5f, ForceMode2D.Impulse);
            OnDamaged();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon") && gameObject.layer == LayerMask.NameToLayer("Enemy") && type != 10)
        {
            //Debug.Log("�÷��̾��� ���� ����");
            circleCollider.isTrigger = true;
            int dirc = transform.position.x - collision.transform.position.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 1) * 0.5f, ForceMode2D.Impulse);
            OnDamaged();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon") && type == 10)
        {
            Destroy(gameObject);
        }
    }

    void Die() //ü�� ���� �ٲٱ�
    {
        anim.SetBool("isDead", true);
        gameObject.layer = 14; //Enemy Damaged�� �ٲ�
        rigid.gravityScale = 0;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        Invoke("Pause", 0.2f);
        Invoke("DeActive", 1f);
    }

    public void Respawn()
    {
        //anim.SetBool("isDead", false);
        gameObject.layer = 7;
        //rigid.gravityScale = 1;
        //spriteRenderer.color = new Color(1, 1, 1, 1);
        currentHp = hp;
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

    void BeingRed()
    {
        spriteRenderer.color = Color.red;
        Invoke("BeingOriginal", 0.1f);
    }
    void BeingOriginal()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    void BeOriginalLayer()
    {
        gameObject.layer = 7;
    }
}
