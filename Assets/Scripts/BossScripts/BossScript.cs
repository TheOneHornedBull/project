using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossScript : MonoBehaviour {

	public int HP;
	public Text hpText;
	public Slider HPSlider;
	public GameObject consecAttAmmo;
	public GameObject sparks;
	public GameObject electricSparks;
	public GameObject fireSparks;
	public GameObject acidSparks;
	public GameObject explosion;
	private Vector3 sparksPosition;
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
	private int consecAttacksStarted=0;
	public int loopsDone=0;

	public void Start () {
		HP = 7500;
		useShield = false;
		shield = GameObject.Find ("Shield");
		useShield = false;
		doBasicAttack = false;
		hpFill = GameObject.Find ("BossFill");
		leftWing = GameObject.Find ("leftWing");
		rightWing = GameObject.Find ("rightWing");
		bossShooting = GetComponent<bossShootingScript> ();
		bossMovement = GetComponent<bossMovementScript> ();
		StartCoroutine (shootingPhasesController());
		bossMovement = GetComponent<bossMovementScript> ();
	}

	public void FixedUpdate () {
		shootingPhases ();
		shieldOnOff ();
		bossMovement.defaultMovement ();
		HPSlider.value = HP;
		if (HP >= 4000) {
			hpFill.GetComponent<Image>().color = new Color32 (10, 255, 0, 255);
		}
		if (HP < 4000 && HP >= 1000) {
			hpFill.GetComponent<Image>().color = new Color32 (255, 180, 40, 255);
		}
		if (HP < 1000) {
			hpFill.GetComponent<Image>().color = new Color32 (255, 0, 0, 255);
		}
		hpText.text = "7500 / " + HP.ToString ();
		if(HP % 1000 == 0 && HP != 7500){
			if(Random.Range(1,3) == 1){
				StartCoroutine (colorChanger(0.2f,10));
				Instantiate(bossShooting.HPCrate,transform.position,transform.rotation);
			}else {
				StartCoroutine (colorChanger(0.2f,10));
				Instantiate(bossShooting.rocketCrate, transform.position, transform.rotation);
			}
			HP -= 10;
		}
		if (HP <= 0) {
			leftWing.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * 10;
			leftWing.GetComponent<Rigidbody>().velocity = (transform.up - transform.right)*10;
			rightWing.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * 10;
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

		if (doConsecAttack && consecAttacksStarted < 1) {
			StartCoroutine (bossShooting.consecAttack ());
			consecAttacksStarted++;
		} else if (doConsecAttack && consecAttacksStarted > 1){
			StopCoroutine(bossShooting.consecAttack ());
		} else if (doConsecAttack == false && consecAttacksStarted <= 1) {
			StopCoroutine(bossShooting.consecAttack());
			consecAttacksStarted = 0;
		}
	}

	public IEnumerator shootingPhasesController () {
		while (true){
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
					doConsecAttack = true;
					yield return new WaitForSeconds (28);
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

		if (other.tag == "playerElectricBullet"){
			StartCoroutine ((takeElectricDmg()));

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
			Instantiate(electricSparks, sparksPosition + transform.position,transform.rotation);
		}

		if (other.tag == "playerFireBullet") {
			StartCoroutine (takeFireDmg());

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
			Instantiate(fireSparks, sparksPosition + transform.position,transform.rotation);
		}

		if (other.tag == "playerAcidBullet") {
			StartCoroutine (takeAcidDmg());

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
			Instantiate(acidSparks, sparksPosition + transform.position,transform.rotation);
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

	IEnumerator takeFireDmg () {
		for (int i=0; i <= 5; i++){
			HP -= 5;
			yield return new WaitForSeconds (0.5f);
		}
		StopCoroutine (takeFireDmg());
	}
	IEnumerator takeAcidDmg () {
		for (int i=0;i <= 5; i++){
			HP -= 10;
			yield return new WaitForSeconds (0.5f);
		}
		StopCoroutine (takeAcidDmg());
	}
	IEnumerator takeElectricDmg (){
		for (int i=0;i<=5;i++){
			HP -= 5;
			yield return new WaitForSeconds (0.5f);
		}
		StopCoroutine (takeElectricDmg ());
	}
}