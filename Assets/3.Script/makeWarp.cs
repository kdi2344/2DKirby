using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeWarp : MonoBehaviour
{
    [SerializeField] private Vector3 destination;
    [SerializeField] private Vector2 newMin;
    [SerializeField] private Vector2 newMax;
    [SerializeField] private int stage;
    private GameObject kirby;
    private Transform kirbyTransform;
    private GameObject[] enemies;

    private void Start()
    {
        enemies = kirby.GetComponent<KirbyControl>().enemies;
    }
    private void Awake()
    {
        kirby = GameObject.Find("Kirby");
    }

    public void doWarp()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].GetComponent<EnemyControl>().stage == stage) //�ش� ���������� ���鸸 Ű��
            {
                enemies[i].GetComponent<EnemyControl>().Respawn();
                enemies[i].SetActive(true);
                Debug.Log(enemies[i].name + "Respawn��");
            }
            else { enemies[i].SetActive(false);  }
        }
        kirby.GetComponent<KirbySound>().playSound("Door");
        kirbyTransform = kirby.GetComponent<Transform>();
        kirbyTransform.position = destination;
        Camera.main.GetComponent<MainCameraController>().minCameraBoundary = newMin;
        Camera.main.GetComponent<MainCameraController>().maxCameraBoundary = newMax;
    }

}
