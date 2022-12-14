using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{    
    public int cursorX;
    public int cursorY;

    public int inventoryWidth = 5;
    public int inventoryHeight = 3;

    // UI prefabs
    public Image inventoryBackgroundPrefab;
    public Image bigInventoryBackgroundPrefab;
    public Image itemImagePrefab;
    public Image cursorPrefab;
    public Image inventoryDescriptionBackgroundPrefab;
    public Text inventoryDescriptionTextPrefab;
    
    // related objects
    public Canvas canvas;
    public InventoryItem selectedItem;
    public Player player;    
    
    public void OpenInventory(InventoryItem newItem) {
	if (newItem != null) {
	    newItem.transform.SetParent(this.transform, false);
	    newItem.isSelected = true;
	    newItem.x = 0;
	    newItem.y = 0;
	    newItem.rotation = 0;

	    this.selectedItem = newItem;
	    this.cursorX = 0;
	    this.cursorY = 0;	    
	}
	this.RefreshInventory();
    }
	    
    public void RefreshInventory() {
	this.ClearView();
	
	// background box	
	Image background = Instantiate(this.inventoryBackgroundPrefab);
	background.transform.SetParent(this.canvas.transform, false);

	// description box
	Image descriptionBackground = Instantiate(this.inventoryDescriptionBackgroundPrefab);
	descriptionBackground.transform.SetParent(this.canvas.transform, false);
	Text descriptionText = Instantiate(this.inventoryDescriptionTextPrefab);
	descriptionText.transform.SetParent(this.canvas.transform, false);
	descriptionText.text = "";
	
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
	    itemRect.Rotate(0, 0, 90 * inventoryItem.rotation);	   
	    itemRect.anchoredPosition = new Vector2(-50 * this.inventoryWidth + inventoryItem.width * 50 + 100 * inventoryItem.x, -50 * this.inventoryHeight + inventoryItem.height * 50 + 100 * inventoryItem.y + 50);

	    // even i don't understand why this is needed
	    if (inventoryItem.rotation % 2 == 1) {
		itemRect.anchoredPosition += new Vector2(-50 * (inventoryItem.width - inventoryItem.height), 50 * (inventoryItem.width - inventoryItem.height));
	    }
	    
	    itemRect.sizeDelta = new Vector2(inventoryItem.width * 100, inventoryItem.height * 100);

	    // colorize for selection and out of bounds
	    if (inventoryItem.isSelected) {
		itemImage.color = new Color(1, 1, 1, 0.5f);
		descriptionText.text = this.player.inkStory.GetItemDescription(inventoryItem.pickUpableStartingKnot);
	    } else if (inventoryItem.isOutOfBounds) {
		itemImage.color = new Color(1, 0, 0, 0.5f);
	    }
	    
	}

	// add cursor
	Image cursorImage = Instantiate(this.cursorPrefab);
	cursorImage.transform.SetParent(this.canvas.transform, false);
	RectTransform cursorRect = cursorImage.GetComponent<RectTransform>();
	cursorRect.anchoredPosition = new Vector2(-50 * this.inventoryWidth + 50 + 100 * this.cursorX, -50 * this.inventoryHeight + 100 + 100 * cursorY);
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
	bool rotate = Input.GetKeyDown("space");

	// close
	if (closeInventory) {
	    this.CloseInventory();
	    return false;
	}	
	
	// move cursor
	bool didScreenChange = false;
	if (up && this.cursorY < this.inventoryHeight) {
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
	if (right && this.cursorX < this.inventoryWidth) {
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

	// rotate
	if (rotate && this.selectedItem != null) {
	    this.selectedItem.rotation += 1;
	    this.selectedItem.rotation %= 4;

	    // move rotated shape to cursor
	    int relativeCursorX = this.cursorX - this.selectedItem.x;
	    int relativeCursorY = this.cursorY - this.selectedItem.y;
	    int translatedCursorX = (this.selectedItem.rotation % 2 == 0 ? this.selectedItem.width : this.selectedItem.height) - 1 - relativeCursorY;
	    int translatedCursorY = relativeCursorX;
	    this.selectedItem.x += relativeCursorX - translatedCursorX;
	    this.selectedItem.y += relativeCursorY - translatedCursorY;
	    
	    didScreenChange = true;	    
	}

	// select and deselect
	if (selected) {
	    if (this.selectedItem == null || this.CanPlaceSelectedItem()) {
		InventoryItem itemAtCursor = this.GetItemAtPosition(this.cursorX, this.cursorY);
		if (this.selectedItem != null) {
		    this.selectedItem.isOutOfBounds = this.IsItemOutOfBounds(this.selectedItem);
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
	bool[][] itemShape = this.RotateItemShape(InventoryItem.ItemShapes[this.selectedItem.itemShapeName], selectedItem.rotation);
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
	// check whether each item is stowed correctly
	foreach (Transform child in this.transform) {	    
	    InventoryItem inventoryItem = child.GetComponent<InventoryItem>();

	    // try to place selected item
	    if (inventoryItem.isSelected) {

		// drop if it can't be placed
		if (!this.CanPlaceSelectedItem()) {
		    inventoryItem.markedForDrop = true;
		    inventoryItem.isSelected = false;
		    inventoryItem.dropPosition = this.player.transform.position;
		    continue;
		}

		// check if item is placed out of bounds
		inventoryItem.isSelected = false;
		inventoryItem.isOutOfBounds = this.IsItemOutOfBounds(inventoryItem);
	    }
	    
	    // drop items that are out of bounds
	    if (inventoryItem.isOutOfBounds) {
		inventoryItem.markedForDrop = true;
		inventoryItem.dropPosition = this.player.transform.position;
	    }
	}
	
	// remove UI elements
	this.selectedItem = null;
	this.ClearView();
    }

    private bool IsItemOutOfBounds(InventoryItem inventoryItem) {
	bool[][] itemShape = this.RotateItemShape(InventoryItem.ItemShapes[inventoryItem.itemShapeName], inventoryItem.rotation);
	for (int i = 0; i < itemShape.Length; i++) {
	    for (int j = 0; j < itemShape[i].Length; j++) {
		if (!itemShape[i][j]) {
		    continue;
		}
		int cellX = inventoryItem.x + j;
		int cellY = inventoryItem.y + i;
		// check if the item is hanging out of the inventory
		if (cellX < 0 || cellX > this.inventoryWidth - 1 || cellY < 0 || cellY > this.inventoryHeight - 1) {
		    return true;
		}
	    }
	}
	return false;
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
	    
	    bool[][] itemShape = this.RotateItemShape(InventoryItem.ItemShapes[inventoryItem.itemShapeName], inventoryItem.rotation);
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

    private bool[][] RotateItemShape(bool[][] itemShape, int rotation) {
	// rotation: 0, 1, 2, 3 -> 0, 90, 180, 270

	// create new empty array
	int iLength = (rotation == 0 || rotation == 2) ? itemShape.Length : itemShape[0].Length;
	int jLength = (rotation == 0 || rotation == 2) ? itemShape[0].Length : itemShape.Length;
	bool[][] newItemShape = new bool[iLength][];

	// there has to be a better way
	for (int i = 0; i < iLength; i++) {
	    newItemShape[i] = new bool[jLength];
	}
	
	// populate array
	for (int i = 0; i < itemShape.Length; i++) {
	    for (int j = 0; j < itemShape[i].Length; j++) {
		// this could be shorter, but writing out to keep it """readable"""
		if (rotation == 0) {
		    newItemShape[i][j] = itemShape[i][j];
		} else if (rotation == 1) {		    
		    newItemShape[j][jLength - 1 - i] = itemShape[i][j];
		} else if (rotation == 2) {
		    newItemShape[iLength - 1 - i][jLength - 1 - j] = itemShape[i][j];;
		} else if (rotation == 3) {
		    newItemShape[iLength - 1 - j][i] = itemShape[i][j];
		}
	    }
	}

	return newItemShape;
    }
    
    private void ClearView() {
	foreach (Transform child in this.canvas.transform) {
	    GameObject.Destroy(child.gameObject);
	}
    }
}











