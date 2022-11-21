using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // constants
    private float SCURRYHEIGHTDELTA = 0.38f;
    
    // configurables
    public float WalkDrag = 40;
    public float WalkAcceleration = 40;
    public float WalkMaxSpeed = 8;    
    public float WalkJumpBoostPower = 16;
    
    public float ScurryDrag = 40;
    public float ScurryAcceleration = 40;
    public float ScurryMaxSpeed = 12;
    public float ScurryJumpBoostPower = 4;

    public float GravityAcceleration = 4;
    public float GravityMaxSpeed = 10;

    public float MaxOnGroundSeconds = 0.1f;
    public float MaxJumpBoostSeconds = 0.2f;
    
    // movement
    private float xMomentum;
    private float yMomentum;
    private bool isScurrying;
    private float onGroundSeconds;

    private float jumpBoostSeconds;
    private int jumpsRemaining;
    
    private bool facingRight;

    // animation
    // 0, 1 - walk right / left
    // 2, 3 - scurry right / left
    public Sprite[] frames;

    
    // Start is called before the first frame update
    void Start()
    {
	// collision
        this.gameObject.AddComponent<BoxCollider2D>();

	Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
	rb.gravityScale = 0.0f;
	rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	rb.sleepMode = RigidbodySleepMode2D.NeverSleep;	
    }

    // Update is called once per frame
    void Update()
    {
	this.MovePlayer();
	this.AnimatePlayer();
    }

    private void MovePlayer() {
	// get inputs
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");
	bool scurry = Input.GetKey("s");
	bool jump = Input.GetKey("space");

	if (!this.isScurrying && scurry) {
	    this.isScurrying = true;
	    this.transform.Translate(Vector3.down * this.SCURRYHEIGHTDELTA);
	    this.transform.Rotate(new Vector3(0, 0, 270));
	} else if (this.isScurrying && !scurry) {
	    this.isScurrying = false;
	    this.transform.Rotate(new Vector3(0, 0, -270));
	    this.transform.Translate(Vector3.up * this.SCURRYHEIGHTDELTA);
	}
	
	// respond to inputs
	float drag = this.isScurrying ? this.ScurryDrag : this.WalkDrag;
	float acceleration = this.isScurrying ? this.ScurryAcceleration : this.WalkAcceleration;
	float maxSpeed = this.isScurrying ? this.ScurryMaxSpeed : this.WalkMaxSpeed;
	
	if (left && !right) {
	    this.facingRight = false;
	    if (this.xMomentum < maxSpeed) {
		this.xMomentum += acceleration * Time.deltaTime;
	    }
	}
	if (right && !left) {
	    this.facingRight = true;
	    if (this.xMomentum > -1 * maxSpeed) {
		this.xMomentum -= acceleration * Time.deltaTime;
	    }
	}
	if ((!right && !left) || this.xMomentum > maxSpeed || this.xMomentum < -1 * maxSpeed) {
	    if (this.xMomentum > 0) {
		this.xMomentum -= drag * Time.deltaTime;
		if (this.xMomentum < 0) {
		    this.xMomentum = 0;
		}
	    } else if (this.xMomentum < 0) {
		this.xMomentum += drag * Time.deltaTime;
		if (this.xMomentum > 0) {
		    this.xMomentum = 0;
		}
	    }
	}

	// jumping
	if (jump && this.jumpBoostSeconds <= 0 && this.onGroundSeconds >= this.MaxOnGroundSeconds && this.jumpsRemaining > 0) {
	    float jumpBoost = this.isScurrying ? this.ScurryJumpBoostPower : this.WalkJumpBoostPower;
	    this.jumpsRemaining -= 1;
	    this.jumpBoostSeconds = this.MaxJumpBoostSeconds;
	    this.yMomentum = jumpBoost;

	    // scurry jump also boost forward
	    if (this.isScurrying) {
		this.xMomentum += (this.facingRight ? -1 : 1) * jumpBoost;
	    }
	}
	
	if (this.jumpBoostSeconds > 0) {
	    this.jumpBoostSeconds -= Time.deltaTime;
	    if (this.jumpBoostSeconds < 0) {
		this.jumpBoostSeconds = 0;
	    }	    
	}
	
	// gravity
	if (this.onGroundSeconds <= 0 && this.jumpBoostSeconds <= 0) {
	    if (this.yMomentum > -1 * this.GravityMaxSpeed) {
		this.yMomentum -= this.GravityAcceleration;
	    }
	} else {
	    if (this.yMomentum < 0) {
		this.yMomentum = 0;
	    }
	    this.onGroundSeconds -= Time.deltaTime;
	}
	
	// apply momentum
	Vector3 forwardVector = this.isScurrying ? Vector3.down : Vector3.left;
	Vector3 upVector = this.isScurrying ? Vector3.left : Vector3.up;
	this.transform.Translate(forwardVector * Time.deltaTime * this.xMomentum);
	this.transform.Translate(upVector * Time.deltaTime * this.yMomentum);
    }

    private void AnimatePlayer() {
	// TODO: will break with more animation
	int frameNumber = (this.facingRight ? 1 : 0) + (this.isScurrying ? 2 : 0);
	this.GetComponent<SpriteRenderer>().sprite = this.frames[frameNumber];
    }

        // Ontriggerstay2d called when this collides with another BoxCollider2D w/ isTrigger=true
    void OnTriggerStay2D(Collider2D collider)
    {
	// check if on platform
	JumpTrigger collidedJumpTrigger = collider.gameObject.GetComponent<JumpTrigger>();
	if (collidedJumpTrigger != null && this.yMomentum <= 0) {
	    // increase timer for how long since player has been on the ground
	    this.onGroundSeconds += 2 * Time.deltaTime;
	    if (this.onGroundSeconds > this.MaxOnGroundSeconds) {
		this.onGroundSeconds = this.MaxOnGroundSeconds;
	    }
	    
	    // glue player to top of platform and reset jumps
	    if (this.jumpBoostSeconds <= 0) {		
		Platform parentPlatform = collidedJumpTrigger.transform.parent.GetComponent<Platform>();
		this.transform.position = new Vector3(this.transform.position.x, parentPlatform.transform.position.y + 0.5f * parentPlatform.height + (this.isScurrying ? 0.36f : 0.5f), 0);

		// TODO: change if we have more than 1 jump
		this.jumpsRemaining = 1;
	    }
	}
    }

}
