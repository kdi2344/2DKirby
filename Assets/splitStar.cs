using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splitStar : MonoBehaviour
{
    private Vector3 position;
    private GameObject kirby;
    private float speed = 2f;

    void Start()
    {
        kirby = GameObject.Find("Kirby");
        position = transform.position;
        if (kirby.GetComponent<KirbyControl>().spriteRenderer.flipX == true)
        {
            speed = -2f;
        }
        else
        {
            speed = 2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        position.x += speed * Time.deltaTime;
        transform.position = position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("enemy¿Í trigger Ãæµ¹");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Layout"))
        {
            Destroy(gameObject);
        }
    }
}
