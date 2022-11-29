using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    // configurables
    public Node TopLeftCornerNode;
    public Node BottomRightCornerNode;
    
    public bool containsPlayer;    
    private Vector3 topLeftCorner;
    private Vector3 bottomRightCorner;

    // related objects
    public Player player;

    public virtual void Start() {
	// get player
	this.player = GameObject.Find("/player").GetComponent<Player>();

	// get corner coordinates, then delete nodes
	this.topLeftCorner = TopLeftCornerNode.transform.position;
	this.bottomRightCorner = BottomRightCornerNode.transform.position;

	GameObject.Destroy(TopLeftCornerNode.gameObject);
	GameObject.Destroy(BottomRightCornerNode.gameObject);
    }

    public virtual void Update() {
	bool currentlyContainsPlayer = this.DoesZoneContainPlayer();
	if (this.containsPlayer && !currentlyContainsPlayer) {
	    this.containsPlayer = false;
	    this.OnPlayerExit();
	} else if (!this.containsPlayer && currentlyContainsPlayer) {
	    this.containsPlayer = true;
	    this.player.mostRecentZone = this;
	    this.OnPlayerEnter();
	}
    }

    private bool DoesZoneContainPlayer() {
	Vector3 playerPosition = this.player.transform.position;

	// top left
	if (playerPosition.x < this.topLeftCorner.x || playerPosition.y > this.topLeftCorner.y) {
	    return false;
	}

	// bottom right
	if (playerPosition.x > this.bottomRightCorner.x || playerPosition.y < this.bottomRightCorner.y) {
	    return false;
	}

	return true;
    }

    public virtual void OnPlayerEnter() {
	Debug.Log("player enter: " + this.gameObject.name);
    }

    public virtual void OnPlayerExit() {
	Debug.Log("player exit: " + this.gameObject.name);
    }
}
