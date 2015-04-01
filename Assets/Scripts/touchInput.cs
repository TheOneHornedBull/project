using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class touchInput : MonoBehaviour {

	private Vector3 touchPoint;
	private int HP = 120;
	private float nextFire;
	public bool useRockets;
	private float nextRocketFire;
	private int rocketCount;
	private Ray ray;
	private RaycastHit hit;
	private GameObject fill;
	private bool isPaused;
	public GameObject playerRockets;
	public GameObject playerBullet;
	public GameObject sparks;
	public Slider playerHPBar;
	public Text HPText;
	public GameObject startButton;
	private Vector3 sparksPosition;
	[Range (0.1f, 10)]
	public float maxMovSpeed;
	private Vector3 newPosition;
	private GameObject playerBody;
	private Animator anim;

	void Awake () {
		anim = GetComponent <Animator> ();
		Time.timeScale = 0;
		rocketCount = 0;
		isPaused = true;
		fill = GameObject.Find ("PlayerFill");
		playerBody = GameObject.Find ("playerBody");
	}
	
	void FixedUpdate () {

		MovementAndShooting ();

		if (HP == 80 || HP == 60 || HP == 30) {
			HP -= 5;
			anim.SetBool ("playerShake",true);
			StartCoroutine (colorChanger());
		}

		playerHPBar.value = HP;
		if (HP >= 80) {
			fill.GetComponent<Image>().color = new Color32 (10, 255, 0, 255);
		}
		if (HP < 80 && HP >= 30) {
			fill.GetComponent<Image>().color = new Color32 (255, 180, 40, 255);
		}
		if (HP < 30) {
			fill.GetComponent<Image>().color = new Color32 (255, 0, 0, 255);
		}
		HPText.text = "120 / " + HP.ToString ();
		if (HP <= 0) {
			Debug.Log("Game over ! You have been defeated.");
			Time.timeScale = 0;
		}

	}

	void MovementAndShooting () {
		#if UNITY_EDITOR
			if(Input.GetMouseButton(0)){
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray,out hit)){
					touchPoint = hit.point;
				}
				transform.position = newPosition;
				Vector3 currentSpeed = Vector3.zero;
				//if (hit.collider.tag == "tapPlaneTag"){
					newPosition = Vector3.SmoothDamp (transform.position, new Vector3 (touchPoint.x,touchPoint.y + 1f, 0), ref currentSpeed, Time.deltaTime * maxMovSpeed);
			//	}
				Shooting();
			}
		#endif
		#if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
			if (Input.touchCount > 0) {
				Touch touch = Input.GetTouch (0);
				Shooting ();
				if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
					ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
					if(Physics.Raycast(ray,out hit)){
						touchPoint = hit.point;
					}
				}
				transform.position = newPosition;
				Vector3 currentSpeed = Vector3.zero;
			//	if (hit.collider.tag == "tapPlaneTag"){
					newPosition = Vector3.SmoothDamp (transform.position, new Vector3(touchPoint.x, touchPoint.y + 1f, 0), ref currentSpeed,Time.deltaTime * maxMovSpeed);
			//	}
			}
		#endif
	}

	void Shooting () {
		if (Time.time > nextFire) {
			nextFire = Time.time + 0.2f;
			Instantiate (playerBullet, transform.position, transform.rotation);
		}

		if (Time.time > nextRocketFire && useRockets == true && rocketCount <= 100) {
			nextRocketFire = Time.time + 0.4f;
			Instantiate (playerRockets, transform.position, transform.rotation);
			rocketCount ++;
			Debug.Log("Rocket");
		}

	} 

	void OnTriggerEnter (Collider other){
		if (other.tag == "enemyBullet") {
			HP -= 5;

			if (Random.Range(1,5) == 1) {
				sparksPosition = new Vector3 (0.5f,-1,0);
			}
			if (Random.Range(1,5) == 2){
				sparksPosition = new Vector3 (-0.5f,-1,0);
			}
			if (Random.Range(1,5) == 3){
				sparksPosition = new Vector3 (0.15f,0.45f,0);
			}
			if (Random.Range(1,5) == 4){
				sparksPosition = new Vector3 (-0.5f,0.25f,0);
			}
			Instantiate(sparks, sparksPosition + transform.position,transform.rotation);
		}

		if (other.tag == "HPCrate") {
			HP = HP + HP/2;
			if (HP > 120){
				HP = 120;
			}
		}

		if (other.tag == "RocketCrate") {
			useRockets = true;
			rocketCount = 0;
		}

	}

	IEnumerator colorChanger () {
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,75,75,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,255,255,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,75,75,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,255,255,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,75,75,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,255,255,255);
		yield return new WaitForSeconds (0.1f);
		StopCoroutine (colorChanger ());
	}

	public void Play () {
		Time.timeScale = 1;
		isPaused = false;
		Destroy (startButton);
	}

	public void Pause () {
		if (isPaused == true) {
			Time.timeScale = 1;
			isPaused = false;
		} else {
			Time.timeScale = 0;
			isPaused = true;
		}
	}

	public void Restart () {
		Time.timeScale = 0;
		Application.LoadLevel (0);
	}
}
