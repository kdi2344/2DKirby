using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.SceneManagement;
using UnityEngine.UI;
=======
>>>>>>> parent of b47d110b (마리오추가)

public class KirbyControl : MonoBehaviour
{
    //��ƼŬ ���� ��� - �±׷� Particle ���̵簡 ���� �±� ����� (�̸����δ� �ϸ� �ȵ�) -> ȿ���� animScript �߰� , animator parameter�� Ʈ���� �߰�
    //[SerializeField] private GameObject splitParticles;  ������ �巡��
    //Instantiate(splitParticles, gameObject.transform.position, Quaternion.identity);
    //GameObject.FindWithTag("Particle").GetComponent<animScript>().playAnim("Ʈ�����̸�");
    //GameObject.FindWithTag("Particle").GetComponent<animScript>().waitAndDelete();
    //[SerializeField] GameObject square;

    private Vector3 destination;
    private bool isDest = false;
    private bool isDie = false;

    private bool isStar = false;
    private bool noFound = false;

    public int change = 0;
    [SerializeField] private int willChange = 0;

<<<<<<< HEAD
    public GameObject AbilitySpace;
    public GameObject Icon;
    public GameObject Number;
    private string[] ability = { "Normal", "Beam", "Spark", "Cutter", "Mario" };
    private string[] abilityIcon = { "Normal", "IconBeam", "IconSpark", "IconCutter", "IconMario" };
    private string[] lifeNumberFirst = { "first0", "first1" };
    private string[] lifeNumberLast = { "last0", "last1", "last2", "last3", "last4", "last5", "last6", "last7", "last8", "last9" };
    private int willChange = 0;

    [SerializeField] private GameObject cutWeapon; //cutter ���� ������
    
=======
>>>>>>> parent of b47d110b (마리오추가)
    [SerializeField] private float maxSpeed = 1.5f;
    [SerializeField] private float jumpmaxSpeed = 3f;
    [SerializeField] private float kirbySpeed = 2f;
    [SerializeField] private float runSpeed = 1.5f;
    [SerializeField] private float jumpPower;
    [SerializeField] private float flyPower;
    private Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public Animator anim;

    private bool isFly = false;
    [SerializeField] private float NoDamageTime = 2f; //�����ð�

    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask whatIsLayer; //layer�� �ִ¾ֶ� �ε����� Ȯ�Τ���
    [SerializeField] private LayerMask layerStar; //�� �Դ��� Ȯ�ο�
    private Vector3 InhaleRange = new Vector3(0.5f, 0, 0); //���� ���� ����

    public bool EnemyAround = false;
    public bool canInhaleSomething = false;
    private EnemyControl enemy;
    public Transform target; //������ Ÿ��
    private bool swallow = false; 
    public GameObject weaponPos; //���� collider ���� �ٲٱ����ؼ� ��

    [SerializeField] private GameObject inhaleParticlePrefabs; //���� ȿ��
    [SerializeField] private GameObject copyParticles; //���� ȿ��
    [SerializeField] private GameObject splitParticles; //��� ȿ��
    [SerializeField] private GameObject offParticles; //������ ȿ��
    [SerializeField] private GameObject stars; //������ ��

    private bool isRunning = false;
    public bool isCoping = false; //�������̸� true
    private bool onGround = true;
    public bool isAttack = false;

    private Vector3 animPosition = new Vector3 (1, 1, 1);

    private float copyTimer;
    private float timer;
    private int waitingTime;
<<<<<<< HEAD
    private float damageTimer;
    [SerializeField] private GameObject square;

