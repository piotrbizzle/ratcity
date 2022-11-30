using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class InkStory : MonoBehaviour {
    public TextAsset inkJSONAsset;
    public Story story;

    // UI Prefabs
    public Image dialogueBoxPrefab;
    public GameObject dialogueChoiceGroupPrefab;
    public Text dialogueChoiceTextPrefab;

    // related objects
    public Player player;
    public Canvas canvas;
    
    public void Start() {
	this.story = new Story(inkJSONAsset.text);
    }

    public void OpenStory(string startingKnot) {
	this.story.ChoosePathString(startingKnot);
	this.ContinueAndRefreshView();
    }

    public void Choose(int choiceIndex) {
	this.story.ChooseChoiceIndex(choiceIndex);

	// continue past choice text
	this.story.Continue(); 

	this.ContinueAndRefreshView();
    }
    
    public void RefreshView(int choiceIndex) {
	this.ClearView();
	this.UpdateInventory();
	
	// dialogue box	
	Image dialogueBox = Instantiate(this.dialogueBoxPrefab);
	dialogueBox.transform.SetParent(this.canvas.transform, false);

	// dialogue group
	GameObject dialogueTextGroup = Instantiate(this.dialogueChoiceGroupPrefab);
	dialogueTextGroup.transform.SetParent(this.canvas.transform, false);

	// character dialogue text
	dialogueTextGroup.transform.GetChild(0).GetComponent<Text>().text = this.story.currentText.Trim();
	
	// choices
	Transform choicePointerGroup = dialogueTextGroup.transform.GetChild(1);
	for (int i = 0; i < this.story.currentChoices.Count; i++) {
	    Text dialogueChoiceText = Instantiate(this.dialogueChoiceTextPrefab);
	    if (i == choiceIndex) {
		dialogueChoiceText.transform.SetParent(choicePointerGroup, false);
		choicePointerGroup.SetAsLastSibling();
	    } else {
		dialogueChoiceText.transform.SetParent(choicePointerGroup.parent, false);
	    }
	    Choice choice = this.story.currentChoices[i];
	    dialogueChoiceText.text = choice.text.Trim();
	}	
    }

    private void UpdateInventory() {
	InkList itemNames = new InkList("all_items", this.story);
	foreach (Transform child in this.player.inventory.transform) {
	    itemNames.AddItem(child.gameObject.name);
	}
	this.story.variablesState["player_inventory"] = itemNames;	
    }
    
    private void ContinueAndRefreshView() {
	// exit out at end of dialogue
	if (!this.story.canContinue) {
	    this.player.isInDialogue = false;
	    this.ClearView();
	    return;
	}
	
	this.UpdateInventory();	
	this.story.Continue();
	this.ProcessTags();
	this.RefreshView(0);
    }

    private void ProcessTags() {
	for (int i = 0; i < story.currentTags.Count; i++) {
	    string tag = story.currentTags[i].Trim();

	    // give item
	    if (tag.StartsWith("give_")) {
		Debug.Log("give!");
	    }
	}
    }
    
    private void ClearView() {
	foreach (Transform child in this.canvas.transform) {
	    GameObject.Destroy(child.gameObject);
	}
    }
}
