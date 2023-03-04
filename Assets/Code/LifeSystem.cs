using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    #region 基础变量与组件（私有）

    //基础变量
    protected bool isHurting = false;
    protected bool isHurtingSpecial = false;
    protected Animator anim;
    protected AnimatorStateInfo info;
    protected Rigidbody2D rb;
    protected Vector2 backDirection;

    protected float attackDamage;
    protected float backSpeed;

    #endregion

    #region 基础变量与组件（公有）

    [Header("初始/当前血量")]
    public float blood;
    [Header("最大血量")]
    public float maxBlood;
    [Header("受伤特效")]
    public GameObject quantumPre;

    [Header("音效")]
    public AkEvent hurtSound;
    public AkEvent deathSound;

    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //获取基础组件
        anim = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);

        BeatBack();
        IfDeath();

        //当目前血量大于最大血量时，将目前血量修改为最大血量
        if (blood > maxBlood)
        {
            blood = maxBlood;
        }
    }

    #region 本地调用函数

    //受到伤害后执行
    protected virtual void BeatBack()
    {
        if(isHurting == true)
        {
            //击退
            rb.AddForce(backSpeed * backDirection, ForceMode2D.Impulse);
            if (info.normalizedTime >= 0.6f)
            {
                isHurting = false;
            }
        }
    }
    
    //血量为0时执行
    protected virtual void IfDeath()
    {
        if (blood <= 0)
        {
            anim.SetTrigger("death");
        }
    }

    #endregion

    #region 音效（动画机调用）


    //动画机调用，受伤后播放音效
    public virtual void HurtAudio()
    {
        if (hurtSound != null)
        {
            hurtSound.HandleEvent(gameObject);
        }
    }

    //动画机调用，死亡后播放音效
    public virtual void DeathAudio()
    {
        if (deathSound != null)
        {
            deathSound.HandleEvent(gameObject);
        }
    }

    #endregion

    #region 效果（动画机调用）

    //动画机调用，死亡动画播放后摧毁物体
    public virtual void isDeath()
    {
        Destroy(gameObject);
    }

    //动画机调用，受伤动画播放后减少血量
    public virtual void AttackedBloodChange()
    {
        if (quantumPre != null)
        {        
            //粒子系统，受伤特效，用对象池进行调用
            GameObject quantum = MyObjectPool.Instance.GetObject(quantumPre);
            quantum.transform.position = transform.position;
        }
        blood = blood - attackDamage;
    }


    #endregion

    #region 攻击判定（外部“攻击碰撞体”调用）

    //攻击判定点的碰撞体调用
    public virtual void GetHurtAttack(float attackDamage,Vector2 direction,float backSpeed)
    {
        //传入来自攻击方向的参数，设置受伤状态，并播放相应的受伤动画
        anim.SetTrigger("hurtAttack");
        transform.localScale = new Vector3(-direction.x, 1, 1);
        isHurting = true;

        backDirection = direction;
        this.attackDamage = attackDamage;
        this.backSpeed = backSpeed;
    }
   

    #endregion
}
