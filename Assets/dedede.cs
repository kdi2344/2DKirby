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
    [SerializeField] private int nextMove = 0; //�ൿ ����
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

    private void ChangeState(State state) //��ȯ�ϱ� �� ���������� ����Ǵ� Exit �Լ�
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

        //state���� ���� ���� ó������ ����Ǵ� Enter �Լ�
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
        if (targetPosition.x < transform.position.x) //Ŀ�� ���𺸴� �����̸�
        {
            transform.localScale = new Vector3(1, 1, 1); //���� ���鼭
            AttackRange.x = -AttackRangeX;
            transform.position += new Vector3(-0.5f * Time.deltaTime, 0, 0); //�������� ����
        }
        else //Ŀ�� ���𺸴� �������̸�
        {
            transform.localScale = new Vector3(-1, 1, 1); //������ ���鼭
            AttackRange.x = AttackRangeX;
            transform.position += new Vector3(0.5f * Time.deltaTime, 0, 0); //���������� ����
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
            return; //Ŀ�� �ٸ� ���������� ���� �� �ȿ�����
        }
        if (targetPosition.x < transform.position.x) //���𺸴� ���ʿ� ������ ������ ��
        {
            transform.localScale = new Vector3(1, 1, 1);
            AttackRange.x = -AttackRangeX;
        }
        else //���𺸴� �����ʿ� Ŀ�� ������ ������ ����
        {
            transform.localScale = new Vector3(-1, 1, 1);
            AttackRange.x = AttackRangeX;
        }
        if (transform.position.x - targetPosition.x > 1.5f || transform.position.x - targetPosition.x < -1.5f) //x �Ÿ��� �־����� �ɾ�� ����
        {
            ChangeState(State.WALK);
        }
        else if (transform.position.y - targetPosition.y > 1f || transform.position.y - targetPosition.y < -1f) //y �Ÿ��� �־����� ���� �����ؼ� ����
        {
            ChangeState(State.FLY);
        }
        Collider2D hitColliders = Physics2D.OverlapBox(transform.position + AttackRange, size, 0, whatIsLayer);
        if (hitColliders != null)
        {
            Debug.Log("Ŀ�� ã��");
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

    private void OnDrawGizmos() //���� ���� ������ ���̰�
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + AttackRange, size);
    }
    private void think()
    {
        nextMove = Random.Range(-1, 2); //-1 0 1
        if (nextMove == -1)
        {
            //����
        }
        else if (nextMove == 0)
        {
            //����
        }
        else if (nextMove == 1)
        {
            //�پ ����
        }
    }
}
