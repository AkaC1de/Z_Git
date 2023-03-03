using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : LifeSystem
{
    [Header("Ѫ����ʾUI")]
    public Text bloodNum;
    [Header("ʰȡѪ�������ӵ�Ѫ��")]
    public float addBloodNum;
    [Header("�����˺�")]
    public float dropDamage;
    [Header("��������")]
    private GameObject areaPoint;


    protected override void Update()
    {
        //Ѫ����ʾUI
        bloodNum.text = blood.ToString() + " / 10";
        //Ѫ����UI
        BloodBar.blood = blood;
        BloodBar.maxBlood = maxBlood;
        base.Update();
    }

    //ʰȡѪ��
    public void AddBlood()
    {
        blood = blood + addBloodNum;
    }

    //�ж��Ƿ���ĳ���ط����䣬�������򷵻ص��õ�����Ӧ��λ��,������Ѫ��
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //����
        if(collision.gameObject.name == "DeadLine1")
        {
            DropDead();
        }
        if (collision.gameObject.name == "DeadLine2")
        {
            DropDead();
        }
        if (collision.gameObject.name == "DeadLine3")
        {
            DropDead();
        }
        if (collision.gameObject.name == "DeadLine4")
        {
            DropDead();
        }
        if (collision.gameObject.name == "DeadLine5")
        {
            DropDead();
        }

        void DropDead() 
        {
            areaPoint = collision.gameObject.transform.GetChild(0).gameObject;
            transform.position = areaPoint.transform.position;
            blood = blood - dropDamage;
            hurtSound.HandleEvent(gameObject);
        }
    }
}
