using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class touchInput : MonoBehaviour {

	private Vector3 touchPoint;
	private int HP = 120;
	private float nextFire;
	private bool useRockets;
	private float nextRocketFire;
	private int rocketCount;
	private Ray ray;
	private RaycastHit hit;
	private GameObject fill;
	private bool isPaused;
	public GameObject playerRockets;
	public GameObject playerBullet;
	public Slider playerHPBar;
	public Text HPText;
	public GameObject startButton;
	[Range(0.1f, 10)]
	public float maxXSpeed;
	[Range(0.1f, 10)]
	public float maxYSpeed;
	private Vector3 newPosition;

	void Awake () {
		Time.timeScale = 0;
		rocketCount = 0;
		isPaused = true;
		fill = GameObject.Find ("PlayerFill");
	}
	
	void FixedUpdate () {

		MovementAndShooting ();

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
				float currentSpeed = 0f;
				newPosition.x = Mathf.SmoothDamp (transform.position.x, touchPoint.x, ref currentSpeed,Time.deltaTime * maxXSpeed);
				newPosition.y = Mathf.SmoothDamp (transform.position.y, touchPoint.y, ref currentSpeed,Time.deltaTime * maxYSpeed);
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
				transform.position = Vector3.Lerp (transform.position, new Vector3 (touchPoint.x, touchPoint.y + 1.5f, transform.position.z), Time.deltaTime * 6);
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
		}

		if (other.tag == "HPCrate") {
			HP = HP + HP/2;
		}

		if (other.tag == "RocketCrate") {
			useRockets = true;
			rocketCount = 0;
		}

	}
	/**
	void OnTriggerStay (Collider other){
		if (other.tag == "MainCamera") {
			MovementAndShooting();
		}
	}

	void OnTriggerExit (Collider other){
		if (other.tag == "MainCamera" ) {
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
	}
	*/
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
