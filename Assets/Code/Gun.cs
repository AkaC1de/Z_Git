using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("�������ʱ��")]
    public float interval;
    [Header("�ӵ����ƫ�ƽǶ�")]
    public float bulletAngel;
    [Header("ǹе��������Ϸ�е�λ��")]
    public Transform gunPoint;
    [Header("�ӵ���Ԥ����")]
    public GameObject bulletPrefab;
    [Header("���ǵ�Ԥ����")]
    public GameObject shellPrefab;
    [Header("�ӵ����ɵ�λ��")]
    public Transform bulletPos;
    [Header("�������ɵ�λ��")]
    public Transform bulletShellPos;
    [Header("��ʼ�ص���")]
    public int bulletNum;
    [Header("ʰȡ��ҩ�������ӵĵ�ҩ��")]
    public int addBulletNum;
    [Header("UI������ʾ���ӵ�����")]
    public Text bulletNumText;
    [Header("�ӵ�����ʱ������˸��UI")]
    public GameObject flashText;
    [Header("��Ч")]
    public AkEvent fireSound;

    protected Vector2 mousePos;
    protected Vector2 direction;
    protected float timer;
    protected float flipY;
    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        flipY = transform.localScale.y;
    }

    protected virtual void Update()
    {
        //���ӵ�������ΪUI��ʾ���ַ���
        bulletNumText.text = bulletNum.ToString();
        //�ӵ�����С�ڵ���0ʱ����UI��˸Ч��
        if(bulletNum <= 0)
        {
            flashText.GetComponent<TextFlash>().StartFlash();
        }

        //�ӵ�����С�ڵ���0ʱ����UI��˸Ч��
        if (bulletNum <= 0)
        {
            flashText.GetComponent<TextFlash>().StartFlash();
        }
        //ǹе�����ڽ�ɫ��ǹе��λ��
        gameObject.transform.position = gunPoint.position; 
        //��ȡ��ǰ���λ�ò�����ת��Ϊ��������
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //ǹе��ת
        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);
        //ǹе���
        Shoot();
    }

    protected virtual void Shoot()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                timer = 0;
        }

        if (Input.GetButton("Fire2"))
        {
            //���������ʱ���������ӵ���������0����Թ���
            if (timer == 0 && bulletNum > 0)
            {
                bulletNum--;
                timer = interval;
                Fire();
            }
        }
    }

    protected virtual void Fire()
    {
        animator.SetTrigger("shoot");

        // GameObject bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        GameObject bullet = MyObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = bulletPos.position;
        float angel = Random.Range(-bulletAngel, bulletAngel);
        bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(angel, Vector3.forward) * direction);

        //�����������
        fireSound.HandleEvent(gameObject);

        // Instantiate(shellPrefab, shellPos.position, shellPos.rotation);
        GameObject shell = MyObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = bulletShellPos.position;
        shell.transform.rotation = bulletShellPos.rotation;
    }

    //ʰȡ�����ӵ��ĵ��ߺ󼤻�ú���
    public void AddBullet()
    {
        bulletNum = bulletNum + addBulletNum;
    }
}
