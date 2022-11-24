using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFist : MonoBehaviour
{    
    // configurables
    private float MaxHittingTime = .2f;
    private float MaxHittingCooldown = .4f;

    // counters
    private float currentHittingTime;
    private float currentHittingCooldown;

    public bool hasHit; // can only hit one hittable. hm.

    // Start is called before the first frame update
    void Start()
    {
        this.SetVisible(false);
	
	// collision
	this.gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
    }

    void Update() {
	if (this.currentHittingCooldown > 0) {
	    this.currentHittingCooldown -= Time.deltaTime;
	}
	
	if (!this.IsHitting()) {
	    return;
	}
	this.currentHittingTime -= Time.deltaTime;
	if (this.currentHittingTime <= 0) {
	    this.SetVisible(false);
	}		    
    }

    public bool IsHitting() {
	return this.currentHittingTime > 0;
    }
    
    public void StartHitting() {
	if (this.currentHittingCooldown > 0) {
	    return;
	}
	this.hasHit = false;
	this.currentHittingTime = this.MaxHittingTime;
	this.currentHittingCooldown = this.MaxHittingCooldown;
	this.SetVisible(true);
    }

    private void SetVisible(bool isVisible) {
	this.GetComponent<SpriteRenderer>().color = isVisible? Color.white : Color.clear;
    }
}
