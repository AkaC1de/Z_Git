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
    }


    protected override void BeatBack()
    {
        if (isHurting == true)
        {
            //������״̬���ݸ���Ϊ��
            enemyTree.SetVariableValue("isHurting", true);
            //����
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
        
       //������������
        Destroy(gameObject);
    }
}
