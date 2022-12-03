using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : Hittable
{
    public override void GetHit(Player player) {
	foreach (Transform child in player.inventory.transform) {
	    InventoryItem inventoryItem = child.GetComponent<InventoryItem>();
	    if (inventoryItem.isExplosive) {
		GameObject.Destroy(inventoryItem.gameObject);
		this.Explode();
		break;
	    }
	}
    }

    public void Explode() {
	// TODO: some visual effect plz
	GameObject.Destroy(this.gameObject);
    }
}
