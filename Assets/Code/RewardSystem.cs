using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [Header("������Ʒ����ĸ��ʣ����ӣ�")]
    public int[] molecule;
    [Header("������Ʒ(ÿ��������Ʒ��ÿ������һһ��Ӧ)")]
    public GameObject[] rewardItem;
    [Header("������Ʒ����ĸ��ʣ���ĸ��")]
    public int denominator;

    public GameObject SetOneReward()
    {
        //���һ����
        System.Random rnd = new System.Random();
        int rand = rnd.Next(0, denominator+1);
        Debug.Log(rand);
        //��ӵõ�������
        int total = 0;
        for (int i = 0; i < molecule.Length; i++)
        {
            //����һ���жϣ�������������Ƿ�С������
            total = total + molecule[i];
            Debug.Log(total);
            if(rand <= total)
            {
                Debug.Log("Y");
                //���С����ִ�е������Ʒ
               return rewardItem[i];
            }
        }
        return null;
    }
}
