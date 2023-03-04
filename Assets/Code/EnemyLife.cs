using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using System;

public class EnemyLife : LifeSystem
{
    [Header("获取行为树组件")]
    public BehaviorTree enemyTree;

    protected override void Update()
    {
        base.Update();
    }


    protected override void BeatBack()
    {
        if (isHurting == true)
        {
            //将受伤状态传递给行为树
            enemyTree.SetVariableValue("isHurting", true);
            //击退
            rb.AddForce(backSpeed * backDirection, ForceMode2D.Impulse);
            if (info.normalizedTime >= 0.6f)
            {
                isHurting = false;
            }
        }
       
    }

    public override void isDeath()
    {
        GameObject reward = gameObject.GetComponent<EnemyReward>().SetOneReward();
        if (reward != null)
        {
            Instantiate(reward,gameObject.transform.position,Quaternion.identity);
        }
        
       //死亡销毁物体
        Destroy(gameObject);
    }
}
