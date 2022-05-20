using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 10f;

    // Update is called once per frame
    void Update()
    {
        if (health <= 0){
            Die();
        }
    }

    void Die(){
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }

    public void  TakeDamage(float damage){
        Debug.Log("Damage Taken = " + damage);
        health -= damage;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 7)
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        // other.gameObject.GetComponent<PlatformerPlayer>().TakeDamage(15);
    }
}
