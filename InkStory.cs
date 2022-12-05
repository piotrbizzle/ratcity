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
    public Dialogue currentDialogue;

    public int choiceIndex;
    
    public void Start() {
	this.story = new Story(inkJSONAsset.text);
    }

    public void OpenStory(Dialogue currentDialogue) {
	this.currentDialogue = currentDialogue;
	this.story.ChoosePathString(this.currentDialogue.startingKnot);
	this.choiceIndex = 0;
	this.ContinueAndRefreshView();
    }

    public void ControlMenu() {
    	// get inputs
	bool up = Input.GetKeyDown("w");
	bool down = Input.GetKeyDown("s");
	bool choose = Input.GetKeyDown("j");

	if (choose) {
	    this.Choose(this.choiceIndex);
	    this.choiceIndex = 0;
	    return;
	}

	if (up && !down && this.choiceIndex > 0) {
	    this.choiceIndex -= 1;
	    this.RefreshView(this.choiceIndex);
	} else if (down && !up && this.choiceIndex < this.story.currentChoices.Count - 1) {
	    this.choiceIndex += 1;
	    this.RefreshView(this.choiceIndex);
	}
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

	itemNames = new InkList("all_items", this.story);
	foreach (Transform child in this.currentDialogue.transform) {
	    itemNames.AddItem(child.gameObject.name);
	}
	this.story.variablesState["dialogue_inventory"] = itemNames;
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
	if (this.ProcessTags()) {
	    return;
	}
	this.RefreshView(0);
    }

    private bool ProcessTags() {
	// returns true if should exit early
	for (int i = 0; i < story.currentTags.Count; i++) {
	    string tag = story.currentTags[i].Trim();

	    // give item
	    if (tag.StartsWith("give_")) {
		string itemName = tag.Substring(5);
		foreach (Transform child in this.currentDialogue.transform) {
		    if (child.gameObject.name == itemName) {
			InventoryItem inventoryItem = child.GetComponent<InventoryItem>();
			// inventoryItem.markedForDrop = true;
			// inventoryItem.dropPosition = this.player.transform.position;
			// open player inventory immediately
			this.player.isInDialogue = false;
			this.player.isInInventory = true;
			player.inventory.OpenInventory(inventoryItem);
			return true;
		    }
		}
	    }
	    
	    // take item
	    if (tag.StartsWith("take_")) {
		string itemName = tag.Substring(5);
		foreach (Transform child in this.player.inventory.transform) {
		    if (child.gameObject.name == itemName) {
			child.transform.SetParent(this.currentDialogue.transform, false);
			break;
		    }
		}	
	    }

	    // grant dark vision
	    if (tag == "grant_darkVision") {
		this.player.hasDarkVision = true;
	    }

	    // grant zipline
	    if (tag == "grant_zipline") {
		this.player.hasZipline = true;
	    }

	    // grant anti metal detector
	    if (tag == "grant_antiMetalDetector") {
		this.player.hasAntiMetalDetector = true;
	    }

	    // grant architect
	    if (tag == "grant_architect") {
		this.player.hasArchitect = true;
	    }

	    // grant bootlegger
	    if (tag == "grant_bootlegger") {
		this.player.hasBootlegger = true;
	    }

	    // upgrade inventory
	    if (tag == "upgrade_inventory") {
		this.player.inventory.inventoryBackgroundPrefab = this.player.inventory.bigInventoryBackgroundPrefab;
		this.player.inventory.inventoryWidth = 6;
		this.player.inventory.inventoryHeight = 4;
	    }
	}
	return false;
    }
    
    private void ClearView() {
	foreach (Transform child in this.canvas.transform) {
	    GameObject.Destroy(child.gameObject);
	}
    }
}
