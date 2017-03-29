using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlayer : MonoBehaviour {

	// Player instance
	//
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
	float inputDir = 0;
	float lean;
	bool canJump;

	void Start () {
		charge = 0;
		xv = 0;
		yv = 0;
		lean = 0;
	}
	
	void Update () {

		// Input
		//
		inputDir = 0;
		if(Input.GetKey("left"))
			inputDir -= 1;
		if(Input.GetKey("right"))
			inputDir += 1;
		if(canJump && Input.GetKeyDown("up"))
			yv = 8;

		// Move player by xv, yv
		//
		transform.position += new Vector3(xv, yv, 0) * Time.deltaTime;

		// Ground collision
		//
		if(transform.position.y <= 0){
			yv = 0;
			transform.position -= new Vector3(0, transform.position.y, 0);
			canJump = true;
		} else {
			canJump = false;
		}

		// Update sprite lean
		//
		sprite.transform.rotation = Quaternion.Euler(0, 0, -lean);

		// Charge/Fire
		//
		float minCharge = .1f;
		if(Input.GetKey("space")){
			charge = Mathf.Clamp(charge, minCharge, 1);
			charge += 1.6f * Time.deltaTime;
			charge = Mathf.Clamp(charge, minCharge, 1);

		} else {
			if(charge > .1f){
				
				// Fire
				//
				Rigidbody2D o = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity).
					GetComponent<Rigidbody2D>();

				float realCharge = 1 - Mathf.Pow(1 - charge, 1.5f);
				float v = 13 * realCharge;

				o.velocity = new Vector2(xv * .2f + Mathf.Sin(Mathf.Deg2Rad * lean) * v * 2f, v);
			}
			charge = 0;
		}

		// Update charge meter size
		//
		meter.transform.localScale = new Vector3(charge, meter.transform.localScale.y, 1);
	}

	void FixedUpdate(){

		// Update velocities
		//
		float acc = .35f;
		float fric = .95f;
		float maxSpeed = 5.5f;

		xv *= fric;
		xv += inputDir * acc;
		xv = Mathf.Clamp(xv, -maxSpeed, maxSpeed);

		yv += Time.fixedDeltaTime * Physics2D.gravity.y;

		// Update lean
		//
		if(inputDir != 0){
			lean *= .95f;
			lean += inputDir * 2.0f;
			float maxLean = 15;
			lean = Mathf.Clamp(lean, -maxLean, maxLean);
		} else {
			lean *= .86f;
		}
	}
}
