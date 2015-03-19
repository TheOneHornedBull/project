using UnityEngine;
using System.Collections;

public class sphereShieldScript : MonoBehaviour {
	private GameObject boss;
	void Start () {
		boss = GameObject.Find ("boss");
	}

	void FixedUpdate () {
		transform.position = boss.transform.position;
		transform.Rotate((Vector3.left + Vector3.back) * Time.deltaTime * 20);
	}
}
