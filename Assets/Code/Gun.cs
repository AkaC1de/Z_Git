using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("攻击间隔时间")]
    public float interval;
    [Header("子弹随机偏移角度")]
    public float bulletAngel;
    [Header("枪械出现在游戏中的位置")]
    public Transform gunPoint;
    [Header("子弹的预制体")]
    public GameObject bulletPrefab;
    [Header("弹壳的预制体")]
    public GameObject shellPrefab;
    [Header("子弹生成的位置")]
    public Transform bulletPos;
    [Header("弹壳生成的位置")]
    public Transform bulletShellPos;
    [Header("初始载弹量")]
    public int bulletNum;
    [Header("拾取弹药包后增加的弹药量")]
    public int addBulletNum;
    [Header("UI界面显示的子弹数量")]
    public Text bulletNumText;
    [Header("子弹不足时进行闪烁的UI")]
    public GameObject flashText;
    [Header("音效")]
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
        //将子弹数量变为UI显示的字符串
        bulletNumText.text = bulletNum.ToString();
        //子弹数量小于等于0时播放UI闪烁效果
        if(bulletNum <= 0)
        {
            flashText.GetComponent<TextFlash>().StartFlash();
        }

        //子弹数量小于等于0时播放UI闪烁效果
        if (bulletNum <= 0)
        {
            flashText.GetComponent<TextFlash>().StartFlash();
        }
        //枪械生成在角色的枪械点位置
        gameObject.transform.position = gunPoint.position; 
        //获取当前鼠标位置并将其转化为世界坐标
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //枪械旋转
        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);
        //枪械射击
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
            //攻击间隔计时结束后且子弹数量大于0则可以攻击
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

        //播放射击声音
        fireSound.HandleEvent(gameObject);

        // Instantiate(shellPrefab, shellPos.position, shellPos.rotation);
        GameObject shell = MyObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = bulletShellPos.position;
        shell.transform.rotation = bulletShellPos.rotation;
    }

    //拾取增加子弹的道具后激活该函数
    public void AddBullet()
    {
        bulletNum = bulletNum + addBulletNum;
    }
}
