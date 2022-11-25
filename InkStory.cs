using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class InkStory : MonoBehaviour {
    public TextAsset inkJSONAsset;
    public Canvas canvas;
    private Story story;

    // UI Prefabs
    public Image dialogueBoxPrefab;
    public GameObject dialogueChoiceGroupPrefab;
    public Text dialogueChoiceTextPrefab;
    
    public void Start() {
	this.story = new Story(inkJSONAsset.text);
    }

    public void OpenStory(string startingKnot) {
	this.story.ChoosePathString(startingKnot);
	this.RefreshView();
    }

    private void RefreshView() {
	this.ClearView();
	
	// dialogue box	
	Image dialogueBox = Instantiate(this.dialogueBoxPrefab);
	dialogueBox.transform.SetParent(this.canvas.transform, false);

	// dialogue group
	GameObject dialogueTextGroup = Instantiate(this.dialogueChoiceGroupPrefab);
	dialogueTextGroup.transform.SetParent(this.canvas.transform, false);

	// character dialogue text
	dialogueTextGroup.transform.GetChild(0).GetComponent<Text>().text = this.story.Continue().Trim();
	
	// choices
	Transform choicePointerGroup = dialogueTextGroup.transform.GetChild(1);
	for (int i = 0; i < this.story.currentChoices.Count; i++) {
	    Text dialogueChoiceText = Instantiate(this.dialogueChoiceTextPrefab);
	    if (i == 0) {
		dialogueChoiceText.transform.SetParent(choicePointerGroup);
	    } else {
		dialogueChoiceText.transform.SetParent(choicePointerGroup.transform.parent);
	    }
	    Choice choice = this.story.currentChoices[i];
	    dialogueChoiceText.text = choice.text.Trim();
	}	
    }

    private void ClearView() {
	foreach (Transform child in this.transform) {
	    GameObject.Destroy(child.gameObject);
	}
    }
}
