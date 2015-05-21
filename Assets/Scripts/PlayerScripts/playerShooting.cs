using UnityEngine;
using System.Collections;

public class playerShooting : MonoBehaviour {

	playerContrAndShoot dl;
	touchInput ti;
	public bool useRockets;
	public GameObject playerRockets;
	public GameObject playerBullet;
	public GameObject acidBullet;
	public GameObject fireBullet;
	public GameObject electricBullet;
	public enum bulletModifier {fireBullet, acidBullet, electricBullet, normalBullet};
	public bulletModifier bm;
	public GameObject bullet;
	float nextFire;
	public int rocketCount;
	float nextRocketFire;

	void Start () {
		dl = GetComponent<playerContrAndShoot>();
		ti = GetComponent<touchInput>();
	}

	void Update () {
		if (bm == bulletModifier.acidBullet) {
			bullet = acidBullet;
		} else if (bm == bulletModifier.electricBullet) {
			bullet = electricBullet;
		} else if (bm == bulletModifier.fireBullet) {
			bullet = fireBullet;
		} else if (bm == bulletModifier.normalBullet){
			bullet = playerBullet;
		}
		basicShooting ();
		rocketShooting ();
		rocketCount = ti.rocketCount;
	}

	void basicShooting () {
		if (Time.time > nextFire && dl.touching) {
			nextFire = Time.time + 0.2f;
			Instantiate (bullet, transform.position, transform.rotation);
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
