using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingNode : Hittable
{
    // configurable
    public bool isGrabbable;
    public bool isZipline;
    public LineRenderer ziplineLine;
    public bool isActivated;
    
    // grabbable nodes must have EITHER left or right    
    public ClimbingNode left;
    public ClimbingNode right;


    // Start is called before the first frame update
    public override void Start()
    {
	base.Start();
	
	this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
	if (this.isZipline && this.isGrabbable && !this.isActivated) {
	    this.ziplineLine.gameObject.SetActive(false);
	}
    }

    public override void GetHit(Player player) {
	if (!this.isZipline || this.isActivated || !this.isGrabbable) {
	    return;
	}
	foreach (Transform child in player.inventory.transform) {
	    InventoryItem inventoryItem = child.GetComponent<InventoryItem>();
	    if (inventoryItem.isRope) {
		GameObject.Destroy(inventoryItem.gameObject);
		this.Activate();
		break;
	    }
	}
    }

    public void Activate() {
	// TODO: better visual effect
	this.ziplineLine.gameObject.SetActive(true);
	this.isActivated = true;
    }
}
