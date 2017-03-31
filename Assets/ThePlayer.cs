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

		// Position
		//
		Vector3 position = transform.position;

		// Move player by xv, yv
		position += new Vector3(xv, yv, 0) * Time.deltaTime;

		// Ground collision
		if(position.y <= 0){
			yv = 0;
			position.y = 0;
			canJump = true;
		} else {
			canJump = false;
		}

		// Clamp to screen
		float screenSize = 16;
		if(position.x < -screenSize){
			position.x = -screenSize;
			xv = 0;
		} else if(position.x > screenSize){
			position.x = screenSize;
			xv = 0;
		}

		transform.position = position;


		// Update sprite lean
		//
		sprite.transform.rotation = Quaternion.Euler(0, 0, -lean);

		// Charge/Fire
		//
		float minCharge = .1f;
		float baseChargeRate = minCharge / .4f;

		if(Input.GetKey("space")){
			if(charge > minCharge - .01f){
				charge += 1.6f * Time.deltaTime;
				charge = Mathf.Clamp(charge, 0, 1);
			} else {
				// "Reloading" - can't speed this part up
				charge += baseChargeRate * Time.deltaTime;
			}
		
		} else {
			if(charge > minCharge){
				
				// Fire
				//
				Rigidbody2D projectile = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity).
					GetComponent<Rigidbody2D>();
				
				// Bonus at lower charges for better feel
				float realCharge = 1 - Mathf.Pow(1 - charge, 1.5f);

				float maxPower = 12.5f;
				float yPower = maxPower * realCharge;
				float xPower = xv * .2f + Mathf.Sin(Mathf.Deg2Rad * lean) * yPower * 1.9f;

				projectile.velocity = new Vector2(xPower, yPower);
				charge = 0;
			}

			// Charge up to minCharge slowly
			charge += baseChargeRate * Time.deltaTime;
			charge = Mathf.Clamp(charge, 0, minCharge);
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
		float maxSpeed = 6.0f;

		xv *= fric;
		xv += inputDir * acc;
		xv = Mathf.Clamp(xv, -maxSpeed, maxSpeed);
//		Debug.Log(xv);

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
