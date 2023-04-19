using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stageClear : MonoBehaviour
{
    [SerializeField] int stage = 1;
    public void clear()
    {
        if (stage == 1)
        {
            GameManager._instance.stage1Clear = true;
        }
        else if (stage == 2)
        {
            GameManager._instance.stage2Clear = true;
        }
        else if (stage == 3)
        {
            GameManager._instance.stage3Clear = true;
        }
        SceneManager.LoadScene("StageSelect");
    }
}
