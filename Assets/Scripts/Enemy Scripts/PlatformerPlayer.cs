using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayer : MonoBehaviour
{
	public float health = 1;
    public float speed = 5.0f;
	private float run = 1;
	public float Max_Speed = 10.0f;
    public float jumpForce = 12.0f;

    public float gravityForce = 45.0f;
	public bool isgrounded = false;
	public bool hasDoubleJumped = false;
    public float minHeightForDeath;

	public bool knockBack = false;
	private bool doubleJumpUnlocked = false;

	

    private Animator _anim;
    private Rigidbody2D _body;
    private BoxCollider2D _box;
	[SerializeField] private LayerMask groundLayerMask;

	

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }


	private bool IsGrounded(){
		float extraHeight = .02f;
		RaycastHit2D raycastHit = Physics2D.BoxCast(_box.bounds.center, _box.bounds.size ,0f, Vector2.down, _box.bounds.extents.y + extraHeight , groundLayerMask);
		isgrounded = false;
		Color rayColor;
		if(raycastHit.collider != null){
			isgrounded = true;
			rayColor = Color.green;
		}else{
			rayColor = Color.red;
		}
		Debug.Log(raycastHit.collider);
		Debug.DrawRay(_box.bounds.center,Vector2.down*(_box.bounds.extents.y + extraHeight), rayColor);
		return raycastHit.collider != null;
	}
    // Update is called once per frame
    void Update()
    {
		float deltaX;
	    if(Input.GetKeyDown(KeyCode.LeftShift)){
	 		run = 3f;
			 deltaX = Input.GetAxis("Horizontal") * speed * run;
	  	}else{
			deltaX = Input.GetAxis("Horizontal") * speed;
	 	}
      
		Vector2 movement = new Vector2(deltaX, _body.velocity.y);
		_body.velocity = movement;

		Vector3 max = _box.bounds.max;
		Vector3 min = _box.bounds.min;
		Vector2 corner1 = new Vector2(max.x, min.y+.1f);
		Vector2 corner2 = new Vector2(min.x, min.y -.2f);
		Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

		_body.gravityScale = (IsGrounded() && Mathf.Approximately(deltaX, 0)) ? 0 : gravityForce;
		if (IsGrounded() && Input.GetButtonDown("Jump")) {
			hasDoubleJumped = false;
			_body.gravityScale = gravityForce;
			_body.velocity = new Vector2(_body.velocity.x,0);
			_body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}else if(!IsGrounded() && !hasDoubleJumped && Input.GetButtonDown("Jump") && doubleJumpUnlocked){
			_body.velocity = new Vector2(_body.velocity.x,0);
			_body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			hasDoubleJumped = true;
		}

		// MovingPlatform platform = null;

		if (hit != null) {
			// platform = hit.GetComponent<MovingPlatform>();
		}

		// if (platform != null) {
		// 	transform.parent = platform.transform;
		// } else {
		// 	transform.parent = null;
		// }

		_anim.SetFloat("speed", Mathf.Abs(deltaX));

		// Vector3 pScale = Vector3.one;
		// if (platform != null) {
		// 	pScale = platform.transform.localScale;
		// }

		if (!Mathf.Approximately(deltaX, 0)) {
			// transform.localScale = new Vector3(Mathf.Sign(deltaX)/ pScale.x, 1/pScale.y, 1);
		}
	
        if(transform.position.y < minHeightForDeath){
            transform.position = new Vector3(-12,0,-.1f);
        }
		
    }

	public Transform GetTransform(){
		return transform;
	}

	void Die(){
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
	public void  TakeDamage(float damage){
        Debug.Log("Player Takes Damage = " + damage);
        health -= damage;
		if (knockBack && transform.localScale.x > 0)
		{
			_body.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            _body.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
		}else if(knockBack && transform.localScale.x < 0){
			_body.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            _body.AddForce(Vector2.left * 5f, ForceMode2D.Impulse);
		}
    }
	private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == 7)
        {
            TakeDamage(2);
			Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }else if(other.gameObject.layer == 8){
			hasDoubleJumped = false;
		}   

    }
	public void SetDoubleJumpAbility(bool enableDoubleJump){
		doubleJumpUnlocked = enableDoubleJump;
	}
}
