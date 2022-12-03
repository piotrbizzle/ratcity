using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDetectorZone : Zone
{
    // configurables
    private float MaxTransitionTime = .2f;

    // TODO: different effect for open / close
    // counters
    public float currentFadeInTime;
    public float currentFadeOutTime;
    
    // NB: metal detectors need to also be walls!
    
    public override void Update() {
	base.Update();
	if (this.currentFadeOutTime > 0) {
	    this.currentFadeOutTime -= Time.deltaTime;
	    if (this.currentFadeOutTime < 0) {
		this.currentFadeOutTime = 0;
		this.GetComponent<BoxCollider2D>().enabled = false;
	    }
	    this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, this.currentFadeOutTime / this.MaxTransitionTime);
	} else if (this.currentFadeInTime > 0) {
	    this.currentFadeInTime -= Time.deltaTime;
	    if (this.currentFadeInTime < 0) {
		this.currentFadeInTime = 0;
		this.GetComponent<BoxCollider2D>().enabled = true;
	    }
	    this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - this.currentFadeInTime / this.MaxTransitionTime);
	}	
    }

    public override void OnPlayerEnter() {
	this.player.mayAccessInventory = false;
	
	// exit early if player has anything metal
	if (!this.player.hasAntiMetalDetector) {	    
	    foreach (Transform child in this.player.inventory.transform) {
		InventoryItem inventoryItem = child.GetComponent<InventoryItem>();
		if (inventoryItem.isMetal) {
		    return;
		}
	    }
	}
	this.currentFadeOutTime = this.MaxTransitionTime;
	this.currentFadeInTime = 0;
    }

    public override void OnPlayerExit() {
	this.player.mayAccessInventory = true;
	this.currentFadeInTime = this.MaxTransitionTime;
	this.currentFadeOutTime = 0;
    }
}
