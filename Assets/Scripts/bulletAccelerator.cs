using UnityEngine;
using System.Collections;

public class bulletAccelerator : MonoBehaviour {
	void Start () {
		GetComponent<Rigidbody>().velocity = transform.up * -20;
		Destroy (gameObject,3);
	}
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			Destroy (gameObject);
		}
	}
}
