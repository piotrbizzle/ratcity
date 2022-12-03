using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Description : Dialogue
{       
    public int isHighlighted;    
    
    // Start is called before the first frame update
    public override void Start() {
	base.Start();
	this.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void Update() {
	if (this.isHighlighted > 0) {
	    this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
	    this.isHighlighted -= 1;
	} else {
	    this.GetComponent<SpriteRenderer>().color = new Color(.7f, .7f, .7f, 1);
	}
    }
    
    public override void GetHit(Player player)
    {
	// do nothing on hit
    }

    public void OpenDescription(Player player) {	
	this.OpenDialogue(player);
    }   
}
