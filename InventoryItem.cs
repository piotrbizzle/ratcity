using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {
    // configurables
    public Sprite pickUpableSprite;
    public string pickUpableStartingKnot;
    public Sprite itemSprite;
    public string itemShapeName;
    public bool isExplosive;
    public bool isRope;
    public bool isMetal;
    
    // positioning in menu
    public int width; 
    public int height;
    public int x;
    public int y;    
    public int rotation; // 0, 1, 2, 3 -> 0, 90, 180, 270
    public bool isSelected;
    public bool isOutOfBounds;
    public bool markedForDrop;
    public Vector3 dropPosition;

    // related objects
    public Player player;

    public void Start() {
	bool[][] itemShape = InventoryItem.ItemShapes[this.itemShapeName];
	this.height = itemShape.Length;
	this.width = itemShape[0].Length;

	// get player
	this.player = GameObject.Find("/player").GetComponent<Player>();
    }
    
    public void Update() {
	if (this.markedForDrop) {
	    this.BecomePickUpable(this.dropPosition);
	}
    }
    
    public void BecomePickUpable(Vector3 dropPosition) {
	GameObject pickUpableGo = new GameObject();
	this.transform.SetParent(pickUpableGo.transform, false);
	
	pickUpableGo.AddComponent<SpriteRenderer>().sprite = this.pickUpableSprite;
	pickUpableGo.GetComponent<SpriteRenderer>().sortingLayerName = "Items";
	pickUpableGo.AddComponent<PickUpable>().startingKnot = this.pickUpableStartingKnot;
	pickUpableGo.transform.position = dropPosition;

	// TODO: zones?
	
	this.markedForDrop = false;
    }    
    
    // registry for convenience / because this is not configurable in unity : (
    // NB: items are mirrored vertically
    //     (i know, i know, but it's cleaner to not flip everything in code...)
    public static Dictionary<string, bool[][]> ItemShapes = new Dictionary<string, bool[][]>(){
	{"DummyShape", new bool[][] {
		new bool[] {true, false},
		new bool[] {true, false},
		new bool[] {true, true}, 
	    }
	},
	{"DummyShape2", new bool[][] {
		new bool[] {true, true, false},
		new bool[] {false, true, true},
		new bool[] {false, true, false}, 
	    }
	},
	{"DummyShape3", new bool[][] {
		new bool[] {true, true, true},
	    }
	},
    };
}
