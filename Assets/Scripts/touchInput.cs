using UnityEngine;
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
	private bool isPaused;
	public GameObject playerRockets;
	public GameObject playerBullet;
	public GameObject UIBtn;

	void Awake () {
		Time.timeScale = 0;
		isPaused = true;
	}
	
	void FixedUpdate () {
		if (HP <= 0) {
			Debug.Log("Game over ! You have been defeated.");
			Time.timeScale = 0;
		}
	}

	void MovementAndShooting () {
		#if UNITY_EDITOR
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    transform.position = Vector3.Lerp (transform.position, new Vector3 (touchPoint.x, touchPoint.y + 1.5f, transform.position.z), Time.deltaTime * 10);
			Shooting();
		#endif
		#if (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
			if (Input.touchCount > 0) {
				Touch touch = Input.GetTouch (0);
				Shooting ();
				if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved || Input.GetMouseButtonDown(0)) {
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
			Instantiate (playerBullet, transform.position, transform.rotation);
			rocketCount ++;
		}
		useRockets = false;

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

	void OnTriggerStay (Collider other){
		if (other.tag == "MainCamera") {
			MovementAndShooting();
		}
	}

	void OnTriggerExit (Collider other){
		if (other.tag == "MainCamer" && transform.position.x > 0) {
			GetComponent<Rigidbody>().AddForce(Vector3.left * 4, ForceMode.Impulse);
		}
		if (other.tag == "MainCamera" && transform.position.x < 0) {
			GetComponent<Rigidbody>().AddForce(Vector3.right * 4, ForceMode.Impulse);
		}
		if (other.tag == "MainCamera" && transform.position.y < 0) {
			GetComponent<Rigidbody>().AddForce(Vector3.up * 4, ForceMode.Impulse);
		}
		if (other.tag == "MainCamera" && transform.position.y > 8) {
			GetComponent<Rigidbody>().AddForce(Vector3.down * 4, ForceMode.Impulse);
		}
	}

	public void Play () {
		Time.timeScale = 1;
		isPaused = false;
		Destroy (UIBtn);
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

	void OnGUI () {
		GUI.Label(new Rect(10, 40, 100, 40), HP.ToString());
	}

}
