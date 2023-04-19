using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animScript : MonoBehaviour
{
    public Animator anim;

    public void playAnim(string name)
    {
        anim.SetTrigger(name);
    }
    public void waitAndDelete()
    {
        Invoke("delete", 0.5f);
    }
    public void delete()
    {
        Destroy(gameObject);
    }
}
