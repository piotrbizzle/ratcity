using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingNode : MonoBehaviour
{
    public bool isGrabbable;
    public bool isZipline;
    
    // grabbable nodes must have EITHER left or right
    public ClimbingNode left;
    public ClimbingNode right;

    // Start is called before the first frame update
    void Start()
    {
	// Destroy(this.GetComponent<SpriteRenderer>());
	this.gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
    }

}
