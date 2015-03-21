using UnityEngine;
using System.Collections;

public class crates : MonoBehaviour {
	void Start () {
		GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * 6;
		GetComponent<Rigidbody> ().velocity = transform.up * -8;
		Destroy (gameObject, 6);
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			Destroy(gameObject);
		}
	}
}