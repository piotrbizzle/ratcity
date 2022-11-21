using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    // configurables
    // TODO: possible optimization deleting nodes
    public Node TopLeftCorner;
    public Node BottomRightCorner;
    public Player player;
    
    public bool isActive;
    

    public void Start() {
	this.SetActive(this.isActive);	    
    }

    // Update is called once per frame
    void Update()
    {
	bool zoneContainsPlayer = this.DoesZoneContainPlayer();
	if (this.isActive && !zoneContainsPlayer) {
	    this.SetActive(false);	    
	} else if (!this.isActive && zoneContainsPlayer) {
	    this.SetActive(true);
	}
    }

    private bool DoesZoneContainPlayer() {
	Vector3 playerPosition = this.player.transform.position;

	// top left
	if (playerPosition.x < this.TopLeftCorner.transform.position.x || playerPosition.y > this.TopLeftCorner.transform.position.y) {
	    return false;
	}

	// bottom right
	if (playerPosition.x > this.BottomRightCorner.transform.position.x || playerPosition.y < this.BottomRightCorner.transform.position.y) {
	    return false;
	}

	return true;
    }

    private void SetActive(bool isActive) {
	foreach (Transform child in this.transform) {
	    child.gameObject.SetActive(isActive);
	}
	this.isActive = isActive;
    }


}
