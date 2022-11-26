using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : Hittable
{
    // nomenclature: PickUpables become "items" when they are picked up
    InventoryItem inventoryItem;    

    public override void GetHit(Player player) {
	// Player.OpenInventoryScreen(this);
    }
}
