using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundZone : Zone
{
    // configurables
    public bool requiresArchitect;
    public bool requiresBootlegger;
    
    // counters
    private float MaxTransitionTime = .2f;
    private float currentFadeInTime;
    private float currentFadeOutTime;
    
    public override void Update() {
	base.Update();
	if (this.currentFadeOutTime > 0) {
	    this.currentFadeOutTime -= Time.deltaTime;
	    if (this.currentFadeOutTime < 0) {
		this.currentFadeOutTime = 0;
	    }
	    this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, this.currentFadeOutTime / this.MaxTransitionTime);
	} else if (this.currentFadeInTime > 0) {
	    this.currentFadeInTime -= Time.deltaTime;
	    if (this.currentFadeInTime < 0) {
		this.currentFadeInTime = 0;
	    }
	    this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - this.currentFadeInTime / this.MaxTransitionTime);
	}	
    }

    public override void OnPlayerEnter() {
	if (this.requiresArchitect && !this.player.hasArchitect) {
	    return;
	}
	if (this.requiresBootlegger && !this.player.hasBootlegger) {
	    return;
	}	
	this.currentFadeOutTime = this.MaxTransitionTime;
	this.currentFadeInTime = 0;
    }

    public override void OnPlayerExit() {
	if (this.requiresArchitect && !this.player.hasArchitect) {
	    return;
	}
	if (this.requiresBootlegger && !this.player.hasBootlegger) {
	    return;
	}	
	this.currentFadeInTime = this.MaxTransitionTime;
	this.currentFadeOutTime = 0;
    }
}
