using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{    
    public int cursorX;
    public int cursorY;

    // UI prefabs
    public Image inventoryBackgroundPrefab;    
    public Image itemImagePrefab;
    public Image cursorPrefab;
    
    // related objects
    public Canvas canvas;
    public InventoryItem selectedItem;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory() {
	this.RefreshInventory();
    }
	    
    public void RefreshInventory() {
	this.ClearView();
	
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

	    // make transparent if selected
	    if (inventoryItem.isSelected) {
		itemImage.color = new Color(1, 1, 1, 0.5f);
	    }
	}

	// add cursor
	Image cursorImage = Instantiate(this.cursorPrefab);
	cursorImage.transform.SetParent(this.canvas.transform, false);
	RectTransform cursorRect = cursorImage.GetComponent<RectTransform>();
	cursorRect.anchoredPosition = new Vector2(-250 + 100 * this.cursorX, -150 + 100 * cursorY);
    }

    // returns whether menu should remain open
    public bool ControlMenu() {	
	// inputs
	bool closeInventory = Input.GetKeyDown("i");
	bool up = Input.GetKeyDown("w");
	bool down = Input.GetKeyDown("s");
	bool left = Input.GetKeyDown("a");
	bool right = Input.GetKeyDown("d");
	bool selected = Input.GetKeyDown("j");

	// close
	if (closeInventory) {
	    this.CloseInventory();
	    return false;
	}	
	
	// move cursor
	// ASSUMES 6x4 inventory. cursor can go 1 space out of bounds
	bool didScreenChange = false;
	if (up && this.cursorY < 4) {
	    this.cursorY += 1;
	    if (this.selectedItem != null) {
		this.selectedItem.y += 1;
	    }
	    didScreenChange = true;	    
	}
	if (down && this.cursorY > -1) {
	    this.cursorY -= 1;
	    if (this.selectedItem != null) {
		this.selectedItem.y -= 1;
	    }
	    didScreenChange = true;
	}
	if (right && this.cursorX < 6) {
	    this.cursorX += 1;
	    if (this.selectedItem != null) {
		this.selectedItem.x += 1;
	    }
	    didScreenChange = true;	    
	}
	if (left && this.cursorX > -1) {
	    this.cursorX -= 1;
	    if (this.selectedItem != null) {
		this.selectedItem.x -= 1;
	    }
	    didScreenChange = true;
	}

	// select and deselect
	if (selected) {
	    if (this.selectedItem == null || this.CanPlaceSelectedItem()) {
		InventoryItem itemAtCursor = this.GetItemAtPosition(this.cursorX, this.cursorY);
		if (this.selectedItem != null) {
		    this.selectedItem.isSelected = false;
		}
		if (itemAtCursor != null) {
		    itemAtCursor.isSelected = true;
		}
		this.selectedItem = itemAtCursor;
		didScreenChange = true; // (probably)
	    }	
	}

	// last steps
	if (didScreenChange) {
	    this.RefreshInventory();
	}
	return true;
    }

    private bool CanPlaceSelectedItem() {
	// ASSUMES this.selectedItem != null
	
	// TODO: rotate item shape @_@
	bool[][] itemShape = InventoryItem.ItemShapes[this.selectedItem.itemShapeName];
	for (int i = 0; i < itemShape.Length; i++) {
	    for (int j = 0; j < itemShape[i].Length; j++) {
		if (!itemShape[i][j]) {
		    continue;
		}
		int cellX = this.selectedItem.x + j;
		int cellY = this.selectedItem.y + i;

		if (this.GetItemAtPosition(cellX, cellY) != null) {
		    return false;
		}
	    }
	}
	return true;
    }
	
    public void CloseInventory() {
	// check whether items are stowed correctly
	foreach (Transform child in this.transform) {	    
	    InventoryItem inventoryItem = child.GetComponent<InventoryItem>();

	    // TODO: rotate item shape @_@
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

    private InventoryItem GetItemAtPosition(int x, int y) {
	// this code is inefficient but it probably doesn't matter since inventory is small
	// TODO: store a grid in this object for fast checks if it does matter >.>
	foreach (Transform child in this.transform) {	    
	    InventoryItem inventoryItem = child.GetComponent<InventoryItem>();

	    // skip selected item, it isn't "in" the inventory
	    if (inventoryItem.isSelected) {
		continue;
	    }
	    
	    // TODO: rotate item shape @_@
	    bool[][] itemShape = InventoryItem.ItemShapes[inventoryItem.itemShapeName];
	    for (int i = 0; i < itemShape.Length; i++) {
		for (int j = 0; j < itemShape[i].Length; j++) {
		    if (!itemShape[i][j]) {
			continue;
		    }
		    int cellX = inventoryItem.x + j;
		    int cellY = inventoryItem.y + i;

		    if (cellX == x && cellY == y) {
			return inventoryItem;
		    }		    
		}
	    }
	}
	return null;
    }
    
    private void ClearView() {
	foreach (Transform child in this.canvas.transform) {
	    GameObject.Destroy(child.gameObject);
	}
    }
}
