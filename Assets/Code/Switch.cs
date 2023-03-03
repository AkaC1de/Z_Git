using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    private bool inSwitch;
    private Animator anim; 

    [Header("要控制的物体")]
    public GameObject[] Item;
    [Header("控制的物体初始状态是开还是关")]
    public bool ifSwitch;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UseSwitch();
        ChangeAnim();
    }

    void UseSwitch()
    {
        if(ifSwitch == false && inSwitch == true && Input.GetKeyDown(KeyCode.E)) 
        {
            ifSwitch = true;
            foreach (var item in Item)
            {
                item.SetActive(true);
            }
        }
        else if(ifSwitch == true && inSwitch == true && Input.GetKeyDown(KeyCode.E))
        {
            ifSwitch = false;
            foreach (var item in Item)
            {
                item.SetActive(false);
            }
        }
    }

    void ChangeAnim()
    {
        if(ifSwitch == false)
        {
            anim.SetBool("switch", false);
        }
        if (ifSwitch == true)
        {
            anim.SetBool("switch", true);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            inSwitch = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            inSwitch = false;
        }
    }
}
