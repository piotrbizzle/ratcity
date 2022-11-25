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
    public float WalkJumpBoostPower = 18;
    
    public float ScurryDrag = 40;
    public float ScurryAcceleration = 40;
    public float ScurryMaxSpeed = 12;
    public float ScurryJumpBoostPower = 4;
    
    public float ClimbingSpeed = 8;
    public float ZiplineSpeed = 16;

    public float GravityAcceleration = 4;
    public float GravityMaxSpeed = 12;

    public float MaxOnGroundSeconds = 0.1f;
    public float MaxJumpBoostSeconds = 0.2f;
    public int MaxJumps = 1;
    
    // movement
    private float xMomentum;
    private float yMomentum;
    private bool isScurrying;
    private bool isClimbing;
    private bool isZiplining;
    private bool isZipliningRight;
    public ClimbingNode leftClimbingNode;
    public ClimbingNode rightClimbingNode;
    private float onGroundSeconds;

    private float jumpBoostSeconds;
    private int jumpsRemaining;
    
    private bool facingRight;
    public bool forcedToScurry;

    // animation
    // 0, 1 - walk right / left
    // 2, 3 - scurry right / left
    public Sprite[] frames;

    // related objects
    public PlayerFist fist;
    
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
	this.ActionPlayer();
	this.AnimatePlayer();
    }

    private void MovePlayer() {
	// get inputs
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");
	bool scurry = Input.GetKey("s");
	bool jump = Input.GetKey("space");

	// scurry vs walk
	if (!this.isScurrying && (scurry || this.forcedToScurry)) {
	    this.isScurrying = true;
	    this.transform.Translate(Vector3.down * this.SCURRYHEIGHTDELTA);
	    this.transform.rotation = new Quaternion(0, 0, 0, 0);
	    this.transform.Rotate(new Vector3(0, 0, 270));
	} else if (this.isScurrying && !scurry && !this.forcedToScurry) {
	    this.isScurrying = false;
	    this.isClimbing = false;
	    this.transform.rotation = new Quaternion(0, 0, 0, 0);
	    this.transform.Translate(Vector3.up * this.SCURRYHEIGHTDELTA);
	}
	
	// climbing and zipping
	if (this.isClimbing) {
	    if (this.isZiplining) {
		this.Zipline();
	    } else {
		this.Climb(left, right);
	    }
	    return;
	}

	// horizontal movement
	this.Move(left, right);
	
	// jumping
	if (jump && this.jumpBoostSeconds <= 0 && this.onGroundSeconds >= this.MaxOnGroundSeconds && this.jumpsRemaining > 0) {
	    this.Jump();
	}
	
	// apply gravity
	if (this.jumpBoostSeconds > 0) {
	    this.jumpBoostSeconds -= Time.deltaTime;
	    if (this.jumpBoostSeconds < 0) {
		this.jumpBoostSeconds = 0;
	    }	    
	}

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

    private void Zipline() {
	if (this.isZipliningRight) {
	    this.facingRight = true;
	    this.transform.rotation = Quaternion.FromToRotation(Vector3.down, this.transform.position - this.rightClimbingNode.transform.position);
	    this.transform.Translate(Vector3.up * this.ZiplineSpeed * Time.deltaTime);

	} else {
	    this.facingRight = false;
	    this.transform.rotation = Quaternion.FromToRotation(Vector3.up, this.transform.position - this.leftClimbingNode.transform.position);
	    this.transform.Translate(Vector3.down * this.ZiplineSpeed * Time.deltaTime);
	}
    }

    private void Climb(bool left, bool right) {
	if (left && !right) {
	    this.facingRight = false;
	    this.transform.rotation = Quaternion.FromToRotation(Vector3.up, this.transform.position - this.leftClimbingNode.transform.position);
	    this.transform.Translate(Vector3.down * this.ClimbingSpeed * Time.deltaTime);
	} else if (right && !left) {
	    this.facingRight = true;
	    this.transform.rotation = Quaternion.FromToRotation(Vector3.down, this.transform.position - this.rightClimbingNode.transform.position);
	    this.transform.Translate(Vector3.up * this.ClimbingSpeed * Time.deltaTime);
	}
    }

    private void Move(bool left, bool right) {
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
    }

    private void Jump() {	 
	float jumpBoost = this.isScurrying ? this.ScurryJumpBoostPower : this.WalkJumpBoostPower;
	this.jumpsRemaining -= 1;
	this.jumpBoostSeconds = this.MaxJumpBoostSeconds;
	this.yMomentum = jumpBoost;

	// scurry jump also boost forward
	if (this.isScurrying) {
	    this.xMomentum += (this.facingRight ? -1 : 1) * jumpBoost;
	}       
    }

    private void ActionPlayer() {
	// get inputs
	bool hit = Input.GetKey("j");
	if (hit && !this.isScurrying) {
	    this.fist.StartHitting();
	}
    }

    private void AnimatePlayer() {
	// player frames
	int frameNumber = (this.facingRight ? 1 : 0) + (this.isScurrying ? 2 : 0);
	this.GetComponent<SpriteRenderer>().sprite = this.frames[frameNumber];

	// relocate player fist
	if (isScurrying) {
	    if (this.facingRight) {
		this.fist.transform.localPosition = new Vector3(0, 0.68f, 0);
	    } else {
		this.fist.transform.localPosition = new Vector3(0, -0.68f, 0);
	    }
	} else {
	    if (this.facingRight) {
		this.fist.transform.localPosition = new Vector3(0.54f, 0, 0);
	    } else {
		this.fist.transform.localPosition = new Vector3(-0.54f, 0, 0);
	    }
	}
    }

        // Ontriggerstay2d called when this collides with another BoxCollider2D w/ isTrigger=true
    void OnTriggerStay2D(Collider2D collider)
    {
	// platform triggers
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
		this.jumpsRemaining = this.MaxJumps;
	    }
	}
	
	// climbing nodes
	ClimbingNode collidedClimbingNode = collider.gameObject.GetComponent<ClimbingNode>();
	if (collidedClimbingNode != null) {
	    if (collidedClimbingNode.isGrabbable && this.isScurrying && !this.isClimbing) {
		// grab onto end node
		this.isClimbing = true;
		this.isZiplining = collidedClimbingNode.isZipline;
		if (collidedClimbingNode.left != null) {
		    this.leftClimbingNode = collidedClimbingNode.left;
		    this.rightClimbingNode = collidedClimbingNode;
		    this.isZipliningRight = false;
		} else {
		    this.leftClimbingNode = collidedClimbingNode;
		    this.rightClimbingNode = collidedClimbingNode.right;
		    this.isZipliningRight = true;
		}
		this.xMomentum = 0;
		this.yMomentum = 0;
		this.transform.position = collidedClimbingNode.transform.position;
	    } else if (this.isClimbing) {
		// reset left and ride nodes if needed
		if (this.facingRight) {
		    this.leftClimbingNode = collidedClimbingNode;
		    this.rightClimbingNode = collidedClimbingNode.right != null ? collidedClimbingNode.right : collidedClimbingNode;
		} else {
		    this.leftClimbingNode = collidedClimbingNode.left != null ? collidedClimbingNode.left : collidedClimbingNode;
		    this.rightClimbingNode = collidedClimbingNode;
		}
	    }
	}
    }

}
