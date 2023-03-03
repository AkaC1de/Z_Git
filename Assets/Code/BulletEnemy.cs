using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [Header("�ӵ��ٶ�")]
    public float speed;
    [Header("�ӵ��˺�")]
    public float bulletDamage;
    [Header("���˾���(�ٶ�)")]
    public float backSpeed;
    [Header("��ը��ЧԤ����")]
    public GameObject explosionPrefab;
    [Header("�����")]
    public float shakeTime;
    public int pauseTime;
    public float strength;

    private Cinemachine.CinemachineImpulseSource impulse;
    new private Rigidbody2D rigidbody;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    public void SetSpeed(Vector2 direction)
    {
        rigidbody.velocity = direction * speed;
    }

    void Update()
    {

    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartAttackSense();
            // Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GameObject exp = MyObjectPool.Instance.GetObject(explosionPrefab);
            exp.transform.position = transform.position;
            MyObjectPool.Instance.PushObject(gameObject);
            if (transform.position.x > other.transform.position.x)
            {
                other.gameObject.transform.parent.GetComponent<PlayerLife>().GetHurtAttack(bulletDamage,Vector2.right,backSpeed);
            }
            else if (transform.position.x <= other.transform.position.x)
            {
                other.gameObject.transform.parent.GetComponent<PlayerLife>().GetHurtAttack(bulletDamage,Vector2.left,backSpeed);
            }
        }
    }
    public void StartAttackSense()
    {
        AttackSenseSystem.Instance.HitPause(pauseTime);
        //��ʹ��Cinemachineʱʹ�ø��ַ���
        AttackSenseSystem.Instance.CameraShake(shakeTime, strength);
        //ʹ��Cinemachineʱʹ�ø��ַ�������ҪΪAttackAera������������
        impulse.GenerateImpulse();
    }
}
