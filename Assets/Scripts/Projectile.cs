using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public float fireballSpeed = 10.0f;
    [SerializeField] Rigidbody2D rigidbody;
    


    void Start()
    {
        rigidbody.velocity = transform.right * fireballSpeed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.gameObject.layer != 13){
            if (collision.gameObject.layer == 6){
                collision.gameObject.GetComponent<EnemyStats>().health -= 1;
            }
            Destroy(gameObject);
        }
        /*
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.Health();
        }
        */
        //Destroy(gameObject);
    }
}
