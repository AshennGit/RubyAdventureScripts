using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private new Rigidbody2D rigidbody; 

    //���ﲻ��Start����Ϊ�ڴ�������ʱ��������start����������һ֡
    void Awake()
    {
        rigidbody= GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction ,float force) {
        rigidbody.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"��������{other.gameObject}");
        if (other.gameObject.GetComponent<Enemy>()) { 
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
