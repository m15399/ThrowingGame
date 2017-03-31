using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheProjectile : MonoBehaviour {

	const int maxDamageLevel = 2;

	public SpriteRenderer sprite;
	public Color[] colorsPerDamageLevel;

	Rigidbody2D rb2d;
	SpriteRenderer spriteRenderer;

	int damageLevel;
	Vector2 bonusV;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		bonusV = new Vector2(0, 0);
		damageLevel = 0;

		if(maxDamageLevel != colorsPerDamageLevel.Length - 1){
			Debug.LogError("maxDamageLevel and colorsPerDamageLevel not the same size");
		}
	}

	void AddBonusVelocity(){
		if(bonusV.magnitude > 0){
//			Debug.Log("Adding " + addToV);
			rb2d.velocity += bonusV;
			transform.position += (Vector3)(Time.fixedDeltaTime * bonusV);
			bonusV = new Vector2(0, 0);
		}
	}
	
	void Update () {
		
		AddBonusVelocity();

		// Delete when off screen
		if(transform.position.y < -50)
			GameObject.Destroy(gameObject);

		// Update color based on power
		sprite.color = colorsPerDamageLevel[damageLevel];
	}

	void FixedUpdate(){

		AddBonusVelocity();
		
		// Float on the way up
		if(rb2d.velocity.y > 0){
			rb2d.velocity += new Vector2(0, .01f);
		}
	}

	void OnCollisionEnter2D(Collision2D collision){

		// Top or bottom of collision?
		//
		if(collision.transform.position.y > transform.position.y){
//			GetComponent<BoxCollider2D>().enabled = false;

		} else {
			bonusV = new Vector2(0, 0);

			// Push sideways
			float hv = collision.relativeVelocity.x;
			bonusV += new Vector2(-hv * .18f, 0);

			// Determine vertical power of collision
			float v = -collision.relativeVelocity.y;
			if(v < 0)
				v = 0;

			// Small bonus for throwing sideways?
//			v += Mathf.Abs(collision.relativeVelocity.x) * .2f;

			// Reduce strength of higher velocities (better feel for average strength)
			float maxTheoreticalV = 120;
			float powerAtMaxV = 0;
			float reducedV = v * Mathf.Lerp(1, powerAtMaxV, v / maxTheoreticalV);
			v = reducedV;
//			Debug.Log(v + " -> " + reducedV);

			// Bump up
			bonusV += new Vector2(0, v * .737f);

			// Increase projectile damage when bumped
			if(damageLevel < maxDamageLevel)
				damageLevel++;
		}
	}
}
