using UnityEngine;
using System.Collections;

public class consecutiveAmmo : MonoBehaviour {
	public GameObject ammo;
	private int count = 0;
	private Quaternion additionalRotation = Quaternion.Euler (0,0,1.5f);
	private float nextFire;
	private float fireRate;

	void FixedUpdate () {
		if (count <= 70 && Time.time > nextFire){
			nextFire = Time.time + 0.1f;
			Instantiate(ammo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
			Instantiate(ammo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
			transform.rotation = transform.rotation * additionalRotation;
			count++;
		}
	}
}
