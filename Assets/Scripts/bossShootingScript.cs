﻿using UnityEngine;
using System.Collections;

public class bossShootingScript : MonoBehaviour {
	public GameObject basicAmmo;
	public GameObject arrowAmmo;
	public GameObject spreadShot;
	public GameObject buckShotAmmo;
	public GameObject consecutiveAmmo;
	public GameObject sparks;
	public GameObject explosion;
	public GameObject HPCrate;
	public GameObject rocketCrate;
	private int consecutiveAmmCount;
	private bool doBasicAttack;
	private bool doConsecutiveAttack;
	private float nextFire;
	private float previousAttack;
	private float basicNextFire;
	private Vector3 nextPosition;
	private Vector3 sparksPosition;

	public IEnumerator basicAttack(float _basicShotRate, int _basicAttackNumber){
		for(int i=0; i <= _basicAttackNumber; i++) {
			Instantiate(basicAmmo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
			Instantiate(basicAmmo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
			yield return new WaitForSeconds (_basicShotRate);
		}
	}

	public IEnumerator arrowAmmoAttack (float _arrowAmmoRate, int _arrowAmmoNumber){
		for (int i=0; i <= _arrowAmmoNumber; i++){
			Instantiate (arrowAmmo,transform.position,transform.rotation);
			yield return new WaitForSeconds (_arrowAmmoRate);
		}
		yield return new WaitForSeconds (1);
	}

	public IEnumerator spreadShotAttack (float _spreadShotRate, int _spreadShotNumber) {
		for (int i =0; i <= _spreadShotNumber; i ++){
			Instantiate (spreadShot, transform.position - new Vector3 (2.7f, -1,0),transform.rotation);
			Instantiate (spreadShot, transform.position + new Vector3 (2.7f, -1,0),transform.rotation);
			yield return new WaitForSeconds (_spreadShotRate);
		}
		yield return new WaitForSeconds (1);
	}

	public IEnumerator buckShotAttack (float _buckShotRate, int _buckShotNumber){
		for (int i =0; i <= _buckShotNumber; i ++){
			Instantiate (buckShotAmmo, transform.position - new Vector3 (2.7f, -1,0),transform.rotation);
			Instantiate (buckShotAmmo, transform.position + new Vector3 (2.7f, -1,0),transform.rotation);
			yield return new WaitForSeconds (_buckShotRate);
		}
		yield return new WaitForSeconds (1);
	}
	
	public IEnumerator consecutiveAttack (float _consecutiveAttackRate) {
		Instantiate (consecutiveAmmo, transform.position, transform.rotation);
		yield return new WaitForSeconds (1);
	}
	 

}
