using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage1 : MonoBehaviour
{
    private GameObject stage1image;
    private GameObject stage1star;
    private GameObject stage1num;
    [SerializeField] private Sprite stage1clearimage;

    private void Awake()
    {
        stage1image = transform.GetChild(0).gameObject;
        stage1star = transform.GetChild(1).gameObject;
        stage1num = transform.GetChild(2).gameObject;
    }
    private void Start()
    {
        if (GameManager._instance.stage1Clear)
        {
            stage1image.GetComponent<SpriteRenderer>().sprite = stage1clearimage;

        }
    }
    private void Update()
    {
        
    }
}
