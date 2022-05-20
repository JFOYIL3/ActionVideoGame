using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStats : MonoBehaviour
{


    [SerializeField] Slider bossHealthBar;
    [SerializeField] Slider damageHealthSlider;
    [SerializeField] AudioManager audioManager;
    private Animator animator;

    public float health;
    public float knockBackLength = 0.5f;
    public float knockBackForce = 15f;

    public Rigidbody2D rb;
    public BoxCollider2D box;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDamagedHealth();
    }

    public void EnemyDamaged(){
        //this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        health -= 1;
        bossHealthBar.value = health;
        if (health <= 0){
            StartCoroutine(Dying());
        }
        else{
            animator.SetTrigger("Hurt");
            audioManager.Play("Goblin Hit");
            
        }
    }

    void UpdateDamagedHealth()
    {
        if (damageHealthSlider.value > bossHealthBar.value)
        {
            damageHealthSlider.value -= .01f;
            
        }
    }

    

    IEnumerator Dying(){
        animator.SetBool("Dead", true);
        this.gameObject.layer = 10;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        box.enabled = false;
        audioManager.Play("Boss Death");
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
