using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : Hittable
{
    // configurables
    public string startingKnot;
    public Sprite portraitSprite;    
    public Sprite modifiedPortraitSprite;
    public Sprite modifiedCharacterSprite;

    // related objects
    private InkStory story;

    public override void Start() {
	base.Start();

	// get story
	this.story = GameObject.Find("/InkStory").GetComponent<InkStory>();
    }
    
    public override void GetHit(Player player) {
	this.OpenDialogue(player);
    }

    public void OpenDialogue(Player player) {
	player.isInDialogue = true;	
	this.story.OpenStory(this);
    }
}
