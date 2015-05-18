using UnityEngine;
using System.Collections;

public class testScript : MonoBehaviour
{
	public bool consecDone;
	public GameObject consecAttAmmo;
	bossMovementScript bMovement;
	bossShootingScript bShooting;
	consecutiveAmmo CA;
	BossScript bs;
	int coroutinesStarted = 0;
	float x;
	void Start ()
	{
		bMovement = GetComponent <bossMovementScript> ();
		bShooting = GetComponent <bossShootingScript>();
		bs = GetComponent <BossScript>();
		CA = consecAttAmmo.GetComponent <consecutiveAmmo>();
	}

	public void Update ()
	{
		bMovement.nextPosition = new Vector3 (x, 20, 0);
		if (bs.doConsecAttack == true) {
			if (coroutinesStarted < 2){
				StartCoroutine(consecAttack ());
				coroutinesStarted++;
			}else {
				StopCoroutine (consecAttack ());
				coroutinesStarted--;
			}
		}
	}

	public IEnumerator consecAttack () {
		consecDone = false;
		if (Random.Range(1,3) == 1){
			x = -6;
		}else {
			x = 6;
		}
		for (int i = 0; i == 3; i++) {
			bMovement.move = true;
			yield return new WaitForSeconds(1.2f);
			bMovement.move = false;
			if (x > 0){
				CA.attackRight = true;
				CA.attackLeft = false;
				Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, -45));
			}else {
				CA.attackRight = false;
				CA.attackLeft = true;
				Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, 45));
			}
			yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 3f);
			x *= -1;
		}
		consecDone = true;
	}
}

