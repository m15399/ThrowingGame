using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheProjectile : MonoBehaviour {

	const int maxPower = 2;

	public SpriteRenderer sprite;
	public Color[] colorsPerPowerLevel;

	Rigidbody2D rb2d;
	SpriteRenderer spriteRenderer;

	int power;
	Vector2 addToV;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		addToV = new Vector2(0, 0);
		power = 0;
	}
	
	void Update () {

		// Add any bonus velocity from collision
		if(addToV.magnitude > 0){
//			Debug.Log("Adding " + addToV.y);
			rb2d.velocity += addToV;
			transform.position += (Vector3)(Time.fixedDeltaTime * addToV);
			addToV = new Vector2(0, 0);
		}

		// Delete when off screen
		if(transform.position.y < -50)
			GameObject.Destroy(gameObject);

		// Update color based on power
		sprite.color = colorsPerPowerLevel[power];
	}

	void FixedUpdate(){
		
		// Float on the way up
		if(rb2d.velocity.y > 0){
			rb2d.velocity += new Vector2(0, .02f);
		}
	}

	void OnCollisionEnter2D(Collision2D collision){

		// Top or bottom of collision?
		//
		if(collision.transform.position.y > transform.position.y){
//			GetComponent<BoxCollider2D>().enabled = false;

		} else {
			// Determine power of collision
			float v = -collision.relativeVelocity.y;
			if(v < 0)
				v = 0;
			if(v < .5f)
				v = v * v;

			// Bump up
			addToV = new Vector2(0, v/1.8f);

			// Increase projectil power
			if(power < maxPower)
				power++;
		}
	}
}
