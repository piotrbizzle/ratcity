using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{    
    public bool containsPlayer;
    public Vector3 topLeftCorner;
    public Vector3 bottomRightCorner;

    // related objects
    public Player player;

    public virtual void Start() {
	// get player
	this.player = GameObject.Find("/player").GetComponent<Player>();

	// get corner coordinates, then delete nodes
	Transform topLeftNode = this.transform.Find("TopLeft");
	Transform bottomRightNode = this.transform.Find("BottomRight");
	this.topLeftCorner = topLeftNode.position;
	this.bottomRightCorner = bottomRightNode.position;

	GameObject.Destroy(topLeftNode.gameObject);
	GameObject.Destroy(bottomRightNode.gameObject);
    }

    public virtual void Update() {
	bool currentlyContainsPlayer = this.DoesZoneContainPlayer();
	if (this.containsPlayer && !currentlyContainsPlayer) {
	    this.containsPlayer = false;
	    this.OnPlayerExit();
	} else if (!this.containsPlayer && currentlyContainsPlayer) {
	    this.containsPlayer = true;
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
