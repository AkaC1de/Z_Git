using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    [Header("����Ŀ���Tag")]
    public string targetTag;
    [Header("�������˺�")]
    public float attackDamage;
    [Header("���˵�ǿ��")]
    public float backSpeed;
    [Header("�����")]
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
        //�����ж�
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
        //��ʹ��Cinemachineʱʹ�ø��ַ���
        impulse.GenerateImpulse();
        AttackSenseSystem.Instance.CameraShake(shakeTime, strength);
        //ʹ��Cinemachineʱʹ�ø��ַ�������ҪΪAttackAera������������
        AttackSenseSystem.Instance.HitPause(pauseTime);
    }
}
