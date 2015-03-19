using UnityEngine;
using System.Collections;

public class playerBulletScript : MonoBehaviour {
	void Start () {
		GetComponent<Rigidbody>().velocity = transform.up * 30;
		Destroy (gameObject,3);
	}
	void OnTriggerEnter (Collider other) {
		if (other.tag == "boss") {
			Destroy (gameObject);
		}
	}
}
