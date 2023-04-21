using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KirbyControl : MonoBehaviour
{
    //파티클 생성 방법 - 태그로 Particle 붙이든가 새로 태그 만들기 (이름으로는 하면 안됨) -> 효과에 animScript 추가 , animator parameter에 트리거 추가
    //[SerializeField] private GameObject splitParticles;  선언후 드래그
    //Instantiate(splitParticles, gameObject.transform.position, Quaternion.identity);
    //GameObject.FindWithTag("Particle").GetComponent<animScript>().playAnim("트리거이름");
    //GameObject.FindWithTag("Particle").GetComponent<animScript>().waitAndDelete();
    //[SerializeField] GameObject square;
    private CircleCollider2D collider;
    private float carSpeed = 1.1f;

    private Vector3 destination;
    private bool isDest = false;
    private bool isDie = false;

    public GameObject Platform;

    public KirbySound sound; //효과음 담당 가져오기

    public GameObject[] enemies; //해당 스테이지 적들만 키게

    private bool isStar = false;
    private bool noFound = false;

    public int change;

    public GameObject AbilitySpace;
    public GameObject Icon;
    public GameObject Number;
    private string[] ability = { "Normal", "Beam", "Spark", "Cutter", "Mario", "Car" };
    private string[] abilityIcon = { "Normal", "IconBeam", "IconSpark", "IconCutter", "IconMario", "IconCar" };
    private string[] lifeNumberFirst = { "first0", "first1" };
    private string[] lifeNumberLast = { "last0", "last1", "last2", "last3", "last4", "last5", "last6", "last7", "last8", "last9" };
    private int willChange = 0;

    [SerializeField] private GameObject cutWeapon; //cutter 무기 프리팹
    
    [SerializeField] private float maxSpeed = 1.5f;
    [SerializeField] private float jumpmaxSpeed = 3f;
    [SerializeField] private float kirbySpeed = 2f;
    [SerializeField] private float runSpeed = 1.5f;
    [SerializeField] private float jumpPower;
    [SerializeField] private float flyPower;
    private Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer; //다른 스크립트에서 참조해야됨
    public Animator anim;

    private bool isFly = false;
    [SerializeField] private float NoDamageTime = 2f; //무적시간

    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask whatIsLayer; //layer에 있는애랑 부딪힌지 확인ㄴ용
    [SerializeField] private LayerMask layerStar; //별 먹는지 확인용
    private Vector3 InhaleRange = new Vector3(0.3f, 0, 0); //흡입 범위 지정

    public bool EnemyAround = false;
    public bool canInhaleSomething = false;
    private EnemyControl enemy;
    public Transform target; //흡입의 타겟
    private bool swallow = false; 
    public GameObject weaponPos; //무기 collider 방향 바꾸기위해서 씀

    [SerializeField] private GameObject fireWeapons; //마리오 불
    [SerializeField] private GameObject splitStars; //별 뱉기
    [SerializeField] private GameObject inhaleParticlePrefabs; //흡입 효과
    [SerializeField] private GameObject copyParticles; //변신 효과
    [SerializeField] private GameObject splitParticles; //뱉는 효과
    [SerializeField] private GameObject offParticles; //벗을때 효과
    [SerializeField] private GameObject stars; //벗을때 별

    private bool isRunning = false;
    public bool isCoping = false; //변신중이면 true
    private bool onGround = true;
    public bool isAttack = false;

    private Vector3 animPosition = new Vector3 (1, 1, 1);

    private float copyTimer;
    private float timer;
    private float cutterTimer;
    private int waitingTime;
    private float damageTimer;
    [SerializeField] private GameObject square;

    private void Awake()
    {
        TryGetComponent(out collider);
        sound = GetComponent<KirbySound>();
        AbilitySpace = GameObject.FindGameObjectWithTag("EditorOnly");
        Icon = GameObject.FindGameObjectWithTag("Finish");
        Number = GameObject.FindGameObjectWithTag("Respawn");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        TryGetComponent(out rigid);
        rigid.freezeRotation = true;
        TryGetComponent(out spriteRenderer);
        TryGetComponent(out anim);
        anim = GetComponent<Animator>();
        GameObject.FindGameObjectWithTag("Enemy").TryGetComponent(out enemy);
        timer = 0.0f;
        damageTimer = 3.0f;
        cutterTimer = 3.0f;
        waitingTime = 3;
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

    }
    private void Update()
    {
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
        timer += Time.deltaTime;
        cutterTimer += Time.deltaTime;
        damageTimer += Time.deltaTime;
        anim.SetInteger("change", change); //테스트를 위한 즉각적인 변신
        activeUI();
        StartCoroutine("CheckAnimationState");
        if (change == 5) //자동차로 변신한다면 걷는 + 달리는 속도 더 빠르게
        {
            carSpeed = 1.5f;
        }
        else
        {
            carSpeed = 1f;
        }
        if (rigid.velocity.y > 0)
        {
            Platform.GetComponent<platform>().IgnoreLayerTrue();
        }
        else
        {
            Platform.GetComponent<platform>().IgnoreLayerFalse();
        }
        if (change != 0 && !isCoping && Input.GetKeyDown(KeyCode.R)) //능력 있는 상태 && 변신중이 아님 && R키를 누름
        {
            //능력 벗는 애니메이션 + 별 발사
            sound.playSound("Off");
            Instantiate(offParticles, gameObject.transform.position, Quaternion.identity);
            GameObject.FindWithTag("off").GetComponent<animScript>().playAnim("off");
            GameObject.FindWithTag("off").GetComponent<animScript>().waitAndDelete();
            anim.SetInteger("change", change);
            activeUI();
            Instantiate(stars, gameObject.transform.position, Quaternion.identity);
        }
        isRunning = Input.GetButton("Run"); //shift가 눌리면 달리기

        if (isRunning && !onGround) isRunning = false; //shift 눌렀지만 onground가 아니면 안달림

        if (change == 0 && Input.GetKeyDown(KeyCode.Q) && !isCoping && !anim.GetBool("isJumping") && !anim.GetBool("isInhale") && !anim.GetBool("isRunning"))
        {
            sound.playSound("Inhale"); //흡입 효과음 시작
            MyCollisions();
            anim.SetBool("isSplit", false);
            anim.SetBool("isSwallow", false);
            anim.SetBool("isStartInhale", true);
            if (EnemyAround && swallow)
            {
                Debug.Log(target.name);
                if (GameObject.Find(target.name).GetComponent<EnemyControl>().type != 10)
                {
                    GameObject.Find(target.name).GetComponent<EnemyControl>().anim.SetBool("isBeingInhaled", true);
                }
                GameObject.Find(target.name).GetComponent<EnemyControl>().isInhaled = true; // 흡입
                GameObject.Find(target.name).GetComponent<EnemyControl>().BeingInhaled();
                willChange = GameObject.Find(target.name).GetComponent<EnemyControl>().change;
                canInhaleSomething = true;
            }
        }
        if (change == 0 && Input.GetKeyUp(KeyCode.Q) && !isCoping && !anim.GetBool("isJumping") && !anim.GetBool("isInhale") && !anim.GetBool("isRunning")) // change가 0인 상태로 Q에서 손을 떼면 흡입 그만
        {
            anim.SetBool("isStartInhale", false);
            sound.stopSound(); //흡입 효과음 그만
            //if (EnemyAround) enemy.isInhaled = false;
            if (isStar) GameObject.FindWithTag("star").GetComponent<bouncStar>().isInhaled = false;

            if (anim.GetBool("isInhale") == false && !anim.GetBool("isStartInhale") && canInhaleSomething && target != null)
            {
                GameObject.Find(target.name).GetComponent<EnemyControl>().isInhaled = false;
                GameObject.Find(target.name).GetComponent<EnemyControl>().anim.SetBool("isBeingInhaled", false);
            }
            canInhaleSomething = false;
            Instantiate(inhaleParticlePrefabs, gameObject.transform.position, Quaternion.identity);
            GameObject.FindWithTag("Particle").GetComponent<animScript>().playAnim("inhaleSomething");
            GameObject.FindWithTag("Particle").GetComponent<animScript>().waitAndDelete();
        }
        else if (change == 0 && Input.GetKeyUp(KeyCode.Q) && !isCoping && !anim.GetBool("isJumping") && anim.GetBool("isInhale"))
            {
                anim.SetBool("isStartInhale", false);
            }

        if (change == 1 && Input.GetKeyDown(KeyCode.Q)) // 능력이 공격 한번 발동시 시간이 걸리는 능력으로 Q를 누르면 공격
        {
            sound.playSound("Beam");
            anim.SetBool("isAttack", true);
        }
        else if (change == 2 && Input.GetKey(KeyCode.Q)) // 능력이 꾹 누르면 계속 지속되는 공격
        {
            anim.SetBool("isAttack", true);
        }
        else if (change == 3 && Input.GetKeyDown(KeyCode.Q)) // 능력이 커터라면 instantiate로 무기 생성 
        { 
            if (timer > waitingTime * 0.5f)
            {
                Instantiate(cutWeapon, transform.position, Quaternion.identity);
                timer = 0.0f;
                anim.SetBool("isAttack", true);
            }
        }
        else if (change == 3 && Input.GetKey(KeyCode.Q))
        {
            if (timer > waitingTime * 0.5f)
            {
                anim.SetBool("isAttack", true);
            }
        }
        else if (change == 4 && Input.GetKeyDown(KeyCode.Q))
        {
            if (timer > waitingTime * 0.2f) 
            {
                Instantiate(fireWeapons, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                timer = 0f;
                anim.SetBool("isAttack", true);
            }
            
        }
        else 
        {
            anim.SetBool("isAttack", false);
        }

        if (!isCoping && !isAttack && Input.GetButtonDown("Jump") && !anim.GetBool("isStartInhale") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("split") && (change != 5)) //변신중이 아니고, 흡입중도 아닌데 점프키를 누르면 실행 + 자동차면 점프 불가
        {
            sound.playSound("Jump");    
            anim.SetBool("onGround", false);
            if (!anim.GetBool("isJumping")) //점프키 누르고, isJumping이 false면 실행 -> 이중 점프 막기
            { 
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
                isFly = true;
            }
            else if (isFly && !anim.GetBool("isInhale")) //점프 이미 하고있고 (땅에 안닿았고), isFlying이 아닌데 점프키 한번 더 눌렀으면, 뭐삼키는중이 아니면 날기가능
            {
                rigid.AddForce(Vector2.up * flyPower, ForceMode2D.Impulse);
                anim.SetBool("isFlying", true);
                rigid.gravityScale = 0.2f;
            }
        }

        if (anim.GetBool("isFlying") && Input.GetKeyDown(KeyCode.W)) //나는 도중에 W로 공기를 뱉는다면
        {
            GameObject newObject = Instantiate(splitParticles, gameObject.transform.position, Quaternion.identity);
            newObject.transform.localScale = animPosition; //방향 맞춰주기
            GameObject.FindWithTag("SplitEffect").GetComponent<animScript>().playAnim("splitAir"); //공기 뱉는 이펙트
            GameObject.FindWithTag("SplitEffect").GetComponent<animScript>().waitAndDelete();
            anim.SetBool("isFlying", false);
            rigid.gravityScale = 1f;
            anim.SetBool("isQuitFly", true);
        }

        if (anim.GetBool("isInhale") && Input.GetKeyDown(KeyCode.W)) //입에 있는중에 W로 뱉는다면
        {
            GameObject newObject = Instantiate(splitStars, gameObject.transform.position, Quaternion.identity);
            newObject.transform.localScale = animPosition;
            anim.SetBool("isSplit", true);
            anim.SetBool("isInhale", false);
            anim.SetBool("isStartInhale", false);
        }

        if (!anim.GetBool("isJumping") && anim.GetBool("isInhale") && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))) //뛰는중이 아니고 입에 있는 중에 아래키 누르면 삼킴
        {
            anim.SetBool("isSwallow", true);
            anim.SetBool("isInhale", false);
            anim.SetBool("isStartInhale", false);
            if (willChange != 0) //무능력 적이 아닌이상 변신
            {
                Debug.Log("변신");
                square.SetActive(true);
                Time.timeScale = 0; //변신하는 동안 일시정지
                sound.playSound("Copy");
                Instantiate(copyParticles, gameObject.transform.position, Quaternion.identity);
                GameObject.FindWithTag("copy").GetComponent<animScript>().playAnim("copy");
                GameObject.FindWithTag("copy").GetComponent<animScript>().waitAndDelete();
                change = willChange;
                anim.SetInteger("change", willChange);
                activeUI();
                Invoke("white1", 1f);
                willChange = 0;
            }

        }
        
        if (!isCoping && !isAttack) //변신중, 공격중이 아닐 때
        {
            if (Input.GetButtonUp("Horizontal"))//버튼에서 손 뗏을 때 
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }
            //방향 전환
            if (Input.GetButton("Horizontal"))
            {
                //기본값은 false
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
        if(Input.GetKey(KeyCode.DownArrow) && !anim.GetBool("isWalking") && !anim.GetBool("isJumping") && !anim.GetBool("isRunning")) //아무것도 안하는 상태에서 아래키 -> 웅크리기
        {
            anim.SetBool("isDown", true);
            Platform.GetComponent<platform>().startCo();
        }
        else
        {
            anim.SetBool("isDown", false);
        }

        //애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.1)
        {
            //횡 이동 단위 값이 0 (즉 멈춘거)
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }

    private void FixedUpdate()
    {
        if (!anim.GetBool("isStartInhale") && !isCoping && !isAttack) //빨아들이는 중, 변신중에는 걸을 수 없음
        {
            float h = Input.GetAxisRaw("Horizontal");
            if (isRunning && anim.GetBool("isWalking") && !anim.GetBool("isJumping") && !anim.GetBool("isFlying") )
            {
                if (change == 5)
                {
                    collider.isTrigger = true;
                    gameObject.layer = 19; //무적 레이어로 바꿈
                }
                rigid.AddForce(Vector2.right * h * runSpeed * carSpeed, ForceMode2D.Impulse);
                anim.SetBool("isRunning", true);
                //최대 속도이면
                if (rigid.velocity.x > runSpeed)
                {
                    rigid.velocity = new Vector2(runSpeed * carSpeed, rigid.velocity.y);
                }
                else if (rigid.velocity.x < runSpeed * (-1))
                {
                    rigid.velocity = new Vector2(runSpeed * (-1) * carSpeed, rigid.velocity.y);
                }
            }
            else
            {
                if (change == 5)
                {
                    collider.isTrigger = false;
                    gameObject.layer = 8;
                }
                rigid.AddForce(Vector2.right * h * kirbySpeed, ForceMode2D.Impulse);
                anim.SetBool("isRunning", false);
                //최대 속도이면
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
                // 시작,방향 색깔

                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("ground"));
                RaycastHit2D platHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("platform"));
                RaycastHit2D enemHit = Physics2D.Raycast(rigid.position, Vector3.down, 0.1f, LayerMask.GetMask("Enemy"));

                //시작,방향,빔 길이,레이어
                //raycasthit 레이어에 해당하는 애만 구별하겠다는거
                //빔에 맞았는지 
                if ((rayHit.collider != null && rayHit.distance < 0.2f) || (platHit.collider != null && platHit.distance < 0.2f) || (enemHit.collider != null))
                {
                    onGround = true;
                    anim.SetBool("isJumping", false);
                    //플레이어의 절반크기만큼이여야 바닥에 닿은거 
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
            // 공격 --> 낙하 중이면서 플레이어의 위치가 enemy의 위치보다 높을 때 
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y && change == 4) //마리오상태이면 위에서 밟는게 가능
            {
                OnAttack(collision.transform);
            }
            else //데미지입음
            {
                if (damageTimer > 3.0f && gameObject.layer != 19)
                {
                    Debug.Log("내가 부딪힘");
                    OnDamaged(collision.transform.position);//충돌했을때 x축,y축 넘김
                }
            }
        }
    }
    void OnAttack(Transform enemy)
    {
        //reaction force
        //rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // enemy die
        EnemyControl enemyMove = enemy.GetComponent<EnemyControl>();
        enemyMove.OnDamaged(); //enemy입장에서 데미지 입은거
    }

    //무적시간
    public void OnDamaged(Vector2 targetPos) //다침
    {
        if (damageTimer > 3.0f && gameObject.layer != 19)
        {
            //능력 벗는 애니메이션 + 별 발사a
            if (change != 0)
            {
                Instantiate(offParticles, gameObject.transform.position, Quaternion.identity);
                GameObject.FindWithTag("off").GetComponent<animScript>().playAnim("off");
                GameObject.FindWithTag("off").GetComponent<animScript>().waitAndDelete();
                change = 0;
                Instantiate(stars, gameObject.transform.position, Quaternion.identity);
            }
            anim.SetInteger("change", change);
            activeUI();

            GameManager._instance.Damaged();
            anim.SetTrigger("doDamaged");
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);
            gameObject.layer = 9;
            spriteRenderer.color = new Color(1, 0.92f, 0.016f, 1);
            Invoke("OffDamaged", NoDamageTime);
            Invoke("white1", 0.1f);
            timer = 0;
            damageTimer = 0f;
        }
    }

    void MyCollisions() //적 흡입
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
                    float player_toEnemy = Vector2.SqrMagnitude(gameObject.transform.position - s_col.transform.position); //거리가 더 짧은 친구 찾기
                    if (short_distance > player_toEnemy)
                    {
                        short_distance = player_toEnemy;
                        near_enemy = s_col.transform;
                    }
                }
            }
            Debug.Log("가까운 적 : " + near_enemy);
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && gameObject.layer == LayerMask.NameToLayer("Strong"))
        {
            OnAttack(collision.transform);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("MonsterWeapon"))
        {
            if (damageTimer > 1.0f)
            {
                //데미지 입음
                anim.SetTrigger("doDamaged");
                OnDamaged(collision.transform.position);
                damageTimer = 0;
            }
            
        }
        if (collision.gameObject.name == "Water")
        {
            anim.SetBool("inWater", true);
            rigid.gravityScale = 0.5f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Water")
        {
            anim.SetBool("inWater", false);
            rigid.gravityScale = 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("door"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                collision.GetComponent<makeWarp>().doWarp();
            }
        }
        if (collision.gameObject.CompareTag("clearDoor") && Input.GetKeyDown(KeyCode.UpArrow))
        {
            collision.GetComponent<stageClear>().clear();
        }
    }

    IEnumerator CheckAnimationState()
    {
        while (true) //변신중이면 
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
