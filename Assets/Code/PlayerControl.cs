using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    #region 基础变量与组件（私有）

    //基础变量与组件
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

    #region 基础变量与组件（公有）

    [Header("角色跳跃的判定图层")]
    public LayerMask ground;
    [Header("角色跳跃的双脚判定点")]
    public Transform ceilingPoint1;
    public Transform ceilingPoint2;
    [Header("角色的移动速度")]
    public float moveSpeed;
    [Header("角色爬梯子的速度")]
    public float climbSpeed;
    [Header("角色的跳跃力度")]
    public float jumpForce;
    [Header("是否开启攻击系统")]
    public bool ifAttack;
    [Header("角色攻击间隔")]
    public float cd;
    [Header("角色攻击时的移速与跳跃力度衰减倍率")]
    public float speedTime;
    public float jumpForceTime;
    [Header("枪械")]
    public GameObject gun;
    [Header("音效")]
    public AkEvent footStepSound;
    public AkEvent coinSound;
    public AkEvent climbSound;
    public AkEvent swordSound;

    #endregion

    // Start is called before the first frame update
    void Start()
    {

        #region 获取私有的初始组件

        //获取初始组件
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

        #region 函数调用

        Timer();
        Movement();
        if(ifAttack == true)
        {
            Attack();
        }

        #endregion

    }


    #region 计时器

    void Timer()
    {
        //限制多次重复攻击，且在攻击过程中移速和跳跃高度有衰减
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

    #region 角色移动

    public void Movement()
    {
        float horizontalMove;
        float facedirection = Input.GetAxisRaw("Horizontal");
        horizontalMove = Input.GetAxis("Horizontal");
        

        //角色移动
        rb.velocity = new Vector2(horizontalMove * moveSpeed, rb.velocity.y);
        anim.SetFloat("move", Mathf.Abs(horizontalMove));
        //根据鼠标位置变换方向
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        //角色是否在地面
        if(Physics2D.OverlapCircle(ceilingPoint1.position, 0.35f, ground) || Physics2D.OverlapCircle(ceilingPoint2.position, 0.35f, ground))
        {
            anim.SetBool("isGround", true);
        }
        else
        {
            anim.SetBool("isGround", false);
        }

        //角色跳跃
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

        //爬梯子
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

    #region 角色近战攻击

    //角色近战攻击
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

    #region 音效（动画机调用）

    //动画机事件，播放音效
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

    #region 碰撞判断

    //碰撞判断
    private void OnTriggerEnter2D(Collider2D other)
    {
        //梯子
        if (other.CompareTag("Ladder"))
        {
            isLadder= true;
        }

        //收集增加子弹的物品
        if (other.CompareTag("ItemBullet"))
        {
            coinSound.HandleEvent(gameObject);
            gun.GetComponent<Gun>().AddBullet();
            Destroy(other.gameObject);
        }

        //血包
        if (other.CompareTag("ItemBlood"))
        {
            coinSound.HandleEvent(gameObject);
            gameObject.GetComponent<PlayerLife>().AddBlood();
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //梯子
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
