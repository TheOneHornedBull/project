using UnityEngine;
using System.Collections;

public class touchInput : MonoBehaviour {

	private Vector3 touchPoint;
	private int HP = 120;
	private float nextFire;
	public GameObject playerBullet;
	
	void FixedUpdate () {

		if (HP <= 0) {
			Debug.Log("Game over ! You have been defeated.");
			Time.timeScale = 0;
		}

		MovementAndShooting ();
	}

	void MovementAndShooting () {

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);

			Shooting ();

			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved || Input.GetMouseButtonDown(0)) {
				Ray ray;
				RaycastHit hit;

				#if UNITY_EDITOR
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
				ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				
				#endif
				
				if(Physics.Raycast(ray,out hit)){
					touchPoint = hit.point;
				}
			}
			transform.position = Vector3.Lerp (transform.position, new Vector3 (touchPoint.x, transform.position.y, transform.position.z), Time.deltaTime * 100);
		}
	}

	void Shooting () {
		if (Time.time > nextFire) {
			nextFire = Time.time + 0.2f;
			Instantiate (playerBullet, transform.position, transform.rotation);
			Debug.Log("fire");
		}
	} 

	void OnTriggerEnter (Collider other){
		if (other.tag == "enemyBullet") {
			HP -= 5;
		}
	}

	void OnGUI () {
		GUI.Label(new Rect(10, 40, 100, 40), HP.ToString());
	}

}
