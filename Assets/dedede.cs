using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dedede : MonoBehaviour
{
    //[SerializeField] int hp = 20;
    //private int change = 7; //7 -> hammar
    //private int stage = 2;
    //private bool onGround = true;
    //private Rigidbody2D rigid;
    //[SerializeField] float speed = 1f;
    //[SerializeField] private int nextMove = 0; //�ൿ ����
    [SerializeField] private Vector3 AttackRange;
    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask whatIsLayer;

    public enum State
    {
        WALK,
        FLY,
        DIE,
        IDLE    
    }

    public State state;

    private void Start()
    {
        state = State.IDLE;
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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (state == State.WALK)
        {
            WalkTriggerStay(other);
        }
    }

    private void OnTriggerEnter(Collider other)
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
        }
        this.state = state;

        //state���� ���� ���� ó������ ����Ǵ� Enter �Լ�
        switch (state)
        {
            case State.WALK:
                WalkEnter();
                break;
        }
    }

    private void WalkEnter()
    {
        Debug.Log("Walk Enter");
    }
    private void Walk()
    {
        Debug.Log("Walk");
    }
    private void WalkTrigger(Collider other)
    {
        Debug.Log("Walk Trigger");
    }
    private void WalkTriggerStay(Collider other)
    {
        Debug.Log("Walk Trigger Stay");
    }
    private void WalkExit()
    {
        Debug.Log("Walk Exit");
    }

    private void Fly()
    {
        Debug.Log("Fly");
    }

    private void Die()
    {
        Debug.Log("Die");
    }

    private void Idle()
    {
        Debug.Log("Idle");
        Collider2D hitColliders = Physics2D.OverlapBox(transform.position + AttackRange, size, 0, whatIsLayer);
        if (hitColliders != null)
        {
            Debug.Log("Ŀ�� ã��");
            ChangeState(State.DIE);
        }
        else
        {
            
        }
    }

    private void OnDrawGizmos() //���� ���� ������ ���̰�
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + AttackRange, size);
    }
}
