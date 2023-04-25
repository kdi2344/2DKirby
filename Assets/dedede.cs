using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dedede : MonoBehaviour
{
    private DDDSound sound;
    private Vector3 spawnPoint;
    [SerializeField] private GameObject treasure;
    private GameObject Weapon;
    [SerializeField] private GameObject leftStar;
    [SerializeField] private GameObject rightStar;

    private float time = 0; //��� �ð� ����
    [SerializeField] private GameObject kirby;
    private Vector3 targetPosition; //Ŀ���� ��ġ
    private Animator anim; 
    private Rigidbody2D rigid;
    [SerializeField] private bool onGround = false;
    private float distance = 0.3f; //������ �Ÿ� onground �Ǵܿ�
    private int maxHP = 2;
    [SerializeField] private int currentHP;
    private bool coll = false;

    public enum State
    {
        WALK,
        FLY,
        DIE,
        IDLE,
        ATTACK,
        WAIT,
        DAMAGE,
        INHALE
    }

    public State state;

    private void Start()
    {
        TryGetComponent(out sound);
        spawnPoint = new Vector3(-5.9f, -1.24f, 0);
        currentHP = maxHP;
        Weapon = gameObject.transform.Find("Weapon").gameObject;
        TryGetComponent(out rigid);
        state = State.WAIT;
        TryGetComponent(out anim);
        targetPosition = kirby.transform.position;
        ChangeState(state);
    }

    private void Update()
    {
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("ground"));
        if ((rayHit.collider != null && rayHit.distance < distance))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
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
            case State.WAIT:
                Wait();
                break;
            case State.DAMAGE:
                Damage();
                break;
            case State.INHALE:
                Inhale();
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            ChangeState(State.DAMAGE);
        }
        switch (state)
        {
            case State.WALK:
                WalkTrigger(other);
                break;
        }
    }

    private void ChangeState(State state) //��ȯ�ϱ� �� ���������� ����Ǵ� Exit �Լ�
    {
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
            case State.WAIT:
                WaitExit();
                break;
            case State.DAMAGE:
                DamageExit();
                break;
            case State.INHALE:
                InhaleExit();
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
            case State.WAIT:
                WaitEnter();
                break;
            case State.DAMAGE:
                DamageEnter();
                break;
            case State.INHALE:
                InhaleEnter();
                break;
        }
    }

    private void WalkEnter()
    {
        anim.SetBool("isWalking", true);
    }
    private void Walk()
    {
        if (targetPosition.x < transform.position.x) //Ŀ�� ���𺸴� �����̸�
        {
            transform.localScale = new Vector3(1, 1, 1); //���� ���鼭
            transform.position += new Vector3(-0.8f * Time.deltaTime, 0, 0); //�������� ����
        }
        else //Ŀ�� ���𺸴� �������̸�
        {
            transform.localScale = new Vector3(-1, 1, 1); //������ ���鼭
            transform.position += new Vector3(0.8f * Time.deltaTime, 0, 0); //���������� ����
        }

        if ( transform.position.x > targetPosition.x - 0.2f && transform.position.x < targetPosition.x + 0.2f)
        {
            ChangeState(State.ATTACK);
        }
    }
    private void WalkTrigger(Collider2D other)
    {
    }
    private void WalkTriggerStay(Collider2D other)
    {
    }
    private void WalkExit()
    {
        anim.SetBool("isWalking", false);
    }

    private void FlyEnter()
    {
        if (onGround && !anim.GetBool("isWalking"))
        {
            anim.SetBool("isJump", true);
            rigid.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
        }
    }
    private void Fly()
    {
        //�ٴ� Ȯ��
        if (rigid.velocity.y < 0 && onGround)
        {
            anim.SetBool("onGround", true);
            ChangeState(State.WAIT);
        }
    }
    private void FlyExit()
    {
        anim.SetBool("isJump", false);
    }

    private void DieEnter()
    {
        sound.playSound("Kill");
        anim.SetBool("isDead", true);
        CancelInvoke();
        gameObject.layer = 14;
        rigid.velocity = Vector2.zero;
    }
    private void Die()
    {
        time += Time.deltaTime;
        if (time > 4f)
        {
            Destroy(gameObject);
            ChangeState(State.IDLE);
        }
    }
    private void DieExit()
    {
        Instantiate(treasure, spawnPoint, Quaternion.identity);
    }

    private void IdleEnter()
    {
    }
    private void Idle()
    {
        if (anim.GetBool("isDead")) return;
        targetPosition = kirby.transform.position;
        if (targetPosition.y > -0.5f)
        {
            return; //Ŀ�� �ٸ� ���������� ���� �� �ȿ�����
        }
        if (targetPosition.x < transform.position.x) //���𺸴� ���ʿ� ������ ������ ��
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else //���𺸴� �����ʿ� Ŀ�� ������ ������ ����
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (transform.position.x - targetPosition.x > 1.5f || transform.position.x - targetPosition.x < -1.5f) //x �Ÿ��� �־����� �ɾ�� ����
        {
            ChangeState(State.WALK);
        }
        else if (transform.position.y - targetPosition.y > 1f || transform.position.y - targetPosition.y < -1f) //y �Ÿ��� �־����� ���� �����ؼ� ����
        {
            ChangeState(State.FLY);
        }
        else
        {
            anim.SetBool("stopInhale", false);
            ChangeState(State.INHALE);
        }
    }
    private void IdleExit()
    {
    }

    private void AttackEnter()
    {
        anim.SetTrigger("triggerAttack");
        anim.SetBool("isAttack", true);
        rigid.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
        distance = 0.5f;
    }
    private void Attack()
    {
        if (rigid.velocity.y < 0 && onGround)
        {
            anim.SetBool("onGround", true);
            ChangeState(State.WAIT);
        } //�ٴ� Ȯ��
    }
    private void AttackExit()
    {
        sound.playSound("Attack");
        Instantiate(leftStar, Weapon.transform.position - new Vector3(0.2f, 0, 0), Quaternion.identity);
        Instantiate(rightStar, Weapon.transform.position + new Vector3(0.2f, 0, 0), Quaternion.identity);
        distance = 0.3f;
    }

    private void WaitEnter()
    {
        time = 0;
    }
    private void Wait()
    {
        time += Time.deltaTime;
        if (time > 1.5f)
        {
            ChangeState(State.IDLE);
        }
    }
    private void WaitExit()
    {
        anim.SetBool("onGround", false);
        anim.SetBool("isAttack", false);
        anim.SetBool("stopInhale", false);
        time = 0;
    }

    private void DamageEnter()
    {
        sound.playSound("Damage");
        gameObject.layer = LayerMask.NameToLayer("EnemyDamaged");
        //CancelInvoke();
        Invoke("BeOriginalLayer", 3.5f);
        anim.SetBool("isWalking", false);
        anim.SetBool("onGround", false);
        anim.SetBool("isAttack", false);
        distance = 0.3f;
        time = 0;
        anim.SetTrigger("triggerDamaged");
        currentHP -= 1;
    }
    private void Damage()
    {
        if (currentHP <= 0)
        {
            ChangeState(State.DIE);
        }
        else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("dedede_damaged"))
        {
            ChangeState(State.IDLE);
        }
    }
    private void DamageExit()
    {
    }
    
    private void InhaleEnter()
    {
        if (targetPosition.x < transform.position.x) //���𺸴� ���ʿ� ������ ������ ��
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else //���𺸴� �����ʿ� Ŀ�� ������ ������ ����
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        anim.SetTrigger("triggerInhale");
        //CancelInvoke();
        Invoke("stopInhale", 2f);
    }
    private void Inhale()
    {
        if (anim.GetBool("stopInhale")) //�ð��� 2�ʰ� ������, Ŀ�� ��Ű�� wait���� �ٲٱ�
        {
            ChangeState(State.WAIT);
        }
        if (transform.localScale.x == 1 && (kirby.transform.position.x < gameObject.transform.position.x) && (kirby.transform.position.x > gameObject.transform.position.x - 1f))
        {//���� ������ ���������� Ŀ�� ���ʿ� �ִ°Ŵϱ�
            kirby.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
            if (coll) //�����ͼ� �ε������� ��
            {
                CancelInvoke();
                anim.SetBool("stopInhale", true);
                kirby.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (transform.localScale.x == -1 && (kirby.transform.position.x > gameObject.transform.position.x) && (kirby.transform.position.x < gameObject.transform.position.x + 1f))
        {//���� �������� ���������� Ŀ�� �����ʿ� �ִ°Ŵϱ�
            kirby.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0);
            if (coll) //�����ͼ� �ε������� ��
            {
                CancelInvoke();
                anim.SetBool("stopInhale", true);
                kirby.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision!= null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            coll = true;
            kirby.GetComponent<KirbyControl>().OnDamaged(gameObject.transform.position);
        }
        else
        {
            anim.SetBool("stopInhale", false);
            coll = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        coll = false;
    }
    private void InhaleExit()
    {
        kirby.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }


    private void BeOriginalLayer()
    {
        gameObject.layer = 23; //Boss layer�� ���ư���
    }
    private void stopInhale()
    {
        anim.SetBool("stopInhale", true);
    }

}
