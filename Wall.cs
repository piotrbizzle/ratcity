using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	// if wall is not explicitly a level boundary, put it at wall level
	if (this.GetComponent<SpriteRenderer>().sortingLayerName != "LevelBoundary" && this.GetComponent<MetalDetectorZone>() == null) {
	    this.GetComponent<SpriteRenderer>().sortingLayerName = "Walls";
	}

	// collision
        this.gameObject.AddComponent<BoxCollider2D>();

	Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
	rb.gravityScale = 0.0f;
	rb.constraints = RigidbodyConstraints2D.FreezeAll;
	rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	rb.sleepMode = RigidbodySleepMode2D.NeverSleep;       
    }
}
