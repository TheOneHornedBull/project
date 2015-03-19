using UnityEngine;
using System.Collections;

public class tapPlateScript : MonoBehaviour {
	private GameObject player;
	private Transform target;
	void Start () {
		player = GameObject.Find ("player");
		target = player.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = new Vector3 (target.position.x, target.position.y, transform.position.z);
	}
}
