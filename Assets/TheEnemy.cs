using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEnemy : MonoBehaviour {

	public int health;
	public SpriteRenderer sprite;

	int startingHealth;
	bool invulnerable;

	void Start () {
		startingHealth = health;
		invulnerable = false;
	}
	
	void Update () {
		
		float alpha = 1;
		if(invulnerable){
			if((int)(Time.time * 20) % 2 == 0){
				alpha = .4f;
			}
		}

		Color c = sprite.color;
		c.a = alpha;
		sprite.color = c;
	}

	void TakeHit(){
//		health--;
		if(health == 0){
			Die();
		}

		invulnerable = true;
		Invoke("StopInvulnerability", .5f);
	}

	void Die(){
		GameObject.Destroy(gameObject);
	}

	void StopInvulnerability(){
		invulnerable = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(!invulnerable){
			TakeHit();
		}
	}
}
