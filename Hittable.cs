using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Start()
    {
	// collision
        this.gameObject.AddComponent<BoxCollider2D>();

	Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
	rb.gravityScale = 0.0f;
	rb.constraints = RigidbodyConstraints2D.FreezeAll;
	rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	rb.sleepMode = RigidbodySleepMode2D.NeverSleep;        
    }

    // Ontriggerstay2d called when this collides with another BoxCollider2D w/ isTrigger=true
    void OnTriggerStay2D(Collider2D collider)
    {
	PlayerFist collidedPlayerFist = collider.gameObject.GetComponent<PlayerFist>();
	if (collidedPlayerFist != null && collidedPlayerFist.IsHitting() && !collidedPlayerFist.hasHit) {
	    collidedPlayerFist.hasHit = true;
	    this.GetHit();
	}
    }

    public virtual void GetHit() {
	Debug.Log("player hit: " + this.gameObject.name);
    }
}
