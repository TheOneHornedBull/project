﻿using UnityEngine;
using System.Collections;

public class consecutiveAmmo : MonoBehaviour {
	public GameObject ammo;
	public int maxCount = 25;
	public bool attackLeft = false;
	public bool attackRight = false;
	public float fireRate = 0.05f;
	public GameObject leftGun;
    public GameObject rightGun;
    Quaternion leftDefRot;
    Quaternion rightDefRot;
    

	void Start () {
		StartCoroutine(attack(fireRate, maxCount));
		leftGun = GameObject.Find ("leftGun");
		rightGun = GameObject.Find ("rightGun");
	}
	
	IEnumerator attack(float fireRate, int count){
		transform.rotation = Quaternion.Euler (0,0,0);
            // leftDefRot = leftGun.transform.rotation;
            //  rightDefRot = rightGun.transform.rotation;
		yield return new WaitForSeconds (1);
		for (int i=0; i<= count - 20; i++) {
            Instantiate(ammo, transform.position - new Vector3(2.7f, -1, 0)  /**leftGun.transform.position*/, transform.rotation);
            Instantiate(ammo, transform.position + new Vector3(2.7f, 1, -0.4f)  /**rightGun.transform.position*/, transform.rotation);
			if (attackLeft){
			// leftGun.transform.rotation = transform.rotation * Quaternion.Euler(0,0,5);
			// rightGun.transform.rotation = transform.rotation * Quaternion.Euler(0,0,5);
				transform.rotation = transform.rotation * Quaternion.Euler(0,0,5);
			}else if (attackRight){
			// leftGun.transform.rotation = transform.rotation * Quaternion.Euler(0,0,-5);
			// rightGun.transform.rotation = transform.rotation * Quaternion.Euler(0,0,-5);
				transform.rotation = transform.rotation * Quaternion.Euler(0,0,-5);
			}
			yield return new WaitForSeconds (fireRate + 0.1f);
		}
		transform.rotation = Quaternion.Euler (0,0,0);
            //  leftGun.transform.rotation = leftDefRot;
            //  rightGun.transform.rotation =  rightDefRot;
		yield return new WaitForSeconds ((fireRate * (count-15)) + 1 );
		for (int i=0; i <= count; i++) {
			Instantiate(ammo,transform.position - new Vector3 (2.7f, -1, 0)/** leftGun.transform.position */, transform.rotation);
            Instantiate(ammo, transform.position + new Vector3(2.7f, 1, -0.4f) /**rightGun.transform.position */, transform.rotation);
			if (attackLeft){
			//	leftGun.transform.rotation = leftGun.transform.rotation * Quaternion.Euler(0,0,1f);
			//	rightGun.transform.rotation = rightGun.transform.rotation * Quaternion.Euler(0,0,1f);
				transform.rotation = transform.rotation * Quaternion.Euler(0,0,1f);
			}else if (attackRight){
			//	leftGun.transform.rotation = leftGun.transform.rotation * Quaternion.Euler(0,0,-1f);
			//	rightGun.transform.rotation = rightGun.transform.rotation * Quaternion.Euler(0,0,-1f);
				transform.rotation = transform.rotation * Quaternion.Euler(0,0,-1f);
			}
			yield return new WaitForSeconds (fireRate);
		}
		StopCoroutine (attack(fireRate, maxCount));
	}

}	
