using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // constants
    private float SCURRYHEIGHTDELTA = 0.38f;
    
    // configurables
    private float WalkDrag = 30;
    private float WalkAcceleration = 20;
    private float WalkMaxSpeed = 6;    

    private float ScurryDrag = 15;
    private float ScurryAcceleration = 12;
    private float ScurryMaxSpeed = 12;    

    private float GravityAcceleration = 4;
    private float GravityMaxSpeed = 8;

    private float CoyoteTimeSeconds = 0.1f;
    
    // movement
    private float xMomentum;
    private float yMomentum;
    private bool isScurrying;
    private float onGroundSeconds;
    
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
        // get inputs
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");
	bool scurry = Input.GetKey("left shift");

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
	    if (this.xMomentum < maxSpeed) {
		this.xMomentum += acceleration * Time.deltaTime;
	    }
	}
	if (right && !left) {
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

	// gravity
	if (this.onGroundSeconds <= 0) {
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

        // Ontriggerstay2d called when this collides with another BoxCollider2D w/ isTrigger=true
    void OnTriggerStay2D(Collider2D collider)
    {	
	JumpTrigger collidedJumpTrigger = collider.gameObject.GetComponent<JumpTrigger>();
	if (collidedJumpTrigger != null && this.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0.1) {
	    this.onGroundSeconds = this.CoyoteTimeSeconds;
	    
	    // TODO: this will break when jump is added
	    Platform parentPlatform = collidedJumpTrigger.transform.parent.GetComponent<Platform>();
	    this.transform.position = new Vector3(this.transform.position.x, parentPlatform.transform.position.y + 0.5f * parentPlatform.height + (this.isScurrying ? 0.36f : 0.5f), 0);
	}
    }

}