    private void Awake()
    {
        sound = GetComponent<KirbySound>();
        AbilitySpace = GameObject.FindGameObjectWithTag("EditorOnly");
        Icon = GameObject.FindGameObjectWithTag("Finish");
        Number = GameObject.FindGameObjectWithTag("Respawn");

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
=======

    private void Awake()
    {
>>>>>>> parent of b47d110b (마리오추가)
        TryGetComponent(out rigid);
        rigid.freezeRotation = true;
        TryGetComponent(out spriteRenderer);
        TryGetComponent(out anim);
        anim = GetComponent<Animator>();
        GameObject.FindGameObjectWithTag("Enemy").TryGetComponent(out enemy);
        timer = 0.0f;
        waitingTime = 3;
<<<<<<< HEAD
        copyTimer = 0f;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<EnemyControl>().stage == 1)
            {
                enemies[i].SetActive(true);
            }
            else
            {
                enemies[i].SetActive(false);
            }
        }
        change = GameManager._instance.getCurrentCopy();
        activeUI();
        square.SetActive(false);
=======
>>>>>>> parent of b47d110b (마리오추가)

    }
    private void Update()
    {
<<<<<<< HEAD
        if (Time.timeScale == 0 && change != 0)
        {
            copyTimer += Time.unscaledDeltaTime;
            if (copyTimer > 1f)
            {
                square.SetActive(false);
                offPause();
                copyTimer = 0f;
            }
                
        }
        if (GameManager._instance.getCurrentHP() <= 0)
        {
            if (!isDie)
            {
                destination = transform.position + new Vector3(0, 1.5f, 0);
                Debug.Log(destination);
                Die();
                isDie = true;
            }
            else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                if (!isDest)
                {
                    if (transform.position.y > destination.y - 0.5f && transform.position.y < destination.y + 0.5f)
                    {
                        isDest = true;
                    }
                    else
                    {
                        transform.position += new Vector3(0, 2 * Time.unscaledDeltaTime, 0);
                    }
                }
                else
                {
                    transform.position -= new Vector3(0, 2 * Time.unscaledDeltaTime, 0);
                }
            }
            if (transform.position.y < -5f)
            {
                GameManager._instance.die();
                Scene currentScene =  SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
                anim.SetBool("isStart", true);
                GameManager._instance.Reset();
                Time.timeScale = 1;
            }
            return;
        }
        weaponPos.transform.localScale = animPosition;
        GameManager._instance.check(change);
