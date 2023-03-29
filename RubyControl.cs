using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class RubyControl : MonoBehaviour
{
    //�������
    private  new Rigidbody2D rigidbody;
    private Animator animator;//�������
    private Vector2 lookDirection = new Vector2(1, 0);
    //��ȡ�û�����
    float horizontal;
    float vertical;
    
    //�ٶ�
    private float speed = 5.0f;

    //Ѫ��
    public float maxHealth = 10.0f;
    public float health
    {//���ÿɷ�����
        get { return currentHealth; }
        //��ɫ��ǰѪ������Ϊ�������ⲿ���� ֻ��ͨ����ǰ���changeHealth���������޸�
        //set { currentHealth = value; }
    }
    private float currentHealth=1.0f;

    //�޵�ʱ���ʱ��
    public float timeinvincible=1.0f;//�޵�ʱ��
    private float invincibleTimer;//������
    private bool isinvincible;
    private void Start()
    {
        //QualitySettings.vSyncCount= 0;//���ô�ֱͬ��
        //Application.targetFrameRate= 60;//��֡
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
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) { //�Ƚ��Ƿ��ƶ�
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();//��һ��,ֻ��Ҫ������Ϣ
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isinvincible==true)
        {
             invincibleTimer-=Time.deltaTime;//ÿ֡����ȥ��λ֡����ʱ��
            if (invincibleTimer <= 0) { 
                isinvincible=false;
            }
        }
    }

    //ȥ����ײbug
    private void FixedUpdate()
    {
        if (rigidbody != null)
        {
            Vector2 position = rigidbody.position;
            //Time.deltaTime Ϊÿ֡��ʱ���� s/fps
            position.x = position.x + speed * horizontal *Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;

            rigidbody.MovePosition(position);
        }
    }
    //����Ѫ������
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
                invincibleTimer = timeinvincible;//�����޵�ʱ��
            }
        }
        else {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        }
        Debug.Log($"��ǰѪ��{currentHealth}/{maxHealth}");
    }
}