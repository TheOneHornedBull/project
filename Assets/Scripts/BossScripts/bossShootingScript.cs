using UnityEngine;
using System.Collections;

public class bossShootingScript : MonoBehaviour {
	public GameObject basicAmmo;
	public GameObject arrowAmmo;
	public GameObject spreadShot;
	public GameObject farLeftLBS;
	public GameObject farLeftRBS;
	public GameObject farRightLBS;
	public GameObject farRightRBS;
	public GameObject middleLBS;
	public GameObject middleRBS;
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
	public bool consecAttFinished = true;
	bossMovementScript bossMovement;
	consecutiveAmmo CA;

	void Start () {
		CA = consecutiveAmmo.GetComponent<consecutiveAmmo> ();
		bossMovement = GetComponent<bossMovementScript> ();
	}

	public IEnumerator basicAttack(float _basicShotRate, int _basicAttackNumber){
		for(int i=0; i <= _basicAttackNumber; i++) {
			Instantiate(basicAmmo, transform.position - new Vector3 (2.7f, 1,0), transform.rotation);
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
			Instantiate (spreadShot, transform.position - new Vector3 (2.7f, 1,0),transform.rotation);
			Instantiate (spreadShot, transform.position + new Vector3 (2.7f, -1,0),transform.rotation);
			yield return new WaitForSeconds (_spreadShotRate);
		}
		yield return new WaitForSeconds (1);
	}

	public IEnumerator buckShotAttack (float _buckShotRate, int _buckShotNumber){

			if (Random.Range (1, 4) == 1) {
			Instantiate (farLeftLBS, transform.position - new Vector3 (2.7f, 1, 0), transform.rotation);
			Instantiate (farLeftRBS, transform.position + new Vector3 (2.7f, -1, 0), transform.rotation);
			yield return new WaitForSeconds (1);
				for (int i =0; i <= _buckShotNumber; i ++) {
					Instantiate (farLeftLBS, transform.position - new Vector3 (2.7f, 1, 0), transform.rotation);
					Instantiate (farLeftRBS, transform.position + new Vector3 (2.7f, -1, 0), transform.rotation);
					yield return new WaitForSeconds (_buckShotRate);
				}
			} else if (Random.Range (1, 4) == 2) {
				Instantiate (farRightLBS, transform.position - new Vector3 (2.7f, 1, 0), transform.rotation);
				Instantiate (farRightRBS, transform.position + new Vector3 (2.7f, -1, 0), transform.rotation);
				yield return new WaitForSeconds (1);
				for (int i =0; i <= _buckShotNumber; i ++) {
					Instantiate (farRightLBS, transform.position - new Vector3 (2.7f, 1, 0), transform.rotation);
					Instantiate (farRightRBS, transform.position + new Vector3 (2.7f, -1, 0), transform.rotation);
					yield return new WaitForSeconds (_buckShotRate);
				}
			} else {
				Instantiate (middleLBS, transform.position - new Vector3 (2.7f, 1, 0), transform.rotation);
				Instantiate (middleRBS, transform.position + new Vector3 (2.7f, -1, 0), transform.rotation);
				yield return new WaitForSeconds (1);
				for (int i =0; i <= _buckShotNumber; i ++) {
					Instantiate (middleLBS, transform.position - new Vector3 (2.7f, 1, 0), transform.rotation);
					Instantiate (middleRBS, transform.position + new Vector3 (2.7f, -1, 0), transform.rotation);
					yield return new WaitForSeconds (_buckShotRate);
				}
			}
			yield return new WaitForSeconds (1);
	}
}