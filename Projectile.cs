using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private new Rigidbody2D rigidbody; 

    //这里不用Start是因为在创建对象时不会运行start，而是在下一帧
    void Awake()
    {
        rigidbody= GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction ,float force) {
        rigidbody.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"触碰到了{other.gameObject}");
        if (other.gameObject.GetComponent<Enemy>()) { 
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
