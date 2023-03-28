using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class RubyControl : MonoBehaviour
{
    //刚体对象
    private  new Rigidbody2D rigidbody;
    private Animator animator;//动画组件
    private Vector2 lookDirection = new Vector2(1, 0);
    //获取用户输入
    float horizontal;
    float vertical;
    
    //速度
    private float speed = 5.0f;

    //血量
    public float maxHealth = 10.0f;
    public float health
    {//设置可访问性
        get { return currentHealth; }
        //角色当前血量设置为不可在外部设置 只能通过当前类的changeHealth方法进行修改
        //set { currentHealth = value; }
    }
    private float currentHealth=1.0f;

    //无敌时间计时器
    public float timeinvincible=1.0f;//无敌时间
    private float invincibleTimer;//计数器
    private bool isinvincible;

    //存储飞弹的公共变量
    public GameObject Cogbullet;
    private void Start()
    {
        //QualitySettings.vSyncCount= 0;//设置垂直同步
        //Application.targetFrameRate= 60;//锁帧
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth= maxHealth;

        isinvincible= false;
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) { //比较是否移动
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();//归一化,只需要方向信息
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        //如果角色还在无敌时间
        if (isinvincible==true)
        {
             invincibleTimer-=Time.deltaTime;//每帧都减去单位帧数的时间
            if (invincibleTimer <= 0) { 
                isinvincible=false;
            }
        }
        //玩家按按键发射
        if (Input.GetKeyDown(KeyCode.F)) {
            Luanch();
        }
    }

    //去除碰撞bug
    private void FixedUpdate()
    {
        if (rigidbody != null)
        {
            Vector2 position = rigidbody.position;
            //Time.deltaTime 为每帧的时间间隔 s/fps
            position.x = position.x + speed * horizontal *Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;
            
            rigidbody.MovePosition(position);
        }
    }
    //控制血量函数
    public void ChangeHealth(float amount) {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isinvincible == true)
            {
                return;
            }
            else
            {
                currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
                isinvincible = true;
                invincibleTimer = timeinvincible;//重置无敌时间
            }
        }
        else {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        }
        Debug.Log($"当前血量{currentHealth}/{maxHealth}");
    }

    //发射飞弹函数
    void Luanch() {
        GameObject projectileObject = Instantiate(Cogbullet, rigidbody.position + Vector2.up * 0.5f, Quaternion.identity);
        
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
    }
}