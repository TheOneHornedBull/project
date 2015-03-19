using UnityEngine;
using System.Collections;

public class bossScript2 : MonoBehaviour {
	public GameObject basicAmmo;
	public GameObject arrowAmmo;
	public GameObject spreadShot;
	public GameObject consecutiveAmmo;
	private GameObject leftWing;
	private GameObject rightWing;
	private GameObject shield;
	private int HP;
	public float lrTime; // has to be long
	private Vector3 velocity = Vector3.zero;
	private int arrowAmmCount;
	private int spreadAmmCount;
	private int consecutiveAmmCount;
	private bool doBasicAttack;
	private bool doArrowAttack;
	private bool doSpreadAttack;
	private bool doConsecutiveAttack;
	private bool stop;
	private bool useShield;
	private int holdCount;
	private float nextFire;
	private float previousAttack;
	private float basicNextFire;
	private Vector3 nextPosition;

	void Start () {
		StartCoroutine (defaultMovement());
		StartCoroutine (phases ());
		HP = 5000;
		stop = false;
		useShield = false;
		doBasicAttack = true;
		doArrowAttack = false;
		doSpreadAttack = false;
		doConsecutiveAttack = false;
		leftWing = GameObject.Find ("leftWing");
		rightWing = GameObject.Find ("rightWing");
		shield = GameObject.Find ("Shield");
	}

	void FixedUpdate () {
		move ();
		basicAttack ();
		shieldOnOff ();
		if (HP <= 0) {
			leftWing.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere;
			leftWing.GetComponent<Rigidbody>().velocity = transform.up - transform.right;
			rightWing.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere;
			rightWing.GetComponent<Rigidbody>().velocity = transform.up + transform.right;
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
			basicNextFire = Time.time + 0.5f;
			Instantiate(basicAmmo, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
			Instantiate(basicAmmo, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
		}
	}

	void spreadShotAttack () {
		if (Time.time > nextFire && doSpreadAttack) {
			nextFire = Time.time + 0.2f;
			if (spreadAmmCount <= 5){
				Instantiate(spreadShot, transform.position - new Vector3 (2.7f, -1,0), transform.rotation);
				Instantiate(spreadShot, transform.position + new Vector3 (2.7f, -1,0), transform.rotation);
				spreadAmmCount++;
			}
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

	void arrowAttack () {
		if (Time.time > nextFire && doArrowAttack) {
			nextFire = Time.time + 0.05f;
			if (arrowAmmCount <= 3) {
				Instantiate (arrowAmmo, transform.position, transform.rotation);
				arrowAmmCount++;
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
			if (Random.Range (1,9) <= 3){
				doArrowAttack = true;
				arrowAttack();
				yield return new WaitForSeconds(1);
				doArrowAttack = false;
				arrowAmmCount = 0;
			}

			if (Random.Range (1,9) >= 4 && Random.Range (1,9) <= 6){
				doSpreadAttack = true;
				spreadShotAttack();
				yield return new WaitForSeconds (1);
				doSpreadAttack = false;
				spreadAmmCount = 0;
			}

			if (Random.Range (1,9) >= 8 && Time.time > previousAttack + 20){
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
		}
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
	}

	void OnGUI () {
		GUI.Label(new Rect(10, 10, 100, 20), HP.ToString());
	}

}
