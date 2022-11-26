using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{    
    // UI prefabs
    public Image inventoryBackgroundPrefab;    
    public Image itemImagePrefab;
    
    // related objets
    public Canvas canvas;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory() {
	// dialogue box	
	Image background = Instantiate(this.inventoryBackgroundPrefab);
	background.transform.SetParent(this.canvas.transform, false);

	// add items
	foreach (Transform child in this.transform) {
	    InventoryItem inventoryItem = child.GetComponent<InventoryItem>();

	    // add item image
	    Image itemImage = Instantiate(this.itemImagePrefab);
	    itemImage.transform.SetParent(this.canvas.transform, false);
	    itemImage.gameObject.name = child.gameObject.name;
	    itemImage.sprite = inventoryItem.itemSprite;		
	    
	    // position and scale
	    RectTransform itemRect = itemImage.GetComponent<RectTransform>();
	    itemRect.anchoredPosition = new Vector2(-300 + inventoryItem.widthPx / 2 + 100 * inventoryItem.x, -200 + inventoryItem.heightPx / 2 + 100 * inventoryItem.y);
	    itemRect.sizeDelta = new Vector2(inventoryItem.widthPx, inventoryItem.heightPx);
	}
    }

    public void CloseInventory() {
	// check whether items are stowed correctly
	foreach (Transform child in this.transform) {
	    InventoryItem inventoryItem = child.GetComponent<InventoryItem>();
	    bool[][] itemShape = InventoryItem.ItemShapes[inventoryItem.itemShapeName];
	    bool outOfBounds = false;
	    for (int i = 0; i < itemShape.Length; i++) {
		for (int j = 0; j < itemShape[i].Length; j++) {
		    if (!itemShape[i][j]) {
			continue;
		    }
		    int cellX = inventoryItem.x + j;
		    int cellY = inventoryItem.y + i;
		    // check if the item is hanging out of the inventory
		    // ASSUMES inventory is 6x4
		    if (cellX < 0 || cellX > 5 || cellY < 0 || cellY > 3) {
			outOfBounds = true;
		    }
		}
	    }
	    if (outOfBounds) {
		Debug.Log("Hanging out: " + child.gameObject.name);
	    } else {
		Debug.Log("All good: " + child.gameObject.name);
	    }
	}
	
	// remove UI elements
	this.ClearView();
    }

    private void ClearView() {
	foreach (Transform child in this.canvas.transform) {
	    GameObject.Destroy(child.gameObject);
	}
    }
}
