using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using System;

public class EnemyLife : LifeSystem
{
    [Header("��ȡ��Ϊ�����")]
    public BehaviorTree enemyTree;

    protected override void Update()
    {
        base.Update();
        BT_Hurt();
    }


    protected void BT_Hurt()
    {
        if (isHurting == true)
        {
            //������״̬���ݸ���Ϊ��
            enemyTree.SetVariableValue("isHurting", true);
        }
        if (info.normalizedTime >= 0.6f)
        {
            isHurting = false;
        }
    }

    public override void isDeath()
    {
        GameObject reward = gameObject.GetComponent<EnemyReward>().SetOneReward();
        if (reward != null)
        {
            Instantiate(reward,gameObject.transform.position,Quaternion.identity);
        }
        
       //������������
        Destroy(gameObject);
    }
}
