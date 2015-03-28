using UnityEngine;
using System.Collections;

public class playerRocket : MonoBehaviour {
	private Vector3 velocity = Vector3.zero;
	private GameObject boss;
	
	void Start () {
		boss = GameObject.Find ("boss");
		Destroy (gameObject, 5);
	}
	void FixedUpdate () {
		transform.LookAt (boss.transform);
		transform.position = Vector3.SmoothDamp (transform.position, boss.transform.position, ref velocity, 0.5f);
	}

	void OnTriggerEnter (Collider other){
		if (other.tag == "boss") {
			Destroy(gameObject);
		}
	}

}
