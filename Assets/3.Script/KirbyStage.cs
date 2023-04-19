using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KirbyStage : MonoBehaviour
{
    public GameObject AbilitySpace;
    public GameObject Icon;
    public GameObject Number;
    private string[] ability = { "Normal", "Beam", "Spark", "Cutter", "Mario" };
    private string[] abilityIcon = { "Normal", "IconBeam", "IconSpark", "IconCutter", "IconMario" };
    private string[] lifeNumberFirst = { "first0", "first1" };
    private string[] lifeNumberLast = { "last0", "last1", "last2", "last3", "last4", "last5", "last6", "last7", "last8", "last9" };
    private int change = 0;
    private Vector3[] positions = { new Vector3(-1.17f, -0.484f, 0), new Vector3(0.058f, -0.696f, 0), new Vector3(1.197f, -0.215f, 0) };
    public int nowStage = 0;
    public int life;

    private void Start()
    {
        change = GameManager._instance.getCurrentCopy();
        activeUI();
        transform.position = positions[nowStage];
        life = GameManager._instance.getCurrentLife();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nowStage += 1;
            if (nowStage > 2) nowStage = 2;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nowStage -= 1;
            if (nowStage < 0) nowStage = 0;
        }
        setKirbyPosition();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startScene();
        }
    }
    private void activeUI()
    {
        for (int i = 0; i < abilityIcon.Length; i++)
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

    private void setKirbyPosition()
    {
        if (!GameManager._instance.stage1Clear) //아무 스테이지 안깸
        {
            nowStage = 0;
        }
        else if (!GameManager._instance.stage2Clear) //첫 스테이지만 깸 -> 1, 2번째 와리가리 가능
        {
            if (nowStage == 2) nowStage = 1;
        }

        if (nowStage == 0)
        {
            transform.position = positions[0];
        }
        else if (nowStage == 1)
        {
            transform.position = positions[1];
        }
        else if (nowStage == 2)
        {
            transform.position = positions[2];
        }
    }
    private void startScene()
    {
        setKirbyPosition(); //스테이지 조정후
        if (nowStage == 0)
        {
            SceneManager.LoadScene("World1-1");
        }
        else if (nowStage == 1)
        {
            SceneManager.LoadScene("World1-2");
        }
        else if (nowStage == 2)
        {
            SceneManager.LoadScene("World1-3");
        }
    }
}
