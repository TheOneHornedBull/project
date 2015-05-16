using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossScript : MonoBehaviour {

	public int HP;
	public Text hpText;
	public Slider HPSlider;
	public GameObject consecAttAmmo;
	public GameObject sparks;
	public GameObject explosion;
	private Vector3 sparksPosition;
	public float defaultWaitBeforeChangeDirection = 4f;
	private GameObject leftWing;
	private GameObject rightWing;
	private GameObject shield;
	private GameObject hpFill;
	private int i;
	public float basicShotRate = 0.15f;
	public float arrowShotRate = 0.3f;
	public float buckShotRate = 0.3f;
	public float spreadShotRate = 0.3f;
	public int basicAttackNumber = 30;
	public int arrowShotNumber = 3;
	public int buckShotNumber = 5;
	public int spreadShotNumber = 4;
	public bool doBasicAttack = false;
	public bool doArrowAttack = false;
	public bool doBuckShotAttack = false;
	public bool doSpreadShotAttack = false;
	public bool doConsecAttack = false;
	public bool useShield;
	bossShootingScript bossShooting;
	bossMovementScript bossMovement;
	private int basicAttacksStarted=0;
	private int arrowAttacksStarted=0;
	private int spreadAttacksStarted=0;
	private int buckAttacksStarted=0;
	public int loopsDone=0;
	private GameObject leftConsecGun;
	private GameObject rightConsecGun;
	Animator anim;
	consecutiveAmmo CA;
	Animator leftAnim;
	Animator rightAnim;
	GameObject leftCover;
	GameObject rightCover;
	testScript ts;

	public void Start () {
		HP = 10000;
		useShield = false;
		leftWing = GameObject.Find ("leftWing");
		rightWing = GameObject.Find ("rightWing");
		shield = GameObject.Find ("Shield");
		leftConsecGun = GameObject.Find ("leftConsecutiveGun");
		rightConsecGun = GameObject.Find ("rightConsecutiveGun");
		anim = GetComponent<Animator> ();
		useShield = false;
		doBasicAttack = false;
		hpFill = GameObject.Find ("BossFill");
		bossShooting = GetComponent<bossShootingScript> ();
		bossMovement = GetComponent<bossMovementScript> ();
		StartCoroutine (shootingPhasesController());
		CA = consecAttAmmo.GetComponent<consecutiveAmmo> ();
		bossMovement = GetComponent<bossMovementScript> ();
		leftConsecGun = GameObject.Find ("leftGun");
		rightConsecGun = GameObject.Find ("rightGun");
		leftCover = GameObject.Find ("leftCover");
		rightCover = GameObject.Find ("rightCover");
		leftAnim = leftCover.GetComponent<Animator>();
		rightAnim = rightCover.GetComponent<Animator>();
	}

	public void FixedUpdate () {
		ts = GetComponent <testScript>();
		shootingPhases ();
		shieldOnOff ();
		bossMovement.defaultMovement ();
		HPSlider.value = HP;
		if (HP >= 6000) {
			hpFill.GetComponent<Image>().color = new Color32 (10, 255, 0, 255);
		}
		if (HP < 6000 && HP >= 2000) {
			hpFill.GetComponent<Image>().color = new Color32 (255, 180, 40, 255);
		}
		if (HP < 2000) {
			hpFill.GetComponent<Image>().color = new Color32 (255, 0, 0, 255);
		}
		hpText.text = "10000 / " + HP.ToString ();
		if(HP % 1000 == 0 && HP != 10000){
			if(Random.Range(1,3) == 1){
				Instantiate(bossShooting.HPCrate,transform.position,transform.rotation);
			}else if(Random.Range(1,3) == 2){
				Instantiate(bossShooting.rocketCrate, transform.position, transform.rotation);
			}
			HP -= 10;
		}
		if (HP <= 0) {
			leftWing.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere;
			leftWing.GetComponent<Rigidbody>().velocity = (transform.up - transform.right)*10;
			rightWing.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * 5;
			rightWing.GetComponent<Rigidbody>().velocity = (transform.up + transform.right)*10;
			GetComponent<Rigidbody>().velocity = transform.up * 7;
			GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * 5;
			doBasicAttack = false;
			doArrowAttack = false;
			doBuckShotAttack = false;
			useShield = false;
			StopAllCoroutines();
			Debug.Log ("You win !");
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

	void shootingPhases () {

		if (doBasicAttack && basicAttacksStarted < 1) {
			StartCoroutine(bossShooting.basicAttack(basicShotRate, basicAttackNumber));
			basicAttacksStarted ++;
		}

		if (doBasicAttack == false){
			StopCoroutine(bossShooting.basicAttack(basicShotRate, basicAttackNumber));
			basicAttacksStarted = 0;
		}
	
		if(doArrowAttack && arrowAttacksStarted < 1){
			StartCoroutine(bossShooting.arrowAmmoAttack(arrowShotRate,arrowShotNumber));
			arrowAttacksStarted ++;
		}else if (doArrowAttack == false)  {
			StopCoroutine(bossShooting.arrowAmmoAttack(arrowShotRate,arrowShotNumber));
			arrowAttacksStarted = 0;
		}

		if(doSpreadShotAttack && spreadAttacksStarted < 1){
			StartCoroutine(bossShooting.spreadShotAttack(spreadShotRate,spreadShotNumber));
			spreadAttacksStarted ++;
		}else if (doSpreadShotAttack == false)  {
			StopCoroutine(bossShooting.spreadShotAttack(spreadShotRate,spreadShotNumber));
			spreadAttacksStarted = 0;
		}

		if(doBuckShotAttack && buckAttacksStarted < 1){
			StartCoroutine(bossShooting.buckShotAttack(buckShotRate,buckShotNumber));
			buckAttacksStarted++;
		}else if (doBuckShotAttack == false)  {
			StopCoroutine(bossShooting.buckShotAttack(buckShotRate,buckShotNumber));
			buckAttacksStarted = 0;
		}
	}

	IEnumerator shootingPhasesController () {
		while (true){
			if (ts.consecDone == true){
				yield return new WaitForSeconds (1.5f);
				bossMovement.move = true;
				bossMovement.moveDefault = true;
				useShield = false;
				doBasicAttack = true;
				bossMovement.maxSpeed = 7;
				yield return new WaitForSeconds (basicShotRate * basicAttackNumber + 1.5f);
				useShield = true;
				doBasicAttack = false;
				bossMovement.maxSpeed = 15;
				doArrowAttack = true;
				yield return new WaitForSeconds (arrowShotRate * arrowShotNumber + 0.5f);
				doArrowAttack = false;
				useShield = false;
				bossMovement.moveDefault = false;
				bossMovement.maxSpeed = 10;
				bossMovement.nextPosition = new Vector3 (0,20,0);
				bossMovement.timeToReachTarget = 2;
				yield return new WaitForSeconds (0.8f);
				bossMovement.move = false;
				doSpreadShotAttack = true;
				yield return new WaitForSeconds (spreadShotRate * spreadShotNumber + 0.5f);
				doSpreadShotAttack = false;
				bossMovement.move = true;
				bossMovement.maxSpeed = 7;
				bossMovement.moveDefault = true;
				doBasicAttack = true;
				yield return new WaitForSeconds (basicShotRate * basicAttackNumber + 0.5f);
				doBasicAttack = false;
				bossMovement.moveDefault = false;
				bossMovement.maxSpeed = 15;
				bossMovement.nextPosition = new Vector3 (0,20,0);
				yield return new WaitForSeconds (0.8f);
				bossMovement.move = false;
				yield return new WaitForSeconds (0.5f);
				doBuckShotAttack = true;
				yield return new WaitForSeconds (buckShotRate * buckShotNumber + 1.5f);
				doBuckShotAttack = false;
				bossMovement.move = true;
				bossMovement.maxSpeed = 30;
				loopsDone ++;
				if (loopsDone >= 2){
					/**
					if (Random.Range (1, 3) == 1) {
						leftAnim.SetBool("leftCoverConsec",true);
						rightAnim.SetBool("rightCoverConsec",true);
						yield return new WaitForSeconds (1f);
						bossMovement.nextPosition = new Vector3 (-6, 20, 0);
						bossMovement.move = true;
						yield return new WaitForSeconds (1.2f);
						bossMovement.move = false;
						CA.attackLeft = true;
						Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, 45));
						yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 3f);
						CA.attackLeft = false;
						bossMovement.move = true;
						bossMovement.nextPosition = new Vector3 (6, 20, 0);
						yield return new WaitForSeconds (0.6f);
						bossMovement.move = false;
						CA.attackRight = true;
						Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, -45));
						yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 3f);
						CA.attackRight = false;
						bossMovement.nextPosition = new Vector3 (-6, 20, 0);
						bossMovement.move = true;
						yield return new WaitForSeconds (1.2f);
						bossMovement.move = false;
						CA.attackLeft = true;
						Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, 45));
						yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 3f);
						CA.attackLeft = false;
						bossMovement.nextPosition = new Vector3 (6, 20, 0);
						bossMovement.move = true;
						yield return new WaitForSeconds (1.2f);
						CA.attackRight = true;
						Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, -45));
						yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 3f);
						CA.attackRight = false;
						leftAnim.SetBool("leftCoverConsec",false);
						rightAnim.SetBool("rightCoverConsec",false);
						yield return new WaitForSeconds (1f);
					}else{
						leftAnim.SetBool("leftCoverConsec",true);
						rightAnim.SetBool("rightCoverConsec",true);
						yield return new WaitForSeconds (0.5f);
						bossMovement.nextPosition = new Vector3 (-6, 20, 0);
						bossMovement.move = true;
						yield return new WaitForSeconds (1.2f);
						bossMovement.move = false;
						CA.attackLeft = true;
						Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, 45));
						yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 3f);
						CA.attackLeft = false;
						bossMovement.move = true;
						bossMovement.nextPosition = new Vector3 (6, 20, 0);
						yield return new WaitForSeconds (0.6f);
						bossMovement.move = false;
						CA.attackRight = true;
						Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, -45));
						yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 3f);
						CA.attackRight = false;
						bossMovement.nextPosition = new Vector3 (-6, 20, 0);
						bossMovement.move = true;
						yield return new WaitForSeconds (1.2f);
						bossMovement.move = false;
						CA.attackLeft = true;
						Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, 45));
						yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 3f);
						CA.attackLeft = false;
						bossMovement.nextPosition = new Vector3 (6, 20, 0);
						bossMovement.move = true;
						yield return new WaitForSeconds (1.2f);
						CA.attackRight = true;
						Instantiate (consecAttAmmo, transform.position, Quaternion.Euler (0, 0, -45));
						yield return new WaitForSeconds ((CA.fireRate * (CA.maxCount + CA.maxCount - 15)) + 3f);
						CA.attackRight = false;
						leftAnim.SetBool("leftCoverConsec",false);
						rightAnim.SetBool("rightCoverConsec",false);
						yield return new WaitForSeconds (1f);
					}
					*/
					doConsecAttack = true;
					yield return new WaitForSeconds (0.5f);
				}
			}
		}
	}

	IEnumerator colorChanger (float _waitBeforeChangeColor, int _numberOfFlashes) {
		for (int i=0; i <= _numberOfFlashes; i++) {
			leftWing.GetComponent<Renderer>().material.color = new Color32(255,75,75,255);
			rightWing.GetComponent<Renderer>().material.color = new Color32(255,75,75,255);
			GetComponent<Renderer>().material.color = new Color32(255,75,75,255);
			yield return new WaitForSeconds (_waitBeforeChangeColor);
			leftWing.GetComponent<Renderer>().material.color = new Color32(255,255,255,255);
			rightWing.GetComponent<Renderer>().material.color = new Color32(255,255,255,255);
			GetComponent<Renderer>().material.color = new Color32(255,255,255,255);
			yield return new WaitForSeconds (_waitBeforeChangeColor);
			i++;
		}
		StopCoroutine (colorChanger(_waitBeforeChangeColor,_numberOfFlashes));
	}
	
	void OnTriggerEnter (Collider other){
		if (other.tag == "playerBullet") {
			HP -= 10;
			if (Random.Range(1,10) == 1) {
				sparksPosition = new Vector3 (2.5f,-0.2f,0);
			}else if (Random.Range(1,10) == 2){
				sparksPosition = new Vector3 (2.9f,1f,0);
			}else if (Random.Range(1,10) == 3){
				sparksPosition = new Vector3 (1.1f,1.45f,0);
			}else if (Random.Range(1,10) == 4){
				sparksPosition = new Vector3 (-1.1f,1f,0);
			}else if (Random.Range(1,10) == 5){
				sparksPosition = new Vector3 (0,-0.75f,0);
			}else if (Random.Range(1,10) == 6){
				sparksPosition = new Vector3 (-0.2f,-2.5f,0);
			}else if (Random.Range(1,10) == 7){
				sparksPosition = new Vector3 (-2.8f,1.6f,0);
			}else if (Random.Range(1,10) == 8){
				sparksPosition = new Vector3 (-2.31f,-0.5f,0);
			}else {
				sparksPosition = new Vector3 (-0.6f,0.25f,0);
			}
			Instantiate(sparks, sparksPosition + transform.position,transform.rotation);
		}

		if (other.tag == "playerRocket") {
			if (Random.Range(1,10) == 1) {
				sparksPosition = new Vector3 (2.5f,-0.2f,0);
			}else if (Random.Range(1,10) == 2){
				sparksPosition = new Vector3 (2.9f,1f,0);
			}else if (Random.Range(1,10) == 3){
				sparksPosition = new Vector3 (1.1f,1.45f,0);
			}else if (Random.Range(1,10) == 4){
				sparksPosition = new Vector3 (-1.1f,1f,0);
			}else if (Random.Range(1,10) == 5){
				sparksPosition = new Vector3 (0,-0.75f,0);
			}else 	if (Random.Range(1,10) == 6){
				sparksPosition = new Vector3 (-0.2f,-2.5f,0);
			}else if (Random.Range(1,10) == 7){
				sparksPosition = new Vector3 (-2.8f,1.6f,0);
			}else if (Random.Range(1,10) == 8){
				sparksPosition = new Vector3 (-2.31f,-0.5f,0);
			}else{
				sparksPosition = new Vector3 (-0.6f,0.25f,0);
			}
			Instantiate(explosion, sparksPosition + transform.position,transform.rotation);

			HP -= 100;

		}
	}
}