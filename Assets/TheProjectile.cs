using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheProjectile : MonoBehaviour {

	Rigidbody2D rb2d;

	Vector2 addToV;

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		addToV = new Vector2(0, 0);
	}
	
	void Update () {
		if(addToV.magnitude > 0){
//			Debug.Log("Adding " + addToV.y);
		}

		rb2d.velocity += addToV;
		transform.position += (Vector3)(Time.fixedDeltaTime * addToV);
		addToV = new Vector2(0, 0);

		if(transform.position.y < -50)
			GameObject.Destroy(gameObject);

	}

	void FixedUpdate(){
		// Float on the way up
		//
		if(rb2d.velocity.y > 0){
			rb2d.velocity += new Vector2(0, .02f);
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.transform.position.y > transform.position.y){
//			GetComponent<BoxCollider2D>().enabled = false;
		} else {
//			Debug.Log(collision.relativeVelocity);
			float v = -collision.relativeVelocity.y;
			if(v < 0)
				v = 0;
			if(v < .5f)
				v = v * v;
			
			addToV = new Vector2(0, v/1.8f);
		}
	}
}
