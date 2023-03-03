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

    //ǹе���ӵ�����ʱ����
    public void StartFlash()
    {
        if(isFlash == false)
        {
            isFlash = true;
            anim.SetTrigger("notFire");
        }
    }

    //�������ڶ������Ž�������ã���ֹ�ظ�����
    public void FlashOver()
    {
        isFlash = false;
    }
}
