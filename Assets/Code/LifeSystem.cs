using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    #region ���������������˽�У�

    //��������
    protected bool isHurting = false;
    protected bool isHurtingSpecial = false;
    protected Animator anim;
    protected AnimatorStateInfo info;
    protected Rigidbody2D rb;
    protected Vector2 backDirection;

    protected float attackDamage;
    protected float backSpeed;

    #endregion

    #region ������������������У�

    [Header("��ʼ/��ǰѪ��")]
    public float blood;
    [Header("���Ѫ��")]
    public float maxBlood;
    [Header("������Ч")]
    public GameObject quantumPre;

    [Header("��Ч")]
    public AkEvent hurtSound;
    public AkEvent deathSound;

    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //��ȡ�������
        anim = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);

        BeatBack();
        IfDeath();

        //��ĿǰѪ���������Ѫ��ʱ����ĿǰѪ���޸�Ϊ���Ѫ��
        if (blood > maxBlood)
        {
            blood = maxBlood;
        }
    }

    #region ���ص��ú���

    //�ܵ��˺���ִ��
    protected virtual void BeatBack()
    {
        if(isHurting == true)
        {
            //����
            rb.AddForce(backSpeed * backDirection, ForceMode2D.Impulse);
            if (info.normalizedTime >= 0.6f)
            {
                isHurting = false;
            }
        }
    }
    
    //Ѫ��Ϊ0ʱִ��
    protected virtual void IfDeath()
    {
        if (blood <= 0)
        {
            anim.SetTrigger("death");
        }
    }

    #endregion

    #region ��Ч�����������ã�


    //���������ã����˺󲥷���Ч
    public virtual void HurtAudio()
    {
        if (hurtSound != null)
        {
            hurtSound.HandleEvent(gameObject);
        }
    }

    //���������ã������󲥷���Ч
    public virtual void DeathAudio()
    {
        if (deathSound != null)
        {
            deathSound.HandleEvent(gameObject);
        }
    }

    #endregion

    #region Ч�������������ã�

    //���������ã������������ź�ݻ�����
    public virtual void isDeath()
    {
        Destroy(gameObject);
    }

    //���������ã����˶������ź����Ѫ��
    public virtual void AttackedBloodChange()
    {
        if (quantumPre != null)
        {        
            //����ϵͳ��������Ч���ö���ؽ��е���
            GameObject quantum = MyObjectPool.Instance.GetObject(quantumPre);
            quantum.transform.position = transform.position;
        }
        blood = blood - attackDamage;
    }


    #endregion

    #region �����ж����ⲿ��������ײ�塱���ã�

    //�����ж������ײ�����
    public virtual void GetHurtAttack(float attackDamage,Vector2 direction,float backSpeed)
    {
        //�������Թ�������Ĳ�������������״̬����������Ӧ�����˶���
        anim.SetTrigger("hurtAttack");
        transform.localScale = new Vector3(-direction.x, 1, 1);
        isHurting = true;

        backDirection = direction;
        this.attackDamage = attackDamage;
        this.backSpeed = backSpeed;
    }
   

    #endregion
}
