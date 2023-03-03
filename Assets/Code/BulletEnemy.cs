using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [Header("子弹速度")]
    public float speed;
    [Header("子弹伤害")]
    public float bulletDamage;
    [Header("击退距离(速度)")]
    public float backSpeed;
    [Header("爆炸特效预制体")]
    public GameObject explosionPrefab;
    [Header("打击感")]
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
        //不使用Cinemachine时使用该种方法
        AttackSenseSystem.Instance.CameraShake(shakeTime, strength);
        //使用Cinemachine时使用该种方法（需要为AttackAera添加其它组件）
        impulse.GenerateImpulse();
    }
}
