using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class EnemyStats : MonoBehaviour
{

    [SerializeField] public Slider enemyHealthBar;
    [SerializeField] AudioManager audioManager;
    [SerializeField] public Transform enemyTransform;
    private Animator animator;
    public AIPath aiPath;

    public float health;
    public float knockBackLength = 0.5f;
    public float knockBackForce = 15f;

    public Rigidbody2D rb;
    public BoxCollider2D box;
    //public AIPath path;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        //path = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        //Flips Sprite depending on desired AIPath
        if(aiPath.desiredVelocity.x >= 0.01f){
            enemyTransform.localScale = new Vector3(4f,4f,1f);
        }else if(aiPath.desiredVelocity.x <= -0.01f){
            enemyTransform.localScale = new Vector3(-4f,4f,1f);
        }
    }

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.gameObject.layer == 7){
    //        Debug.Log("OnCollisionEnter2D");
    //        EnemyDamaged();
    //        if (health <= 0){
    //            Destroy(this.gameObject);
    //        }
    //    }  
    //}

    public void EnemyDamaged(){
        this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        health -= 1;
        enemyHealthBar.value = health;
        if (health <= 0){
            StartCoroutine(Dying());
        }
        else{
            animator.SetTrigger("Hurt");
            audioManager.Play("Goblin Hit");
        }
    }

    IEnumerator Dying(){
        animator.SetBool("Dead", true);
        this.gameObject.layer = 10;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        box.enabled = false;
        audioManager.Play("Goblin Death");
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
