using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : LifeSystem
{
    [Header("血量显示UI")]
    public Text bloodNum;
    [Header("拾取血包后增加的血量")]
    public float addBloodNum;
    [Header("跌落伤害")]
    public float dropDamage;
    [Header("跌落区域")]
    private GameObject areaPoint;


    protected override void Update()
    {
        //血量显示UI
        bloodNum.text = blood.ToString() + " / 10";
        //血量条UI
        BloodBar.blood = blood;
        BloodBar.maxBlood = maxBlood;
        base.Update();
    }

    //拾取血包
    public void AddBlood()
    {
        blood = blood + addBloodNum;
    }

    //判断是否在某个地方跌落，若跌落则返回到该跌落点对应的位置,并减少血量
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //区域
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
