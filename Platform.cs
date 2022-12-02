using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float width;
    public float height;
    
    // Start is called before the first frame update
    void Start()
    {
	// get dimensions
	this.width = this.GetComponent<SpriteRenderer>().sprite.rect.width / 100;
	this.height = this.GetComponent<SpriteRenderer>().sprite.rect.height / 100;
									     
	
	// collision
        this.gameObject.AddComponent<BoxCollider2D>();

	Rigidbody2D rb = this.gameObject.AddComponent<Rigidbody2D>();
	rb.gravityScale = 0.0f;
	rb.constraints = RigidbodyConstraints2D.FreezeAll;
	rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

	// add trigger to reset jumps
	GameObject jumpTriggerGo = new GameObject(this.name + "floor trigger");
	jumpTriggerGo.AddComponent<JumpTrigger>();
	jumpTriggerGo.AddComponent<BoxCollider2D>();
	jumpTriggerGo.GetComponent<BoxCollider2D>().isTrigger = true;
	jumpTriggerGo.transform.parent = this.gameObject.transform;
	jumpTriggerGo.transform.localScale = new Vector3(this.width, this.height * 0.1f, 1);
	jumpTriggerGo.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);	
    }
}
