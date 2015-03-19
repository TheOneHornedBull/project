using UnityEngine;
using System.Collections;

public class mineScript : MonoBehaviour {
	public float speed;
	private Vector3 velocity = Vector3.down;

	void FixedUpdate () {
		transform.position = Vector3.SmoothDamp (transform.position, Vector3.down,ref velocity, speed);
	}

	void OnTirggerEnter (Collider other) {
		if (other.tag == "Player") {
			Destroy(gameObject);
		}
	}
}
