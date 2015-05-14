using UnityEngine;
using System.Collections;

public class playerShooting : MonoBehaviour {

	playerContrAndShoot dl;
	touchInput ti;
	public bool useRockets;
	public GameObject playerRockets;
	public GameObject playerBullet;
	float nextFire;
	public int rocketCount;
	float nextRocketFire;

	void Start () {
		dl = GetComponent<playerContrAndShoot>();
		ti = GetComponent<touchInput>();
	}

	void Update () {
		basicShooting ();
		rocketShooting ();
		rocketCount = ti.rocketCount;
	}

	void basicShooting () {
		if (Time.time > nextFire && dl.touching) {
			nextFire = Time.time + 0.2f;
			Instantiate (playerBullet, transform.position, transform.rotation);
		}
	}

		void rocketShooting () {
		if (Time.time > nextRocketFire && useRockets == true && rocketCount <= 25 && dl.touching) {
			nextRocketFire = Time.time + 0.4f;
			Instantiate (playerRockets, transform.position, transform.rotation);
			rocketCount ++;
			ti.rocketCount ++;
		}
	}
}
