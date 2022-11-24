using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingZone : Zone
{
    // Start is called before the first frame update
    public override void Start() {
	base.Start();
	this.SetActive(this.containsPlayer);	    
    }
    
    public override void OnPlayerEnter() {
	this.SetActive(true);
    }

    public override void OnPlayerExit() {
	this.SetActive(false);	    
    }

    private void SetActive(bool containsPlayer) {
	foreach (Transform child in this.transform) {
	    child.gameObject.SetActive(containsPlayer);
	}
	this.containsPlayer = containsPlayer;
    }
}
