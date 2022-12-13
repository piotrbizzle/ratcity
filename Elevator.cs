using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    // configurables
    public float speed = 10;
    public float maxFloorDelay = 0.5f;
    
    public float[] floorYs;
    public float currentFloorDelay;
    public bool goingDown;
    private int currentFloorIndex;
    

    // Start is called before the first frame update
    void Start()
    {
    	// get floor y coordinates
	this.floorYs = new float[this.transform.childCount];
	for (int i = 0; i < this.transform.childCount; i++) {
	    if (this.transform.GetChild(i).gameObject.name != "FloorNode") {
		continue; // this is a lame fix for jumptrigger getting deleted
	    }
	    this.floorYs[i] = this.transform.GetChild(i).position.y;
	    GameObject.Destroy(this.transform.GetChild(i).gameObject);
	}
	this.transform.position = new Vector3(this.transform.position.x, this.floorYs[0], 0);
    }

    // Update is called once per frame
    void Update()
    {
	if (this.currentFloorDelay > 0) {
	    this.currentFloorDelay -= Time.deltaTime;
	    if (this.currentFloorDelay <= 0) {
		if (this.goingDown) {
		    this.currentFloorIndex -= 1;
		    if (this.currentFloorIndex == 0) {
			this.goingDown = false;
		    } 	      
		} else {
		    this.currentFloorIndex += 1;
		    if (this.currentFloorIndex == floorYs.Length - 1) {
			this.goingDown = true;
			this.currentFloorIndex -= 1;
		    }
		}
	    }
	    return;
	}
	
        if (this.goingDown) {
	    this.transform.position -= new Vector3(0, this.speed * Time.deltaTime, 0);
	    float nextFloorY = this.floorYs[this.currentFloorIndex - 1];
	    if (this.transform.position.y <= nextFloorY) {
		this.currentFloorDelay = this.maxFloorDelay;
	    }
	} else {
	    this.transform.position += new Vector3(0, this.speed * Time.deltaTime, 0);
	    float nextFloorY = this.floorYs[this.currentFloorIndex + 1];
	    if (this.transform.position.y >= nextFloorY) {
		this.currentFloorDelay = this.maxFloorDelay;
	    }
	}
    }
}
