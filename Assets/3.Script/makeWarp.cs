using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class makeWarp : MonoBehaviour
{
    public GameObject Loading; //투명 네모 -> 검정으로
    [SerializeField] private Vector3 destination;
    [SerializeField] private Vector2 newMin;
    [SerializeField] private Vector2 newMax;
    [SerializeField] private int stage;
    private GameObject kirby;
    private Transform kirbyTransform;
    private GameObject[] enemies;
    private bool once = false;
    public bool waiting = false;
    private bool active = true;

    private void Start()
    {
        enemies = kirby.GetComponent<KirbyControl>().enemies;
    }
    private void Awake()
    {
        kirby = GameObject.Find("Kirby");
    }
    private void Update()
    {
        if (once && Loading.GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 1))
        {
            StopCoroutine("FadeinCoroutine");
            Invoke("clear", 0.25f);
        }
    }

    public void doWarp()
    {

        if (active && SceneManager.GetActiveScene().name == "World1-3")
        {
            active = false;
            gameObject.GetComponent<bossBGM>().playBoss();
        }
            waiting = true;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<EnemyControl>().stage == stage) //해당 스테이지의 적들만 키기
            {
                enemies[i].GetComponent<EnemyControl>().Respawn();
                enemies[i].SetActive(true);
                Debug.Log(enemies[i].name + "Respawn됨");
            }
            else { enemies[i].SetActive(false);  }
        }
        Loading.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        Loading.SetActive(true);
        StartCoroutine("FadeinCoroutine");
        kirby.GetComponent<KirbySound>().playSound("Door");
        Invoke("kirbyMove", 1f);
        
    }

    IEnumerator FadeinCoroutine()
    {
        once = true;
        float fadeCount = 0;
        while (fadeCount <= 1)
        {
            fadeCount += 0.05f;
            yield return new WaitForSeconds(0.05f);
            Loading.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, fadeCount);
            
        }
    }

    private void kirbyMove()
    {
        kirbyTransform = kirby.GetComponent<Transform>();
        kirbyTransform.position = destination;
        Camera.main.GetComponent<MainCameraController>().minCameraBoundary = newMin;
        Camera.main.GetComponent<MainCameraController>().maxCameraBoundary = newMax;
    }

    private void clear()
    {
        Loading.SetActive(false);
        Loading.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        once = false;
    }
}
