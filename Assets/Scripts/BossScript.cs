using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossScript : MonoBehaviour {

	public int HP;
	public Text hpText;
	public Slider HPSlider;
	public float defaultWaitBeforeChangeDirection;
	private GameObject leftWing;
	private GameObject rightWing;
	private GameObject shield;
	private GameObject hpFill;
	private int consecutiveAmmCount;
	private int i;
	public float basicShotRate;
	public float arrowShotRate;
	public float buckShotRate;
	public float spreadShotRate;
	public int basicAttackNumber;
	public int arrowShotNumber;
	public int buckShotNumber;
	public int spreadShotNumber;
	public bool doBasicAttack;
	public bool doArrowAttack;
	public bool doBuckShotAttack;
	public bool doSpreadShotAttack;
	public bool doConsecutiveAttack;
	public bool useShield;
	private Animator anim;
	bossShootingScript bossShooting;
	bossMovementScript bossMovement;
	private int basicAttacksStarted=0;
	private int arrowAttacksStarted=0;
	private int spreadAttacksStarted=0;
	private int buckAttacksStarted=0;
	private int consecutiveAttacksStarted=0;

	void Awake () {
		bossShooting = GetComponent<bossShootingScript> ();
		bossMovement = GetComponent<bossMovementScript> ();
		StartCoroutine (shootingPhasesController());
		anim = GetComponent<Animator> ();
		HP = 10000;
		useShield = false;
		leftWing = GameObject.Find ("leftWing");
		rightWing = GameObject.Find ("rightWing");
		shield = GameObject.Find ("Shield");
		useShield = false;
		doBasicAttack = false;
		hpFill = GameObject.Find ("BossFill");
	}

	void FixedUpdate () {
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
			}else if(Random.Range(1,3)>1){
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
			doConsecutiveAttack = false;
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
			yield return new WaitForSeconds (0.5f);
			bossMovement.move = true;
			bossMovement.moveDefault = true;
			useShield = false;
			doBasicAttack = true;
			bossMovement.maxSpeed = 5;
			yield return new WaitForSeconds (basicShotRate * basicAttackNumber + 1f);
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
			yield return new WaitForSeconds (0.5f);
			bossMovement.move = false;
			doSpreadShotAttack = true;
			yield return new WaitForSeconds (spreadShotRate * spreadShotNumber + 0.5f);
			doSpreadShotAttack = false;
			bossMovement.move = true;
			bossMovement.maxSpeed = 5;
			bossMovement.moveDefault = true;
			doBasicAttack = true;
			yield return new WaitForSeconds (basicShotRate * basicAttackNumber + 0.5f);
			doBasicAttack = false;
			bossMovement.moveDefault = false;
			bossMovement.maxSpeed = 15;
			bossMovement.nextPosition = new Vector3 (0,20,0);
			yield return new WaitForSeconds (0.5f);
			bossMovement.move = false;
			doBuckShotAttack = true;
			yield return new WaitForSeconds (buckShotRate * buckShotNumber + 0.5f);
			doBuckShotAttack = false;
			bossMovement.move = true;
			bossMovement.maxSpeed = 5;
			bossMovement.moveDefault = true;
			Debug.Log("end of cycle");
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
				//sparksPosition = new Vector3 (2.5f,-0.2f,0);
			}
			if (Random.Range(1,10) == 2){
				//sparksPosition = new Vector3 (2.9f,1f,0);
			}
			if (Random.Range(1,10) == 3){
				//sparksPosition = new Vector3 (1.1f,1.45f,0);
			}
			if (Random.Range(1,10) == 4){
				//sparksPosition = new Vector3 (-1.1f,1f,0);
			}
			if (Random.Range(1,10) == 5){
				//sparksPosition = new Vector3 (0,-0.75f,0);
			}
			if (Random.Range(1,10) == 6){
				//sparksPosition = new Vector3 (-0.2f,-2.5f,0);
			}
			if (Random.Range(1,10) == 7){
				//sparksPosition = new Vector3 (-2.8f,1.6f,0);
			}
			if (Random.Range(1,10) == 8){
				//sparksPosition = new Vector3 (-2.31f,-0.5f,0);
			}
			if (Random.Range(1,10) == 9){
				//sparksPosition = new Vector3 (-0.6f,0.25f,0);
			}
			//Instantiate(sparks, sparksPosition + transform.position,transform.rotation);
		}

		if (other.tag == "playerRocket") {
			if (Random.Range(1,10) == 1) {
			//	sparksPosition = new Vector3 (2.5f,-0.2f,0);
			}
			if (Random.Range(1,10) == 2){
			//	sparksPosition = new Vector3 (2.9f,1f,0);
			}
			if (Random.Range(1,10) == 3){
			//	sparksPosition = new Vector3 (1.1f,1.45f,0);
			}
			if (Random.Range(1,10) == 4){
			//	sparksPosition = new Vector3 (-1.1f,1f,0);
			}
			if (Random.Range(1,10) == 5){
			//	sparksPosition = new Vector3 (0,-0.75f,0);
			}
			if (Random.Range(1,10) == 6){
			//	sparksPosition = new Vector3 (-0.2f,-2.5f,0);
			}
			if (Random.Range(1,10) == 7){
			//	sparksPosition = new Vector3 (-2.8f,1.6f,0);
			}
			if (Random.Range(1,10) == 8){
			//	sparksPosition = new Vector3 (-2.31f,-0.5f,0);
			}
			if (Random.Range(1,10) == 9){
			//	sparksPosition = new Vector3 (-0.6f,0.25f,0);
			}
			//Instantiate(explosion, sparksPosition + transform.position,transform.rotation);

			HP -= 20;

		}
	}
}