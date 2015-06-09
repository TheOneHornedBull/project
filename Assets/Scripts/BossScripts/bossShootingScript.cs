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
	Animator leftConsecCoverAnim;
	Animator rightConsecCoverAnim;
    Animator leftConsecGunAnim;
    Animator rightConsecGunAnim;
	GameObject leftCover;
	GameObject rightCover;
	GameObject leftGun;
	GameObject rightGun;
    GameObject leftSpreadShotGunCenter;
    GameObject rightSpreadShotGunCenter;
	bossMovementScript bossMovement;
	consecutiveAmmo CA;
	bossShootingScript bShooting;
	BossScript bs;
	float x;

	void Start () {
		bs = GetComponent <BossScript>();
		bossMovement = GetComponent<bossMovementScript> ();
		CA = consecutiveAmmo.GetComponent<consecutiveAmmo> ();
		leftCover = GameObject.Find ("leftCover");
		rightCover = GameObject.Find ("rightCover");
		leftGun = GameObject.Find ("leftGun");
		rightGun = GameObject.Find ("rightGun");
        leftSpreadShotGunCenter = GameObject.Find("lSSCenter");
        rightSpreadShotGunCenter = GameObject.Find("rSSCenter");
		leftConsecCoverAnim = leftCover.GetComponent<Animator>();
		rightConsecCoverAnim = rightCover.GetComponent<Animator>();
        leftConsecGunAnim = leftGun.GetComponent<Animator>();
        rightConsecGunAnim = rightGun.GetComponent<Animator>();
	}

	public IEnumerator basicAttack(float _basicShotRate, int _basicAttackNumber){
		for(int i=0; i <= _basicAttackNumber; i++) {
			Instantiate(basicAmmo, transform.position - new Vector3 (2, 2,0), transform.rotation);
			Instantiate(basicAmmo, transform.position + new Vector3 (2, -2,0), transform.rotation);
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


        for (int i = 0; i <= 3; i++) {
            leftSpreadShotGunCenter.GetComponent<Renderer>().material.color = new Color32(255,200,0,255);
            rightSpreadShotGunCenter.GetComponent<Renderer>().material.color = new Color32(255, 200, 0, 255);
            yield return new WaitForSeconds(0.2f);
            leftSpreadShotGunCenter.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
            rightSpreadShotGunCenter.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
            yield return new WaitForSeconds(0.2f);
        }
        leftSpreadShotGunCenter.GetComponent<Renderer>().material.color = new Color32(255, 200, 0, 255);
        rightSpreadShotGunCenter.GetComponent<Renderer>().material.color = new Color32(255, 200, 0, 255);

			if (Random.Range (1, 4) == 1) {
			Instantiate (farLeftLBS, transform.position - new Vector3 (2.3f, 1.5f, 0), transform.rotation);
            Instantiate(farLeftRBS, transform.position + new Vector3(2.3f, -1.5f, 0), transform.rotation);
			yield return new WaitForSeconds (1);
				for (int i =0; i <= _buckShotNumber; i ++) {
                    Instantiate(farLeftLBS, transform.position - new Vector3(2.3f, 1.5f, 0), transform.rotation);
                    Instantiate(farLeftRBS, transform.position + new Vector3(2.3f, -1.5f, 0), transform.rotation);
					yield return new WaitForSeconds (_buckShotRate);
				}
			} else if (Random.Range (1, 4) == 2) {
                Instantiate(farRightLBS, transform.position - new Vector3(2.3f, 1.5f, 0), transform.rotation);
                Instantiate(farRightRBS, transform.position + new Vector3(2.3f, -1.5f, 0), transform.rotation);
				yield return new WaitForSeconds (1);
				for (int i =0; i <= _buckShotNumber; i ++) {
                    Instantiate(farRightLBS, transform.position - new Vector3(2.5f, 1.5f, 0), transform.rotation);
                    Instantiate(farRightRBS, transform.position + new Vector3(2.5f, -1.5f, 0), transform.rotation);
					yield return new WaitForSeconds (_buckShotRate);
				}
			} else {
                Instantiate(middleLBS, transform.position - new Vector3(2.3f, 1.5f, 0), transform.rotation);
                Instantiate(middleRBS, transform.position + new Vector3(2.3f, -1.5f, 0), transform.rotation);
				yield return new WaitForSeconds (1);
				for (int i =0; i <= _buckShotNumber; i ++) {
                    Instantiate(middleLBS, transform.position - new Vector3(2.3f, 1.5f, 0), transform.rotation);
                    Instantiate(middleRBS, transform.position + new Vector3(2.3f, -1.5f, 0), transform.rotation);
					yield return new WaitForSeconds (_buckShotRate);
				}
			}
			yield return new WaitForSeconds (1);
	}

	public IEnumerator consecAttack () {
		if (Random.Range(1,3) == 1){
			x = -6;
		}else {
			x = 6;
		}
		leftConsecCoverAnim.SetBool("leftCoverConsec",true);
		rightConsecCoverAnim.SetBool("rightCoverConsec",true);
     //   yield return new WaitForSeconds(0.5f);
     //   leftConsecGunAnim.SetBool("riseLeftConsecGun",true);
     //  rightConsecGunAnim.SetBool("riseRightConsecGun",true);
		yield return new WaitForSeconds (1);
		for (int i = 0; i <= 3; i++) {
			bossMovement.nextPosition = new Vector3 (x,20,0);
			bossMovement.move = true;
			yield return new WaitForSeconds(1.2f);
			bossMovement.move = false;
			if (x > 0){
				CA.attackRight = true;
				CA.attackLeft = false;
				Instantiate (consecutiveAmmo, transform.position, Quaternion.Euler (0, 0, -45));
			}else {
				CA.attackRight = false;
				CA.attackLeft = true;
				Instantiate (consecutiveAmmo, transform.position, Quaternion.Euler (0, 0, 45));
			}
			yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 4f);
			x *= -1;
		}
       // leftConsecGunAnim.SetBool("riseLeftConsecGun",false);
       // rightConsecGunAnim.SetBool("riseRightConsecGun",false);
       // yield return new WaitForSeconds(0.5f);
		leftConsecCoverAnim.SetBool("leftCoverConsec",false);
		rightConsecCoverAnim.SetBool("rightCoverConsec",false);
		yield return new WaitForSeconds (1);
		bs.doConsecAttack = false;
	}

}