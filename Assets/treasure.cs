using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class treasure : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("따라라란따라라란~ 보물 씬 보여주기");
            GameManager._instance.stage3Clear = true;
            SceneManager.LoadScene("StageSelect");
        }
    }
}
