using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFlash : MonoBehaviour
{
    private Animator anim;
    private bool isFlash;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //枪械在子弹不足时调用
    public void StartFlash()
    {
        if(isFlash == false)
        {
            isFlash = true;
            anim.SetTrigger("notFire");
        }
    }

    //动画机在动画播放结束后调用，防止重复播放
    public void FlashOver()
    {
        isFlash = false;
    }
}
