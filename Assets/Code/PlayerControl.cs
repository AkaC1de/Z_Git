using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    #region ���������������˽�У�

    //�������������
    private Vector2 mousePos;
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private float timer;
    private float _moveSpeed;
    private float _jumpForce;
    private float _gravity;
    private float vertical;
    private bool isLadder = false;
    private bool isClimbing = false;

    #endregion

    #region ������������������У�

    [Header("��ɫ��Ծ���ж�ͼ��")]
    public LayerMask ground;
    [Header("��ɫ��Ծ��˫���ж���")]
    public Transform ceilingPoint1;
    public Transform ceilingPoint2;
    [Header("��ɫ���ƶ��ٶ�")]
    public float moveSpeed;
    [Header("��ɫ�����ӵ��ٶ�")]
    public float climbSpeed;
    [Header("��ɫ����Ծ����")]
    public float jumpForce;
    [Header("�Ƿ�������ϵͳ")]
    public bool ifAttack;
    [Header("��ɫ�������")]
    public float cd;
    [Header("��ɫ����ʱ����������Ծ����˥������")]
    public float speedTime;
    public float jumpForceTime;
    [Header("ǹе")]
    public GameObject gun;
    [Header("��Ч")]
    public AkEvent footStepSound;
    public AkEvent coinSound;
    public AkEvent climbSound;
    public AkEvent swordSound;

    #endregion

    // Start is called before the first frame update
    void Start()
    {

        #region ��ȡ˽�еĳ�ʼ���

        //��ȡ��ʼ���
        rb = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<CapsuleCollider2D>();
        anim = gameObject.GetComponent<Animator>();
        _moveSpeed = moveSpeed;
        _jumpForce = jumpForce;
        _gravity = rb.gravityScale;

        #endregion

    }

    // Update is called once per frame
    void Update()
    {

        #region ��������

        Timer();
        Movement();
        if(ifAttack == true)
        {
            Attack();
        }

        #endregion

    }


    #region ��ʱ��

    void Timer()
    {
        //���ƶ���ظ����������ڹ������������ٺ���Ծ�߶���˥��
        if(timer != 0)
        {
            moveSpeed = _moveSpeed / speedTime;
            jumpForce= _jumpForce / jumpForceTime;   
            timer = timer - Time.deltaTime;
        }
        if(timer <= 0)
        {
            moveSpeed = _moveSpeed;
            jumpForce = _jumpForce;
            timer = 0;
        }
    }

    #endregion

    #region ��ɫ�ƶ�

    public void Movement()
    {
        float horizontalMove;
        float facedirection = Input.GetAxisRaw("Horizontal");
        horizontalMove = Input.GetAxis("Horizontal");
        

        //��ɫ�ƶ�
        rb.velocity = new Vector2(horizontalMove * moveSpeed, rb.velocity.y);
        anim.SetFloat("move", Mathf.Abs(horizontalMove));
        //�������λ�ñ任����
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        //��ɫ�Ƿ��ڵ���
        if(Physics2D.OverlapCircle(ceilingPoint1.position, 0.35f, ground) || Physics2D.OverlapCircle(ceilingPoint2.position, 0.35f, ground))
        {
            anim.SetBool("isGround", true);
        }
        else
        {
            anim.SetBool("isGround", false);
        }

        //��ɫ��Ծ
        if (Physics2D.OverlapCircle(ceilingPoint1.position, 0.01f, ground)|| Physics2D.OverlapCircle(ceilingPoint2.position, 0.01f, ground))
        {
            anim.SetBool("isGround", true);
            anim.SetBool("jump", false);
        }
        if (Input.GetButtonDown("Jump") && (Physics2D.OverlapCircle(ceilingPoint1.position, 0.35f, ground)|| Physics2D.OverlapCircle(ceilingPoint2.position, 0.35f, ground)) && isClimbing == false)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("isGround", false);
            anim.SetBool("jump", true); 
        }

        //������
        vertical = Input.GetAxis("Vertical");
        anim.SetFloat("verticalMove", Mathf.Abs(vertical));
        if (isLadder == true && Mathf.Abs(vertical) > 0)
        {
            isClimbing = true;
            anim.SetBool("climb", true);
        }
        if (isClimbing == true)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
        }
        else
        {
            rb.gravityScale = _gravity;
        }
    }

    #endregion

    #region ��ɫ��ս����

    //��ɫ��ս����
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && timer  == 0)
        {
            timer = cd;
            anim.SetTrigger("attack");
            if (swordSound != null)
            {
                swordSound.HandleEvent(gameObject);
            }
        }
    }

    #endregion

    #region ��Ч�����������ã�

    //�������¼���������Ч
    public void FootStep()
    {
        if(footStepSound != null)
        {
            footStepSound.HandleEvent(gameObject);
        }
    }
    public void Climbing()
    {
        if (climbSound != null)
        {
            climbSound.HandleEvent(gameObject);
        }
    }

    #endregion

    #region ��ײ�ж�

    //��ײ�ж�
    private void OnTriggerEnter2D(Collider2D other)
    {
        //����
        if (other.CompareTag("Ladder"))
        {
            isLadder= true;
        }

        //�ռ������ӵ�����Ʒ
        if (other.CompareTag("ItemBullet"))
        {
            coinSound.HandleEvent(gameObject);
            gun.GetComponent<Gun>().AddBullet();
            Destroy(other.gameObject);
        }

        //Ѫ��
        if (other.CompareTag("ItemBlood"))
        {
            coinSound.HandleEvent(gameObject);
            gameObject.GetComponent<PlayerLife>().AddBlood();
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //����
        if (other.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            anim.SetBool("climb", false);
            rb.gravityScale = _gravity;
        }
    }

    #endregion
}
