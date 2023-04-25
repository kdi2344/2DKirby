using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;

    public int MaxHP = 10;
    public float currentHP = 10;
    public GameObject sl_ob;
    public Slider slHP;
    public int currentKirby = 0;
    public int life = 3;

    public bool stage1Clear = false;
    public bool stage2Clear = false;
    public bool stage3Clear = false;

    void Start()
    {
        slHP = sl_ob.GetComponent<Slider>();
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void Damaged()
    {
        currentHP -= 1;
    }

    void Update()
    {
        slHP.value = currentHP / MaxHP;
        if (currentHP > MaxHP)
        {
            currentHP = MaxHP;
        }
        if (life < 1)
        {
            SceneManager.LoadScene("GameOver");
            Destroy(gameObject);
        }
    }
    public void check(int num)
    {
        currentKirby = num;
    }

    public int getCurrentCopy()
    {
        return currentKirby;
    }
    public int getCurrentLife()
    {
        return life;
    }
    public float getCurrentHP()
    {
        return currentHP;
    }
    public void die()
    {
        life -= 1;
    }
    public void Reset()
    {
        currentHP = MaxHP;
    }
}
