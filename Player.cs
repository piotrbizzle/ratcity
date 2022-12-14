using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // constants
    private float SCURRYHEIGHTDELTA = 0.38f;
    
    // configurables
    private float WalkDrag = 40;
    private float WalkAcceleration = 40;
    private float WalkMaxSpeed = 10;    
    private float WalkJumpBoostPower = 18;
    
    private float ScurryDrag = 50;
    private float ScurryAcceleration = 50;
    private float ScurryMaxSpeed = 16;
    private float ScurryJumpBoostPower = 12;
    
    private float ClimbingSpeed = 8;
    private float ZiplineSpeed = 20;   
    
    private float GravityAcceleration = 4;
    private float GravityMaxSpeed = 14;

    private float MaxOnGroundSeconds = 0.1f;
    private float MaxJumpBoostSeconds = 0.2f;
    private int MaxJumps = 1;
    
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
    
    public bool facingRight;
    public bool forcedToScurry;

    public bool mayAccessInventory = true;

    private Vector3 resetPosition;

    public float ziplineSpeedMultiplier;

    public int darknessCount;
    
    // dialogue
    public bool isInDialogue;

    // inventory
    public bool isInInventory;
    
    // animation
    // 0, 1 - walk right / left
    // 2, 3 - scurry right / left
    // add 4 for walk animations
    public Sprite[] frames;
    private float maxBetweenFramesSeconds = .1f;
    private float betweenFramesSeconds;
    
    // unlocks
    public bool hasDarkVision;
    public bool hasZipline;
    public bool hasClimb;
    public bool hasZiplineClimb;
    public bool hasAntiMetalDetector;
    public bool hasBootlegger;
    public bool hasArchitect;

    // related objects
    public PlayerFist fist;
    public InkStory inkStory;
    public InventoryScreen inventory;
    public Zone mostRecentZone;
    public SpriteRenderer darkVision;
    public SpriteRenderer noVision;
    
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

	this.resetPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
	// TODO: move out inventory and dialogue controls to separate files
	if (this.isInInventory) {
	    this.isInInventory = this.inventory.ControlMenu();
	} else if (this.isInDialogue) {
	    this.inkStory.ControlMenu();
	} else {
	    this.MovePlayer();
	    this.ActionPlayer();
	}
	this.AnimatePlayer();
    }

    private void MovePlayer() {
	// get inputs
	bool left = Input.GetKey("a");
	bool right = Input.GetKey("d");
	bool scurry = Input.GetKey("s");
	bool jump = Input.GetKey("space");
	bool reset = Input.GetKeyDown("r");

	// reset position if needed
	if (reset && !this.isClimbing) {
	    this.transform.position = this.resetPosition;
	    this.xMomentum = 0;
	    this.yMomentum = 0;
	}
	
	// decide whether to be solid or not
	this.GetComponent<BoxCollider2D>().enabled = !this.isClimbing;

	// scurry vs walk
	if (!this.isScurrying && (scurry || this.forcedToScurry)) {
	    this.isScurrying = true;
	    this.transform.Translate(Vector3.down * this.SCURRYHEIGHTDELTA);
	    this.transform.rotation = new Quaternion(0, 0, 0, 0);
	    this.transform.Rotate(new Vector3(0, 0, 270));
	    this.fist.StopHitting();
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
	
	if (!Input.GetKey("space")) {
	    this.jumpBoostSeconds = 0;
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

    private void ActionPlayer() {
	// get inputs
	bool hit = Input.GetKeyDown("j");
	bool openInventory = Input.GetKeyDown("i");

	if (openInventory && this.mayAccessInventory) {
	    this.isInInventory = true;
	    this.inventory.OpenInventory(null);
	}
	if (hit && !this.isScurrying) {
	    this.fist.StartHitting();
	}
    }    
    
    private void Zipline() {	
	if (this.isZipliningRight) {
	    this.facingRight = true;
	    if (Vector3.Distance(this.transform.position, this.rightClimbingNode.transform.position) < 0.2) {
		// TODO: end of zipline is so dorky
		this.isZiplining = false;
		return;
	    }
	    this.transform.rotation = Quaternion.FromToRotation(Vector3.down, this.transform.position - this.rightClimbingNode.transform.position);
	    this.transform.Translate(Vector3.up * this.ZiplineSpeed * this.ziplineSpeedMultiplier * Time.deltaTime);

	} else {
	    this.facingRight = false;
	    if (Vector3.Distance(this.transform.position, this.leftClimbingNode.transform.position) < 0.2) {
		this.isZiplining = false;
		return;
	    }
	    this.transform.rotation = Quaternion.FromToRotation(Vector3.up, this.transform.position - this.leftClimbingNode.transform.position);
	    this.transform.Translate(Vector3.down * this.ZiplineSpeed * this.ziplineSpeedMultiplier * Time.deltaTime);
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


    private void AnimatePlayer() {
	// check inputs
	bool isMoving = Input.GetKey("a") || Input.GetKey("d");
	bool isOnGround = this.onGroundSeconds >= 0;	
	
	// player frames
	int frameNumber = (this.facingRight ? 1 : 0) + (this.isScurrying ? 2 : 0);
	if (this.betweenFramesSeconds > 0) {
	    this.betweenFramesSeconds -= Time.deltaTime;
	}
	
	if (!isOnGround) {
	    // use run frame in the air because it's cute
	    frameNumber += 4; // running frame 
	} else {
	    // otherwise, alternate between frames if moving
	    if (this.betweenFramesSeconds <= 0 && isMoving) {
		this.betweenFramesSeconds += 2 * this.maxBetweenFramesSeconds;
	    }
	    if (this.betweenFramesSeconds > this.maxBetweenFramesSeconds) {
		frameNumber += 4; // running frame
	    }
	}
	
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
		this.fist.GetComponent<SpriteRenderer>().flipX = false;
		this.fist.transform.localPosition = new Vector3(0.42f, 0.18f, 0);
	    } else {
		this.fist.GetComponent<SpriteRenderer>().flipX = true;
		this.fist.transform.localPosition = new Vector3(-0.42f, 0.18f, 0);
	    }
	}
    }

        // Ontriggerstay2d called when this collides with another BoxCollider2D w/ isTrigger=true
    void OnTriggerStay2D(Collider2D collider)
    {
	// descriptions
	Description collidedDescription = collider.gameObject.GetComponent<Description>();
	if (collidedDescription != null) {
	    collidedDescription.isHighlighted = 5; // TODO: put this in a constant
	    if (Input.GetKey("k")) {
		collidedDescription.OpenDescription(this);
	    }
	}
	
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
	    
	    // move player with elevator if needed
	    Elevator elevator = collidedJumpTrigger.transform.parent.GetComponent<Elevator>();
	    if (elevator != null && elevator.currentFloorDelay <= 0) {
		if (elevator.goingDown) {
		    this.transform.position -= new Vector3(0, elevator.speed * Time.deltaTime, 0);
		} else {
		    this.transform.position += new Vector3(0, elevator.speed * Time.deltaTime, 0);
		}
	    }	    
	}
	
	// climbing nodes
	ClimbingNode collidedClimbingNode = collider.gameObject.GetComponent<ClimbingNode>();
	if (collidedClimbingNode != null) {
	    if (collidedClimbingNode.isGrabbable && this.isScurrying && !this.isClimbing) {
		// only zipline if it's activated and ability is unlocked
		if (collidedClimbingNode.isZipline && (!this.hasZipline || !collidedClimbingNode.isActivated)) {
		    return;
		}

		// only climb if ability is unlocked
		if (!collidedClimbingNode.isZipline && !this.hasClimb) {
		    return;
		}

		// grab onto end node
		this.isClimbing = true;
		this.isZiplining = collidedClimbingNode.isZipline;
		if (this.isZiplining) {
		    this.ziplineSpeedMultiplier = collidedClimbingNode.speedMultiplier;
		}
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
