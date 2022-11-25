using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : Hittable
{
    // configurables
    public string startingKnot;

    // related objects
    private InkStory story;
    private Player player;

    public override void Start() {
	base.Start();

	// get story
	this.story = GameObject.Find("/InkStory").GetComponent<InkStory>();
	this.player = GameObject.Find("/player").GetComponent<Player>();
    }
    
    public override void GetHit() {
	this.player.isInDialogue = true;
	this.player.choiceIndex = 0;
	this.story.OpenStory(this.startingKnot);
    }
}
