using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : Hittable
{
    // nomenclature: PickUpables become "items" when they are picked up
    public InventoryItem inventoryItemPrefab;    

    public override void Start() {
	base.Start();
	this.GetComponent<BoxCollider2D>().isTrigger = true;
    }
    
    public override void GetHit(Player player) {
	player.isInInventory = true;
	player.inventory.OpenInventory(this.inventoryItemPrefab);
	GameObject.Destroy(this.gameObject);
    }
}
