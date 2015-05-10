﻿using UnityEngine;
using System.Collections;

public class consecutiveAmmo : MonoBehaviour {
	public GameObject ammo;
	public int maxCount = 25;
	public bool attackLeft = false;
	public bool attackRight = false;
	public float fireRate = 0.05f;

	void Start () {
		StartCoroutine(attack(fireRate, maxCount));
	}
	
	IEnumerator attack(float fireRate, int count){
		for (int i=0; i<= count - 15; i++) {
			Instantiate(ammo, transform.position - new Vector3 (2.7f, 1,0), transform.rotation);
			Instantiate(ammo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
			if (attackLeft){
				transform.rotation = transform.rotation * Quaternion.Euler(0,0,3);
			}else if (attackRight){
				transform.rotation = transform.rotation * Quaternion.Euler(0,0,-3);
			}
			yield return new WaitForSeconds (fireRate);
		}
		transform.rotation = Quaternion.Euler (0,0,0);
		yield return new WaitForSeconds ((fireRate * (count-15)) + 1 );
		for (int i=0; i <= count; i++) {
			Instantiate(ammo, transform.position - new Vector3 (2.7f, 1,0), transform.rotation);
			Instantiate(ammo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
			if (attackLeft){
				transform.rotation = transform.rotation * Quaternion.Euler(0,0,1f);
			}else if (attackRight){
				transform.rotation = transform.rotation * Quaternion.Euler(0,0,-1f);
			}
			yield return new WaitForSeconds (fireRate);
		}
		StopCoroutine (attack(fireRate, maxCount));
	}

}	
