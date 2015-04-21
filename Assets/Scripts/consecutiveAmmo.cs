using UnityEngine;
using System.Collections;

public class consecutiveAmmo : MonoBehaviour {
	public GameObject ammo;
	public int maxCount = 25;
	public Quaternion additionalRotation; 
	public bool attackLeft = false;
	public bool attackRight = false;
	public float fireRate = 0.05f;

	void Start () {
		additionalRotation = Quaternion.Euler (0,0,-1.5f);
		StartCoroutine(attack(fireRate, maxCount));
	}
	
	IEnumerator attack(float fireRate, int count){
		for (int i=0; i <= count; i++) {
			Instantiate(ammo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
			Instantiate(ammo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
			if (attackLeft){
				transform.rotation = transform.rotation * additionalRotation;
			}else if (attackRight){
				transform.rotation = transform.rotation * additionalRotation;
			}
			yield return new WaitForSeconds (fireRate);
		}
	}

}	
