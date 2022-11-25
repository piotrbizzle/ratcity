using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScurryZone : Zone
{
    public override void OnPlayerEnter() {
	this.player.forcedToScurry = true;       
    }

    public override void OnPlayerExit() {
	this.player.forcedToScurry = false;
    }
}
