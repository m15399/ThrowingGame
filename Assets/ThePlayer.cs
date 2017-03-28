using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlayer : MonoBehaviour {

	static ThePlayer _player = null;
	public static ThePlayer player {
		get {
			if(_player == null)
				_player = GameObject.Find("Player").GetComponent<ThePlayer>();
			return _player;
		}
	}

	public GameObject projectilePrefab;
	public GameObject meter;
	public GameObject sprite;

	float charge;

	float xv, yv;
	float moveDir = 0;
	float lean;
	bool canJump;

	void Start () {
		charge = 0;
		xv = 0;
		yv = 0;
		lean = 0;
	}
	
	void Update () {

		// Update moveDir
		//
		moveDir = 0;
		if(Input.GetKey("left"))
			moveDir -= 1;
		if(Input.GetKey("right"))
			moveDir += 1;
		if(canJump && Input.GetKeyDown("up"))
			yv = 8;

		// Move player by xv, yv
		//
		Vector2 moveVector = new Vector2(xv, yv) * Time.deltaTime;
		transform.position += (Vector3)moveVector;

		// Ground
		//
		if(transform.position.y <= 0){
			yv = 0;
			transform.position -= new Vector3(0, transform.position.y, 0);
			canJump = true;
		} else {
			canJump = false;
		}

		// Lean into movement
		//
		sprite.transform.rotation = Quaternion.Euler(0, 0, -lean);

		// Charge/Fire
		//
		if(Input.GetKey("space")){
			charge = Mathf.Clamp(charge, .1f, 1);
			charge += .8f * Time.deltaTime;
			charge = Mathf.Clamp(charge, .1f, 1);
		} else {
			if(charge > .1f){
				
				// Fire
				//
				Rigidbody2D o = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity).
					GetComponent<Rigidbody2D>();

				float uncharged = 1 - charge;
				float realCharge = 1 - Mathf.Pow(uncharged, 4f);

				float v = 14 * realCharge;
				o.velocity = new Vector2(xv * .2f + Mathf.Sin(Mathf.Deg2Rad * lean) * v * 2f, v);
			}
			charge = 0;
		}
			
		meter.transform.localScale = new Vector3(charge, meter.transform.localScale.y, 1);
	}

	void FixedUpdate(){

		// Update velocity
		//
		float acc = .35f;
		float fric = .95f;
		float maxSpeed = 5.5f;

		xv += moveDir * acc;
		xv = Mathf.Clamp(xv, -maxSpeed, maxSpeed);
		xv *= fric;

		yv += Time.fixedDeltaTime * Physics2D.gravity.y;

		// Update lean
		//
		if(moveDir != 0){
			lean += moveDir * 2.0f;
			lean *= .95f;
			float maxLean = 15;
			lean = Mathf.Clamp(lean, -maxLean, maxLean);
		} else {
			lean *= .86f;
		}
	}
}
