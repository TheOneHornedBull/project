﻿using UnityEngine;
using System.Collections;

public class consecutiveAmmo : MonoBehaviour {
	public GameObject ammo;
	public int count;
	public int maxCount = 30;
	public Quaternion additionalRotation = new Quaternion.Euler (0,0,1.5f);
	private float nextFire;
	private float fireRate;

	void FixedUpdate () {
		if (count <= maxCount && Time.time > nextFire){
			nextFire = Time.time + 0.1f;
			Instantiate(ammo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
			Instantiate(ammo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
			transform.rotation = transform.rotation * additionalRotation;
			count++;
		}
	}
}
