using UnityEngine;
using System.Collections;

public class playerRocket : MonoBehaviour {
	private GameObject boss;
	private float step;
	public float speed = 25f;
	
	void Start () {
		boss = GameObject.Find ("boss");
		Destroy (gameObject, 5);
		step = speed * Time.deltaTime;
	}
	void FixedUpdate () {
		transform.LookAt (boss.transform);
		transform.position = Vector3.MoveTowards (transform.position, boss.transform.position, step);
	}

	void OnTriggerEnter (Collider other){
		if (other.tag == "boss") {
			Destroy(gameObject);
		}
	}

}
