using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] int hp = 1; //기본 몬스터는 피가 1, 아니면 고치기
    private int currentHp;

    //능력따라 change 숫자 바꾸기
    public int change = 1;
    public int stage = 1; //없애기 용 변수
    public int type = 0;
    [SerializeField] private bool onGround = true;

    Rigidbody2D rigid;
    [SerializeField] float speed = 1f;
    [SerializeField] private int nextMove = 0;//행동지표를 결정할 변수
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
            if (gameObject.layer == 14 && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Die")) //죽은처리가 되었는데 die animation이 아니면 끄기
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
            if (findKirby && !isInhaled) //커비가 공격 범위에 있는 동안 공격하기 && 빨려들어가는 중이 아니라면 가만히 있기
            {
                BeOriginalLayer();
                anim.SetBool("isWalking", false);
                if (timer > waitingTime)
                {
                    Attack(); //각자의 공격패턴 진행하기 -> 다른 스크립트에 공격패턴 각자 짜기
                    timer = 0;
                }
                else
                {
                    anim.SetBool("isAttack", false);
                }
            }
            else if (!isAttack) //커비가 공격 범위 안에 없고 공격중이 아니면 돌아댕기기
            {
                anim.SetBool("isAttack", false);

                //한 방향으로만 알아서 움직이게
                if (!isInhaled && !isAttack) //흡입되는 중이 아닐때만 걸을 수 잇음
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
                        if (nextMove == 1) //오른쪽 무빙
                        {
                            Debug.DrawRay(rigid.position, new Vector3(0.15f, 0, 0), new Color(1, 1, 0));
                            RaycastHit2D frontCheck = Physics2D.Raycast(rigid.position, Vector3.right, 0.15f, LayerMask.GetMask("ground")); //땅과의 충돌 
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
                            RaycastHit2D frontCheck = Physics2D.Raycast(rigid.position, Vector3.left, 0.15f, LayerMask.GetMask("ground")); //땅과의 충돌 
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

                    if (type == 0) //걸어다니는 타입은 걸어다니고 
                    {
                        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y); //왼쪽으로 가니까 -1, y축은 0을 넣으면 큰일남! 
                    }

                    else if (type == 1) //점프만 하는 타입은 좌우로 점프
                    {
                        if (nextMove == 1 && onGround) //왼쪽으로 점프 
                        {
                            rigid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
                            rigid.AddForce(Vector2.left, ForceMode2D.Impulse);
                            CancelInvoke();
                            nextMove = 0;
                            Invoke("Think", thinkTime);
                            anim.SetTrigger("jump");
                        }
                        else if (nextMove == -1 && onGround) //오른쪽으로 점프 
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

                // 아래로 raycast그려서 땅인지 확인
                Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
                Debug.DrawRay(frontVec, Vector3.down * 0.2f, new Color(0, 1, 0));

                RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down * 0.2f, 0.2f, LayerMask.GetMask("ground")); //땅과의 충돌 
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

    //행동지표를 바꿔줄 함수 생각 --> 랜덤클래스 활용 

    void Think()
    {
        nextMove = Random.Range(-1, 2); //-1이면 왼쪽, 0이면 멈추기,1이면 오른쪽으로이동

        Invoke("Think", thinkTime);//재귀
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
            Invoke("BeOriginalLayer", 0.2f); //0.5초간 무적
        }
        //0.1초간 빨간색으로 바뀌기
    }
    

    private void Pause()  //공중으로 날다가 정지
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
                rigid.gravityScale = 0; //이동하는 동안은 무중력
            }
            destination = kirby.transform.position; //목표 지점
            Vector3 speed = new Vector3(0, 0, 0);
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref speed, 0.1f); //목표지점까지 이동
            gameObject.layer = 10; //이동하는 동안 레이어 다른 곳으로 처리 -> 충돌 무시를 위해 
            if (gameObject.transform.position.x <= destination.x + 0.09f && gameObject.transform.position.x >= destination.x - 0.09f) //먹혔으면
            {
                gameObject.SetActive(false);
                kirby.anim.SetBool("isInhale", true);
                kirby.anim.SetBool("isStartInhale", false);
            }
        }
        
    }


    void MyCollisions() //범위에 커비가 있는지 확인 후 공격
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
    private void OnDrawGizmos() //공격 범위 눈으로 보이게
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
            //Debug.Log("플레이어의 공격 받음");
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

    void Die() //체력 닳기로 바꾸기
    {
        anim.SetBool("isDead", true);
        gameObject.layer = 14; //Enemy Damaged로 바꿈
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
        while (true) //공격중이면
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
