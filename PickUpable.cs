using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : Hittable
{
    // nomenclature: PickUpables become "items" when they are picked up
    public override void Start() {
	base.Start();
	this.GetComponent<BoxCollider2D>().isTrigger = true;
    }
    
    public override void GetHit(Player player) {
	if (!player.mayAccessInventory) {
	    return;
	}
	player.isInInventory = true;

	// assumes inventory item is child
	player.inventory.OpenInventory(this.transform.GetChild(0).GetComponent<InventoryItem>());
	GameObject.Destroy(this.gameObject);
    }
}
