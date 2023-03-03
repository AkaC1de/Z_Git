using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [Header("奖励物品掉落的概率（分子）")]
    public int[] molecule;
    [Header("奖励物品(每个奖励物品与每个分子一一对应)")]
    public GameObject[] rewardItem;
    [Header("奖励物品掉落的概率（分母）")]
    public int denominator;

    public GameObject SetOneReward()
    {
        //随机一个数
        System.Random rnd = new System.Random();
        int rand = rnd.Next(0, denominator+1);
        Debug.Log(rand);
        //相加得到的总数
        int total = 0;
        for (int i = 0; i < molecule.Length; i++)
        {
            //进行一次判断，看随机到的数是否小于总数
            total = total + molecule[i];
            Debug.Log(total);
            if(rand <= total)
            {
                Debug.Log("Y");
                //如果小于则执行掉落该物品
               return rewardItem[i];
            }
        }
        return null;
    }
}