=======
>>>>>>> parent of b47d110b (마리오추가)
        timer += Time.deltaTime;
        anim.SetInteger("change", change); //�׽�Ʈ�� ���� �ﰢ���� ����
        StartCoroutine("CheckAnimationState");
        if (change != 0) //�ɷ� �ִ� ����
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //�ɷ� ���� �ִϸ��̼� + �� �߻�
                Instantiate(offParticles, gameObject.transform.position, Quaternion.identity);
                GameObject.FindWithTag("off").GetComponent<animScript>().playAnim("off");
                GameObject.FindWithTag("off").GetComponent<animScript>().waitAndDelete();
                anim.SetInteger("change", change);
                Instantiate(stars, gameObject.transform.position, Quaternion.identity);
            }

        }
        isRunning = Input.GetButton("Run"); //shift�� ������ �޸���

        if (isRunning && !onGround) isRunning = false; //shift �������� onground�� �ƴϸ� �ȴ޸�
        
        if (change == 0 && Input.GetKey(KeyCode.Q) && !isCoping && !anim.GetBool("isJumping")) // change�� 0�� ���¿��� Q������ ���� ���� , �������� �ƴ϶��
        {
            MyCollisions();
            anim.SetBool("isSplit", false);
            anim.SetBool("isSwallow", false);
            anim.SetBool("isStartInhale", true);
            if (EnemyAround && swallow)
            {
                GameObject.Find(target.name).GetComponent<EnemyControl>().isInhaled = true; // ����
                willChange = GameObject.Find(target.name).GetComponent<EnemyControl>().change;
                canInhaleSomething = true;
                //enemy.isInhaled = true;
            }
        }

        if (change == 0 && Input.GetKeyUp(KeyCode.Q) && !isCoping && !anim.GetBool("isJumping")) // change�� 0�� ���·� Q���� ���� ���� ���� �׸�
        {
            //if (EnemyAround) enemy.isInhaled = false;
            if (isStar) GameObject.FindWithTag("star").GetComponent<bouncStar>().isInhaled = false;
<<<<<<< HEAD

            if (anim.GetBool("isInhale") == false && !anim.GetBool("isStartInhale") && canInhaleSomething && target != null)
=======
            
            if (anim.GetBool("isInhale") == false && anim.GetBool("isStartInhale") && canInhaleSomething)
>>>>>>> parent of b47d110b (마리오추가)
            {
                GameObject.Find(target.name).GetComponent<EnemyControl>().isInhaled = false;
            }
            canInhaleSomething = false;
            anim.SetBool("isStartInhale", false);
            Instantiate(inhaleParticlePrefabs, gameObject.transform.position, Quaternion.identity);
            GameObject.FindWithTag("Particle").GetComponent<animScript>().playAnim("inhaleSomething");
            GameObject.FindWithTag("Particle").GetComponent<animScript>().waitAndDelete();
        }

        if (change == 1 && Input.GetKeyDown(KeyCode.Q)) // �ɷ��� ���� �ѹ� �ߵ��� �ð��� �ɸ��� �ɷ����� Q�� ������ ����
        {
            anim.SetBool("isAttack", true);
        }
        else if (change == 2 && Input.GetKey(KeyCode.Q)) // �ɷ��� �� ������ ��� ���ӵǴ� ����
        {
            anim.SetBool("isAttack", true);
        }
        else
        {
            anim.SetBool("isAttack", false);
        }

        if (!isCoping && !isAttack && Input.GetButtonDown("Jump") && !anim.GetBool("isStartInhale")) //�������� �ƴϰ�, �����ߵ� �ƴѵ� ����Ű�� ������ ����
        {
            anim.SetBool("onGround", false);
            if (!anim.GetBool("isJumping")) //����Ű ������, isJumping�� false�� ���� -> ���� ���� ����
            { 
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
                isFly = true;
            }
            else if (isFly && !anim.GetBool("isInhale")) //���� �̹� �ϰ��ְ� (���� �ȴ�Ұ�), isFlying�� �ƴѵ� ����Ű �ѹ� �� ��������, ����Ű������ �ƴϸ� ���Ⱑ��
            {
                rigid.AddForce(Vector2.up * flyPower, ForceMode2D.Impulse);
                anim.SetBool("isFlying", true);
                rigid.gravityScale = 0.2f;
            }
        }

        if (anim.GetBool("isFlying") && Input.GetKeyDown(KeyCode.W)) //���� ���߿� W�� ���⸦ ��´ٸ�
        {
            GameObject newObject = Instantiate(splitParticles, gameObject.transform.position, Quaternion.identity);
            newObject.transform.localScale = animPosition; //���� �����ֱ�
            GameObject.FindWithTag("SplitEffect").GetComponent<animScript>().playAnim("splitAir"); //���� ��� ����Ʈ
            GameObject.FindWithTag("SplitEffect").GetComponent<animScript>().waitAndDelete();
            anim.SetBool("isFlying", false);
            rigid.gravityScale = 1f;
            anim.SetBool("isQuitFly", true);
        }

        if (anim.GetBool("isInhale") && Input.GetKeyDown(KeyCode.W)) //�Կ� �ִ��߿� W�� ��´ٸ�
        {
            anim.SetBool("isSplit", true);
            anim.SetBool("isInhale", false);
            anim.SetBool("isStartInhale", false);
        }

        if (anim.GetBool("isInhale") && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))) //�Կ� �ִ� �߿� �Ʒ�Ű ������ ��Ŵ
        {
            anim.SetBool("isSwallow", true);
            anim.SetBool("isInhale", false);
            anim.SetBool("isStartInhale", false);
            if (willChange != 0) //���ɷ� ���� �ƴ��̻� ����
            {
<<<<<<< HEAD
                Debug.Log("����");
                square.SetActive(true);
                Time.timeScale = 0; //�����ϴ� ���� �Ͻ�����
                sound.playSound("Copy");
=======
>>>>>>> parent of b47d110b (마리오추가)
                Instantiate(copyParticles, gameObject.transform.position, Quaternion.identity);
                GameObject.FindWithTag("copy").GetComponent<animScript>().playAnim("copy");
                GameObject.FindWithTag("copy").GetComponent<animScript>().waitAndDelete();
                change = willChange;
                anim.SetInteger("change", willChange);
                Invoke("white1", 1f);
                willChange = 0;
            }

        }
        
        if (!isCoping && !isAttack) //������, �������� �ƴ� ��
        {
            if (Input.GetButtonUp("Horizontal"))//��ư���� �� ���� �� 
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }
            //���� ��ȯ
            if (Input.GetButton("Horizontal"))
            {
                //�⺻���� false
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                InhaleRange.x = 0.5f;
                animPosition = new Vector3(1, 1, 1);
                weaponPos.transform.localScale = animPosition;
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                InhaleRange.x = -0.5f;
                animPosition = new Vector3(-1, 1, 1);
                weaponPos.transform.localScale = animPosition;
            }
        }
        if(Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("isWalking") && !anim.GetBool("isJumping") && !anim.GetBool("isRunning")) //�ƹ��͵� ���ϴ� ���¿��� �Ʒ�Ű -> ��ũ����
        {
            anim.SetBool("isDown", true);
        }
        else
        {
            anim.SetBool("isDown", false);
        }

        //�ִϸ��̼�
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            //Ⱦ �̵� ���� ���� 0 (�� �����)
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }

    private void FixedUpdate()
    {
        if (!anim.GetBool("isStartInhale") && !isCoping && !isAttack) //���Ƶ��̴� ��, �����߿��� ���� �� ����
        {
            float h = Input.GetAxisRaw("Horizontal");
            if (isRunning && anim.GetBool("isWalking") && !anim.GetBool("isJumping") && !anim.GetBool("isFlying"))
            {
                rigid.AddForce(Vector2.right * h * runSpeed, ForceMode2D.Impulse);
                anim.SetBool("isRunning", true);
                //�ִ� �ӵ��̸�
                if (rigid.velocity.x > runSpeed)
                {
                    rigid.velocity = new Vector2(runSpeed, rigid.velocity.y);
                }
                else if (rigid.velocity.x < runSpeed * (-1))
                {
                    rigid.velocity = new Vector2(runSpeed * (-1), rigid.velocity.y);
                }
            }
            else
            {
                rigid.AddForce(Vector2.right * h * kirbySpeed, ForceMode2D.Impulse);
                anim.SetBool("isRunning", false);
                //�ִ� �ӵ��̸�
                if (rigid.velocity.x > maxSpeed)
                {
                    rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
                }
                else if (rigid.velocity.x < maxSpeed * (-1))
                {
                    rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
                }
            }
            

            if (rigid.velocity.y > jumpmaxSpeed)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpmaxSpeed);
            }
            else if (rigid.velocity.y < jumpmaxSpeed * (-1))
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpmaxSpeed * (-1));
            }

            if (rigid.velocity.y <= 0)
            {
                Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
                // ����,���� ����

                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("ground"));

                //����,����,�� ����,���̾�
                //raycasthit ���̾ �ش��ϴ� �ָ� �����ϰڴٴ°�
                //���� �¾Ҵ��� 
                if (rayHit.collider != null && rayHit.distance < 0.2f)
                {
                    onGround = true;
                    anim.SetBool("isJumping", false);
                    //�÷��̾��� ����ũ�⸸ŭ�̿��� �ٴڿ� ������ 
                    anim.SetBool("onGround", true);
                }
                else
                {
                    onGround = false;
                    anim.SetBool("onGround", false);
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // ���� --> ���� ���̸鼭 �÷��̾��� ��ġ�� enemy�� ��ġ���� ���� �� 
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else //����������
            {
                OnDamaged(collision.transform.position);//�浹������ x��,y�� �ѱ�
            }
        }
    }
    void OnAttack(Transform enemy)
    {
        //reaction force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // enemy die
        EnemyControl enemyMove = enemy.GetComponent<EnemyControl>();
        enemyMove.OnDamaged(); //enemy���忡�� ������ ������
    }

    //�����ð�
    void OnDamaged(Vector2 targetPos)
    {
        if (timer > waitingTime)
        {
            GameManager._instance.Damaged();
            anim.SetTrigger("doDamaged");
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);
            gameObject.layer = 9;
            spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
            Invoke("OffDamaged", NoDamageTime);
            Invoke("white1", 0.1f);
            timer = 0;
        }
    }

    void MyCollisions() //�� ����
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position + InhaleRange, size, 0, whatIsLayer);
        Collider2D hitStar = Physics2D.OverlapBox(transform.position + InhaleRange, size, 0, layerStar);
        Transform near_enemy = null;

        if (hitStar != null)
        {
            willChange = GameObject.FindWithTag("star").GetComponent<bouncStar>().change;
            GameObject.FindWithTag("star").GetComponent<bouncStar>().isInhaled = true;
            isStar = true;
        }
        else if (hitColliders.Length > 0)
        {
            isStar = false;
            float short_distance = Mathf.Infinity; 
            if (hitColliders.Length == 1)
            {
                near_enemy = hitColliders[0].transform;
            }
            else
            {
                foreach (Collider2D s_col in hitColliders)
                {
                    float player_toEnemy = Vector2.SqrMagnitude(gameObject.transform.position - s_col.transform.position); //�Ÿ��� �� ª�� ģ�� ã��
                    if (short_distance > player_toEnemy)
                    {
                        short_distance = player_toEnemy;
                        near_enemy = s_col.transform;
                    }
                }
            }
            EnemyAround = true;
            target = near_enemy;
            swallow = true;
            noFound = false;
        }
        else
        {
            isStar = false;
            EnemyAround = false;
            //target = null;
            swallow = false;
            noFound = true;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + InhaleRange, size);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MonsterWeapon"))
        {
            //������ ����
            anim.SetTrigger("doDamaged");
            OnDamaged(collision.transform.position);
        }
    }


    IEnumerator CheckAnimationState()
    {
        while (true) //�������̸� 
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Copy"))
            {
                isCoping = true;
            }
            else
            {
                isCoping = false;
            }
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

<<<<<<< HEAD
    private void activeUI()
    {
        for (int i =0; i < abilityIcon.Length; i++)
        {
            AbilitySpace.transform.Find(ability[i]).gameObject.SetActive(false);
            Icon.transform.Find(abilityIcon[i]).gameObject.SetActive(false);
        }
        AbilitySpace.transform.Find(ability[change]).gameObject.SetActive(true);
        Icon.transform.Find(abilityIcon[change]).gameObject.SetActive(true);
        int life = GameManager._instance.getCurrentLife();
        for (int i = 0; i < lifeNumberLast.Length; i++)
        {
            Number.transform.Find(lifeNumberLast[i]).gameObject.SetActive(false);
        }
        Number.transform.Find(lifeNumberFirst[0]).gameObject.SetActive(false);
        Number.transform.Find(lifeNumberFirst[1]).gameObject.SetActive(false);
        if (life < 10)
        {
            Number.transform.Find(lifeNumberFirst[0]).gameObject.SetActive(true);
            Number.transform.Find(lifeNumberLast[life]).gameObject.SetActive(true);
        }
        else if (life < 20)
        {
            Number.transform.Find(lifeNumberFirst[1]).gameObject.SetActive(true);
            Number.transform.Find(lifeNumberLast[life]).gameObject.SetActive(true);
        }
        
    }

    public void Die()
    {
        Time.timeScale = 0;
        gameObject.layer = 9;
        anim.SetTrigger("Die");
    }

    private void remove()
    {
        Destroy(gameObject);
    }

=======
>>>>>>> parent of b47d110b (마리오추가)
    #region
    void OffDamaged()
    {
        gameObject.layer = 8;
    }

    private void offPause()
    {
        Time.timeScale = 1;
    }

    void white1()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow2", 0.1f);
    }
    void yellow2()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white2", 0.1f);
    }
    void white2()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow3", 0.1f);
    }
    void yellow3()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white3", 0.1f);
    }
    void white3()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow4", 0.1f);
    }
    void yellow4()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white4", 0.1f);
    }
    void white4()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow5", 0.1f);
    }
    void yellow5()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white5", 0.1f);
    }
    void white5()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow6", 0.1f);
    }
    void yellow6()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white6", 0.1f);
    }
    void white6()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow7", 0.1f);
    }
    void yellow7()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white7", 0.1f);
    }
    void white7()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow8", 0.1f);
    }
    void yellow8()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white8", 0.1f);
    }
    void white8()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow9", 0.1f);
    }
    void yellow9()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white9", 0.1f);
    }
    void white9()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow10", 0.1f);
    }
    void yellow10()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white10", 0.1f);
    }
    void white10()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow11", 0.1f);
    }
    void yellow11()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white11", 0.1f);
    }
    void white11()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow12", 0.1f);
    }
    void yellow12()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white12", 0.1f);
    }
    void white12()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Invoke("yellow13", 0.1f);
    }
    void yellow13()
    {
        spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
        Invoke("white13", 0.1f);
    }
    void white13()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        //Invoke("yellow12", 0.1f);
    }
    #endregion
}