using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [Header("攻击目标的Tag")]
    public string targetTag;
    [Header("攻击的伤害")]
    public float attackDamage;
    [Header("击退的强度")]
    public float backSpeed;
    [Header("打击感")]
    public float shakeTime;
    public int pauseTime;
    public float strength;

    private Cinemachine.CinemachineImpulseSource impulse;

    private void Start()
    {
        impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //攻击判断
        if (other.CompareTag(targetTag))
        {
            if (targetTag == "Enemy")
            {
                StartAttackSense();

                if (transform.position.x < other.gameObject.transform.position.x)
                {
                    other.GetComponent<LifeSystem>().GetHurtAttack(attackDamage, Vector2.right, backSpeed);
                }
                else if (transform.position.x > other.gameObject.transform.position.x)
                {
                    other.GetComponent<LifeSystem>().GetHurtAttack(attackDamage, Vector2.left, backSpeed);
                }
            }
            if (targetTag == "Player")
            {
                StartAttackSense();

                if (transform.position.x < other.gameObject.transform.position.x)
                {
                    other.gameObject.transform.parent.GetComponent<LifeSystem>().GetHurtAttack(attackDamage, Vector2.right, backSpeed);
                }
                else if (transform.position.x > other.gameObject.transform.position.x)
                {
                    other.gameObject.transform.parent.GetComponent<LifeSystem>().GetHurtAttack(attackDamage, Vector2.left, backSpeed);
                }
            }
        }
    }

    public void StartAttackSense()
    {
        //不使用Cinemachine时使用该种方法
        impulse.GenerateImpulse();
        AttackSenseSystem.Instance.CameraShake(shakeTime, strength);
        //使用Cinemachine时使用该种方法（需要为AttackAera添加其它组件）
        AttackSenseSystem.Instance.HitPause(pauseTime);
    }
}
