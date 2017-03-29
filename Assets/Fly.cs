using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

	public float xv;

	void Start () {
		
	}
	
	void Update () {
		Vector3 pos = transform.position;
		pos.x += xv * Time.deltaTime;

		float screenSize = 18;
		if(pos.x < -screenSize)
			pos.x += screenSize * 2;
		if(pos.x > screenSize)
			pos.x -= screenSize * 2;

		transform.position = pos;
	}
}
