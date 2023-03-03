using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("�ӵ��ٶ�")]
    public float speed;
    [Header("�ӵ��˺�")]
    public float bulletDamage;
    [Header("����ǿ��")]
    public float backSpeed;
    [Header("��ը��ЧԤ����")]
    public GameObject explosionPrefab;

    new private Rigidbody2D rigidbody;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
        if (other.CompareTag("Enemy"))
        {
            // Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GameObject exp = MyObjectPool.Instance.GetObject(explosionPrefab);
            exp.transform.position = transform.position;
            MyObjectPool.Instance.PushObject(gameObject);
            if (transform.position.x > other.transform.position.x)
            {
                other.GetComponent<EnemyLife>().GetHurtAttack(bulletDamage,Vector2.left,backSpeed);
            }
            else if(transform.position.x <= other.transform.position.x)
            {
                other.GetComponent<EnemyLife>().GetHurtAttack(bulletDamage, Vector2.right, backSpeed);
            }
            // Destroy(gameObject);
            
        }
        if (other.CompareTag("Ground"))
        {
            GameObject exp = MyObjectPool.Instance.GetObject(explosionPrefab);
            exp.transform.position = transform.position;
            MyObjectPool.Instance.PushObject(gameObject);
        }
    }
}
