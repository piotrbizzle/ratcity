using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDoor : Hittable {
    // configurables
    public float[] knockPattern;
    public Sprite closeSprite;
    public Sprite openSprite;
    public Sprite badSequenceSprite;
    public float maxKnockCooldown = 0.3f;

    public int currentKnockIndex = -1;
    public float currentKnockCounter;
    public float currentKnockCooldown;
   
    public void Update() {
	if (this.currentKnockIndex == -1) {
	    if (this.currentKnockCooldown > 0) {
		this.currentKnockCooldown -= Time.deltaTime;
	    }
	    if (this.currentKnockCooldown <= 0) {
		this.GetComponent<SpriteRenderer>().sprite = this.closeSprite;
	    }
	    return;
	}

	// odd indicies are knocks, evens are pauses
	bool isPause = this.currentKnockIndex % 2 == 0;
	
	this.currentKnockCounter -= Time.deltaTime;
	if (this.currentKnockCounter <= 0) {
	    if (isPause){
		this.nextKnock();
	    } else {
		// failed, reset knock sequence
		this.currentKnockIndex = -1;
		this.currentKnockCooldown = this.maxKnockCooldown;
		this.GetComponent<SpriteRenderer>().sprite = this.badSequenceSprite;
	    }
	}	
    }
    
    public override void GetHit() {
	// don't let sequence start if on cooldown
	if (this.currentKnockCooldown > 0) {
	    return;
	}
	
	// start sequence
	if (this.currentKnockIndex == -1) {
	    this.nextKnock();
	    return;
	}
	
	// continue sequence
	bool isPause = this.currentKnockIndex % 2 == 0;
	if (isPause) {
	    // failed, reset sequence
	    this.currentKnockIndex = -1;
	    this.currentKnockCooldown = this.maxKnockCooldown;
	    this.GetComponent<SpriteRenderer>().sprite = this.badSequenceSprite;
	} else {
	    this.nextKnock();
	}	
    }

    private void nextKnock() {
	this.currentKnockIndex += 1;

	// completed knock sequence
	if (this.currentKnockIndex >= this.knockPattern.Length) {
	    this.openDoor();
	    return;
	}

	// move to next item in knock sequence
	this.currentKnockCounter = this.knockPattern[this.currentKnockIndex];
    }

    private void openDoor() {	
	Destroy(this.GetComponent<BoxCollider2D>());
	Destroy(this.GetComponent<Rigidbody2D>());
	Destroy(this);
	this.GetComponent<SpriteRenderer>().sprite = this.openSprite;
    }
}

