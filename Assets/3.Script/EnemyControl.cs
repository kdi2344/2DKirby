using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    //능력따라 change 숫자 바꾸기
    public int change = 1;

    Rigidbody2D rigid;
    [SerializeField] float speed = 1f;
    private int nextMove = 0;//행동지표를 결정할 변수
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
        if (findKirby && !isInhaled) //커비가 공격 범위에 있는 동안 공격하기 && 빨려들어가는 중이 아니라면 가만히 있기
        {
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

            if (isInhaled) //빨려들어가는 중이면 
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
            //한 방향으로만 알아서 움직이게
            if (!isInhaled) //흡입되는 중이 아닐때만 걸을 수 잇음
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
                rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);//왼쪽으로 가니까 -1, y축은 0을 넣으면 큰일남!
            }
            else
            {
                nextMove = 0;
            }

            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            // 시작,방향 색깔

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 0.5f, LayerMask.GetMask("ground"));


            if (rayHit.collider == null)
            {
                nextMove = nextMove * (-1);
                CancelInvoke();
                Invoke("Think", thinkTime);
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
        // 투명하게 변함
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //방향 바꿔주기
        spriteRenderer.flipY = true;
        //콜라이더 비활성화
        circleCollider.enabled = false;
        //die effect jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        
        //dead존에 부딪히면 삭제 하는 식 만들기
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }

    public void BeingInhaled()
    {
        anim.SetBool("isBeingInhaled", true);
        destination = kirby.transform.position; //목표 지점
        Vector3 speed =  new Vector3(0, 0, 0); 
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref speed, 0.1f); //목표지점까지 이동
        gameObject.layer = 10; //이동하는 동안 레이어 다른 곳으로 처리 -> 충돌 무시를 위해 
        rigid.gravityScale = 0; //이동하는 동안은 무중력
        circleCollider.isTrigger = true;
        if (gameObject.transform.position.x <= destination.x + 0.09f && gameObject.transform.position.x >= destination.x - 0.09f) //먹혔으면
        {
            gameObject.SetActive(false);
            kirby.anim.SetBool("isInhale", true);
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
        while (true) //변신중이면 
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
