using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dedede : MonoBehaviour
{
    //[SerializeField] int hp = 20;
    //private int change = 7; //7 -> hammar
    //private bool onGround = true;
    //private Rigidbody2D rigid;
    //[SerializeField] float speed = 1f;
    [SerializeField] private int nextMove = 0; //행동 패턴
    [SerializeField] private Vector3 AttackRange;
    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask whatIsLayer;
    private float time = 0;
    [SerializeField] private GameObject kirby;
    private Vector3 targetPosition;
    private Animator anim;
    private float AttackRangeX;

    public enum State
    {
        WALK,
        FLY,
        DIE,
        IDLE,
        ATTACK
    }

    public State state;

    private void Start()
    {
        AttackRangeX = AttackRange.x;
        state = State.IDLE;
        TryGetComponent(out anim);
        targetPosition = kirby.transform.position;
        ChangeState(state);
    }

    private void Update()
    {
        switch (state)
        {
            case State.WALK:
                Walk();
                break;
            case State.FLY:
                Fly();
                break;
            case State.DIE:
                Die();
                break;
            case State.IDLE:
                Idle();
                break;
            case State.ATTACK:
                Attack();
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (state == State.WALK)
        {
            WalkTriggerStay(other);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (state)
        {
            case State.WALK:
                WalkTrigger(other);
                break;
        }
    }

    private void ChangeState(State state) //전환하기 전 마지막으로 실행되는 Exit 함수
    {
        Debug.Log("Change State");
        switch (this.state)
        {
            case State.WALK:
                WalkExit();
                break;
            case State.FLY:
                FlyExit();
                break;
            case State.DIE:
                DieExit();
                break;
            case State.IDLE:
                IdleExit();
                break;
            case State.ATTACK:
                AttackExit();
                break;
        }
        this.state = state;

        //state에서 들어가고 나서 처음으로 실행되는 Enter 함수
        switch (state)
        {
            case State.WALK:
                WalkEnter();
                break;
            case State.FLY:
                FlyEnter();
                break;
            case State.DIE:
                DieEnter();
                break;
            case State.IDLE:
                IdleEnter();
                break;
            case State.ATTACK:
                AttackEnter();
                break;
        }
    }

    private void WalkEnter()
    {
        anim.SetBool("isWalking", true);
        Debug.Log("Walk Enter");
    }
    private void Walk()
    {
        if (targetPosition.x < transform.position.x) //커비가 디디디보다 왼쪽이면
        {
            transform.localScale = new Vector3(1, 1, 1); //왼쪽 보면서
            AttackRange.x = -AttackRangeX;
            transform.position += new Vector3(-0.5f * Time.deltaTime, 0, 0); //왼쪽으로 가기
        }
        else //커비가 디디디보다 오른쪽이면
        {
            transform.localScale = new Vector3(-1, 1, 1); //오른쪽 보면서
            AttackRange.x = AttackRangeX;
            transform.position += new Vector3(0.5f * Time.deltaTime, 0, 0); //오른쪽으로 가기
        }

        if ( transform.position.x > targetPosition.x - 0.2f && transform.position.x < targetPosition.x + 0.2f)
        {
            ChangeState(State.ATTACK);
        }
        //Debug.Log("Walk");
    }
    private void WalkTrigger(Collider2D other)
    {
        Debug.Log("Walk Trigger");
    }
    private void WalkTriggerStay(Collider2D other)
    {
        Debug.Log("Walk Trigger Stay");
    }
    private void WalkExit()
    {
        Debug.Log("Walk Exit");
        anim.SetBool("isWalking", false);
    }

    private void FlyEnter()
    {
        Debug.Log("Fly Enter");
        anim.SetBool("isJump", true);
    }
    private void Fly()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            time = 0;
            ChangeState(State.IDLE);
        }
        //Debug.Log("Fly");
    }
    private void FlyExit()
    {
        Debug.Log("Fly Exit");
        anim.SetBool("isJump", false);
    }

    private void DieEnter()
    {
        Debug.Log("Die Enter");
        anim.SetBool("isDead", true);
    }
    private void Die()
    {
        //Debug.Log("Die");
    }
    private void DieExit()
    {
        Debug.Log("Die Exit");
    }

    private void IdleEnter()
    {
        Debug.Log("Idle Enter");
    }
    private void Idle()
    {
        //Debug.Log("Idle");
        targetPosition = kirby.transform.position;
        if (targetPosition.y > -0.5f)
        {
            return; //커비가 다른 스테이지에 있을 땐 안움직임
        }
        if (targetPosition.x < transform.position.x) //디디디보다 왼쪽에 있으면 왼쪽을 봄
        {
            transform.localScale = new Vector3(1, 1, 1);
            AttackRange.x = -AttackRangeX;
        }
        else //디디디보다 오른쪽에 커비가 있으면 오른쪽 보기
        {
            transform.localScale = new Vector3(-1, 1, 1);
            AttackRange.x = AttackRangeX;
        }
        if (transform.position.x - targetPosition.x > 1.5f || transform.position.x - targetPosition.x < -1.5f) //x 거리가 멀어지면 걸어가서 공격
        {
            ChangeState(State.WALK);
        }
        else if (transform.position.y - targetPosition.y > 1f || transform.position.y - targetPosition.y < -1f) //y 거리가 멀어지면 높게 점프해서 공격
        {
            ChangeState(State.FLY);
        }
        Collider2D hitColliders = Physics2D.OverlapBox(transform.position + AttackRange, size, 0, whatIsLayer);
        if (hitColliders != null)
        {
            Debug.Log("커비 찾음");
        }
        else
        {
            
        }
    }
    private void IdleExit()
    {
        Debug.Log("Idle Exit");
    }

    private void AttackEnter()
    {
        Debug.Log("Attack Enter");
        anim.SetTrigger("triggerAttack");
    }
    private void Attack()
    {
        time += Time.deltaTime;
        if (time > 1.9f)
        {
            ChangeState(State.IDLE);
        }
    }
    private void AttackExit()
    {
        Debug.Log("Attack Exit");
    }

    private void OnDrawGizmos() //공격 범위 눈으로 보이게
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + AttackRange, size);
    }
    private void think()
    {
        nextMove = Random.Range(-1, 2); //-1 0 1
        if (nextMove == -1)
        {
            //왼쪽
        }
        else if (nextMove == 0)
        {
            //공격
        }
        else if (nextMove == 1)
        {
            //뛰어서 공격
        }
    }
}
