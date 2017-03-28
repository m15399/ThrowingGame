using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFollower : MonoBehaviour {

	public bool preferX, preferY;

	void Start () {
		
	}
	
	void Update () {
		Vector2 playerPos = ThePlayer.player.transform.position;
		Vector2 pos = transform.position;
//		Vector2 dPos = playerPos - transform.position;

		bool moveX = true, moveY = true;


		if(preferX && preferY){
			Debug.LogError("Shouldn't prefer both directions");
		}
			
		if(Mathf.Approximately(pos.y, playerPos.y)){
			moveY = false;
		} else if (Mathf.Approximately(pos.x, playerPos.x)) {
			moveX = false;
		}

		if(moveX && moveY){
			if(preferX)
				moveY = false;
			if(preferY)
				moveX = false;
		}

		Vector2 newPos;
		float speed = 2.5f * Time.deltaTime;

		if(moveX && moveY){
			float speed2 = speed / 2;
			newPos = new Vector2(Mathf.MoveTowards(pos.x, playerPos.x, speed2), Mathf.MoveTowards(pos.y, playerPos.y, speed2));
		} else if (moveX) {
			newPos = new Vector2(Mathf.MoveTowards(pos.x, playerPos.x, speed), pos.y);
		} else if (moveY) {
			newPos = new Vector2(pos.x, Mathf.MoveTowards(pos.y, playerPos.y, speed));
		} else {
			newPos = new Vector2(pos.x, pos.y);
		}

		transform.position = newPos;
	}
}
