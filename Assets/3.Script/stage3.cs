using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage3 : MonoBehaviour
{
    private GameObject stage3image;
    private GameObject stage3star;
    private GameObject stage3num;
    [SerializeField] private Sprite stage3clearimage;

    private void Awake()
    {
        stage3image = transform.GetChild(0).gameObject;
        stage3star = transform.GetChild(1).gameObject;
        stage3num = transform.GetChild(2).gameObject;
    }
    private void Start()
    {
        if (!GameManager._instance.stage1Clear)
        {
            stage3image.SetActive(false);
            stage3star.SetActive(false);
            stage3num.SetActive(false);
        }
        if (!GameManager._instance.stage2Clear)
        {
            stage3image.SetActive(false);
            stage3star.SetActive(false);
            stage3num.SetActive(false);
        }
        else
        {
            stage3image.SetActive(true);
            stage3star.SetActive(true);
            stage3num.SetActive(true);
        }
        if (GameManager._instance.stage3Clear)
        {
            stage3image.GetComponent<SpriteRenderer>().sprite = stage3clearimage;
        }
    }
    private void Update()
    {
        
    }
}
