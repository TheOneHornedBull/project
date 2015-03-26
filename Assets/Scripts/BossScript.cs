using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {

	public GameObject basicAmmo;
	public GameObject arrowAmmo;
	public GameObject spreadShot;
	public GameObject buckShotAmmo;
	public GameObject consecutiveAmmo;
	public GameObject HPCrate;
	public GameObject rocketCrate;
	//public GameObject doubleDamageCrate;
	private GameObject leftWing;
	private GameObject rightWing;
	private GameObject shield;
	public int HP;
	public float lrTime; // has to be long
	private Vector3 velocity = Vector3.zero;
	private int consecutiveAmmCount;
	private bool doBasicAttack;
	private bool doConsecutiveAttack;
	private bool stop;
	private bool useShield;
	private float nextFire;
	private float previousAttack;
	private float basicNextFire;
	private Vector3 nextPosition;
	/**
		private Shader lwdiff;
		private Shader lwadd;
		private Shader rwdiff;
		private Shader rwadd;
	 */

	
	void Start () {
		StartCoroutine (defaultMovement());
		StartCoroutine (phases ());
		HP = 10000;
		stop = false;
		useShield = false;
		doBasicAttack = true;
		leftWing = GameObject.Find ("leftWing");
		rightWing = GameObject.Find ("rightWing");
		shield = GameObject.Find ("Shield");
		useShield = false;
//		lwdiff = Shader.Find ("Mobile/Diffuse");
//		lwadd = Shader.Find ("Mobile/Particles/Additive");
	}
	
	void FixedUpdate () {
		move ();
		basicAttack ();
		shieldOnOff ();

		if (HP == 1000 || HP == 2000 || HP == 3000 || HP == 4000 || HP == 5000 || HP == 7000 || HP == 9000) {
			HP -= 10;
			if (Random.Range (1,5) <= 2){
				Instantiate (HPCrate, transform.position, transform.rotation);
			}else {
				Instantiate (rocketCrate, transform.position, transform.rotation);
			}

		}
		/**
			if (HP <= 2000){
				if (HP % 100 == 0){
					// трябваше едно от крилата да стават transparent и само по другото да може да се бие
				}
			}
		 */

		if (HP <= 0) {
			leftWing.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere;
			leftWing.GetComponent<Rigidbody>().velocity = transform.up - transform.right;
			rightWing.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere;
			rightWing.GetComponent<Rigidbody>().velocity = transform.up + transform.right;
			GetComponent<Rigidbody>().velocity = transform.up;
			GetComponent<Rigidbody>().velocity = Random.insideUnitSphere;
			doBasicAttack = false;
			stop = true;
			useShield = false;
			StopAllCoroutines();
			Debug.Log ("You win !");
		}
		
	}
	
	void move () {
		if (stop == false) {
			transform.position = Vector3.SmoothDamp (transform.position, nextPosition, ref velocity, lrTime);
		}
	}
	
	// Attacks
	
	void basicAttack () {
		if (Time.time > basicNextFire && doBasicAttack) {
			basicNextFire = Time.time + 0.2f;
			Instantiate(basicAmmo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
			Instantiate(basicAmmo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
		}
	}

	void consecutiveAmmoAttack () {
		if (Time.time > nextFire && doConsecutiveAttack) {
			nextFire = Time.time + 0.1f;
			if (consecutiveAmmCount <= 8) {
				Instantiate (consecutiveAmmo, transform.position, transform.rotation * Quaternion.Euler (0,0,-30));
				consecutiveAmmCount++;
			}
		}
	}
	
	void shieldOnOff () {
		if (useShield) {
			shield.GetComponent<MeshRenderer> ().enabled = true;
			shield.GetComponent<SphereCollider> ().enabled = true;
		} else {
			shield.GetComponent<MeshRenderer> ().enabled = false;
			shield.GetComponent<SphereCollider> ().enabled = false;
		}
	}

	// Coroutines
	
	IEnumerator phases () {
		while (true) {

				if (Random.Range (1,12) <= 3){
					doBasicAttack = false;
					yield return new WaitForSeconds(1.5f);
					Instantiate (arrowAmmo, transform.position, transform.rotation);
					yield return new WaitForSeconds(0.3f);
					Instantiate (arrowAmmo, transform.position, transform.rotation);
					yield return new WaitForSeconds(0.3f);
					Instantiate (arrowAmmo, transform.position, transform.rotation);
					yield return new WaitForSeconds(1);
					doBasicAttack = true;
				}

				if (Random.Range (1,12) >= 4 && Random.Range (1,9) <= 6){
					doBasicAttack = false;
					yield return new WaitForSeconds(1.5f);
					Instantiate(spreadShot, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
					Instantiate(spreadShot, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
					yield return new WaitForSeconds (0.3f);
					Instantiate(spreadShot, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
					Instantiate(spreadShot, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
					yield return new WaitForSeconds (0.3f);
					Instantiate(spreadShot, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
					Instantiate(spreadShot, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
					yield return new WaitForSeconds (0.3f);
					Instantiate(spreadShot, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
					Instantiate(spreadShot, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
					yield return new WaitForSeconds (1);
					doBasicAttack = true;
				}
			
				if (Random.Range (1,12) == 8 && Time.time > previousAttack + 20){
					previousAttack = Time.time;
					StopCoroutine (defaultMovement());
					lrTime = 3;
					nextPosition = new Vector3 (0,20,0);
					yield return new WaitForSeconds (5);
					doBasicAttack = false;
					useShield = true;
					stop = true;
					doConsecutiveAttack = true;
					consecutiveAmmoAttack();
					yield return new WaitForSeconds (5);
					doConsecutiveAttack = false;
					consecutiveAmmCount = 0;
					stop = false;
					useShield = false;
					doBasicAttack = true;
					lrTime = 5;
					StartCoroutine (defaultMovement());
				}

					if (Random.Range(1,12) >= 9){
					doBasicAttack = false;
					yield return new WaitForSeconds (2);
					Instantiate (buckShotAmmo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
					Instantiate (buckShotAmmo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
					yield return new WaitForSeconds (0.3f);
					Instantiate (buckShotAmmo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
					Instantiate (buckShotAmmo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
					yield return new WaitForSeconds (0.3f);
					Instantiate (buckShotAmmo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
					Instantiate (buckShotAmmo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
					yield return new WaitForSeconds (0.3f);
					Instantiate (buckShotAmmo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
					Instantiate (buckShotAmmo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
					yield return new WaitForSeconds (0.3f);
					Instantiate (buckShotAmmo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
					Instantiate (buckShotAmmo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
					yield return new WaitForSeconds (1.5f);
					doBasicAttack = true;
				}
			}
		}

	IEnumerator GetOut () {
		StopCoroutine (defaultMovement());
		StopCoroutine (phases());
		if (Random.Range (1, 3) == 1) {
			nextPosition = new Vector3 (10, 25, 0);
		} else {
			nextPosition = new Vector3 (-10, 25, 0);
		}
		useShield = true;
		if (Random.Range (1, 3) >= 2) {
			Instantiate (HPCrate, transform.position, transform.rotation);
		}

		if (Random.Range (1, 3) <= 1) {
			Instantiate (rocketCrate, transform.position, transform.rotation);
		}

		yield return new WaitForSeconds (5);
		nextPosition = new Vector3 (0,20,0);
		yield return new WaitForSeconds (5);
		useShield = false;
		StartCoroutine (defaultMovement());
		StartCoroutine (phases());
		StopCoroutine (GetOut ());
	}
	
	IEnumerator defaultMovement () {
		while (true) {
			nextPosition =  new Vector3 (-6,20,0);
			yield return new WaitForSeconds (8);
			nextPosition =  new Vector3 (6,20,0);
			yield return new WaitForSeconds (8);
		}
	}
	
	void OnTriggerEnter (Collider other){
		if (other.tag == "playerBullet") {
			HP -= 10;
		}

		if (other.tag == "playerRocket") {
			HP -= 35;
		}
	}
	
	void OnGUI () {
		GUI.Label(new Rect(10, 10, 100, 20), HP.ToString());
	}
	
}