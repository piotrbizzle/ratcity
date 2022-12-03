using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : Description
{
    // nomenclature: PickUpables become "items" when they are picked up
    public override void Start() {
	base.Start();

	// init from child inventory item
	InventoryItem inventoryItem = this.transform.GetChild(0).GetComponent<InventoryItem>();
	
	this.GetComponent<SpriteRenderer>().sprite = inventoryItem.pickUpableSprite;
	this.GetComponent<SpriteRenderer>().sortingLayerName = "Items";
	this.startingKnot = inventoryItem.pickUpableStartingKnot;
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
