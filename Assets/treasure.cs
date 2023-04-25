using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class treasure : MonoBehaviour
{
    private BoxCollider2D collid;
    public GameObject Loading;
    private bool alreadyStart = false;
    private AudioSource audioSource;

    private void Awake()
    {
        TryGetComponent(out audioSource);
        TryGetComponent(out collid);
        Loading = Camera.main.transform.Find("Loading").gameObject;
    }

    private void Update()
    {
        if (alreadyStart && Loading.GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 1))
        {
            StopCoroutine("FadeinCoroutine");
            SceneManager.LoadScene("Outro");
            GameManager._instance.stage3Clear = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gameObject.layer = 14;
            StartCoroutine("FadeinCoroutine");
            GameManager._instance.stage3Clear = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            audioSource.Play();
            Loading.SetActive(true);
            gameObject.layer = 14;
            StartCoroutine("FadeinCoroutine");
            GameManager._instance.stage3Clear = true;
        }
    }

    IEnumerator FadeinCoroutine()
    {
        float fadeCount = 0;
        while (fadeCount <= 1)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            if (fadeCount > 0.8f) alreadyStart = true;
            Loading.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, fadeCount);
        }
    }
}
