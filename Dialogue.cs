using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : Hittable
{
    // configurables
    public string startingKnot;
    private InkStory story;

    public override void Start() {
	base.Start();

	// get story
	this.story = GameObject.Find("/InkStory").GetComponent<InkStory>();
    }
    
    public override void GetHit() {
	this.story.OpenStory(this.startingKnot);
    }
}
