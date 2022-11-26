using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {
    public Sprite pickUpableSprite;
    public Sprite itemSprite;
    public int itemRotation;
    public string itemShapeName;
    public float widthPx;
    public float heightPx;

    // positioning in menu
    public int x;
    public int y;
    public float rotation;
    
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
    };
}
