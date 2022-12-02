using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : Hittable
{
    // configurables
    public string startingKnot;

    // related objects
    private InkStory story;

    public override void Start() {
	base.Start();

	// get story
	this.story = GameObject.Find("/InkStory").GetComponent<InkStory>();
    }
    
    public override void GetHit(Player player) {
	player.isInDialogue = true;	
	this.story.OpenStory(this);
    }
}
