using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pathfinding;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] public Slider playerHealthBar;
    [SerializeField] public Slider playerPowerBar;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Animator deathAnimation;
    [SerializeField] Text areaText;
    [SerializeField] LevelLoader levelLoader;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private bool                dead = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    private bool _onDoor;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float health;
    public float power;




    // Use this for initialization
    void Start ()
    {
        if (SceneManager.GetActiveScene().name == "Boss"){
            areaText.text = "Area: BOSS";
        }
        else{
            areaText.text = "Area: " + SceneManager.GetActiveScene().buildIndex.ToString();
        }
        
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        health = 10.0f;
        power = 10.0f;
        _onDoor = false;
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0 && !dead)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputX < 0 && !dead)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        // Move
        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        //m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        //m_animator.SetBool("WallSlide", m_isWallSliding);

        

            
        
        

        //Attack
        if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling && !dead)
        {
            audioManager.Play("Sword Slash");
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach(Collider2D enemy in hitEnemies){
                Scene currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == "Boss"){
                    enemy.GetComponent<BossStats>().EnemyDamaged();
                }
                else{
                    enemy.GetComponent<EnemyStats>().EnemyDamaged();
                }
                
            }

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            if(power > 0){
                m_animator.SetTrigger("Block");
                m_animator.SetBool("IdleBlock", true);
                power -= 1;
                playerPowerBar.value = power;
                audioManager.Play("Fireball");
            }
            
        }

        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
            

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }


        if (Input.GetKeyDown("q")){
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        // go through a door
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _onDoor)
        {
            //Scene scene = SceneManager.GetActiveScene();
            levelLoader.LoadNextLevel();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetKey("z"))
        {
            Application.Quit();
        }

        
    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.layer == 6){
            m_animator.SetTrigger("Hurt");
            health -= 1;
            playerHealthBar.value = health;
            if(health <= 0){
                Die();
            }
            else{
                audioManager.Play("Player Hurt");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.layer == 11){
            Die();
        }

        if (col.gameObject.layer == 13){
            col.gameObject.GetComponentInParent<AIPath>().enabled = true;
        } 
    }

    

    void Die(){
        m_body2d.constraints = RigidbodyConstraints2D.FreezeAll;
        dead = true;
        audioManager.Stop("Main Music");
        audioManager.Play("Death Music");
        m_speed = 0;
        m_animator.SetBool("noBlood", m_noBlood);
        m_animator.SetTrigger("Death");
        audioManager.Play("Death Sound");
        deathAnimation.SetTrigger("Play");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // checks to see if you're in front of a door
        if (other.gameObject.layer == 12)
        {
            _onDoor = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // checks to see if you're not in front of a door
        if (other.gameObject.layer == 12)
        {
            _onDoor = false;
        }

        if (other.gameObject.layer == 13){
            other.gameObject.GetComponentInParent<AIPath>().enabled = false;
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
