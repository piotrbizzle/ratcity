using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // constants
    private float SCURRYHEIGHTDELTA = 0.38f;
    
    // configurables
    private float WalkDrag = 15;
    private float WalkAcceleration = 20;
    private float WalkMaxSpeed = 6;    

    private float ScurryDrag = 8;
    private float ScurryAcceleration = 12;
    private float ScurryMaxSpeed = 12;    
    
    // movement
    private float yMomentum;
    private float xMomentum;
    private bool isScurrying;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
	
	// apply momentum
	Vector3 forwardVector = this.isScurrying ? Vector3.down : Vector3.left;
	this.transform.Translate(forwardVector * Time.deltaTime * this.xMomentum);
    }
}
