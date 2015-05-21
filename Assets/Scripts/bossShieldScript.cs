using UnityEngine;
using System.Collections;

public class bossShieldScript : MonoBehaviour {
	public int bShieldHP = 5000;

	void Update () {
	
	}
	void onTriggerEnter (Collider other){
		if (other.tag == "playerBullet") {
			bShieldHP -= 20;
		} else if (other.tag == "playerElectricBullet") {
			bShieldHP -= 35;
		} else if (other.tag == "playerFireBullet") {
			StartCoroutine (takeFireDmg ());
		} else if (other.tag == "playerAcidBullet") {
			StartCoroutine (takeAcidDmg());
		}
	}
	IEnumerator takeFireDmg () {
		for (int i=0; i <= 5; i++){
			bShieldHP -= 5;
			yield return new WaitForSeconds (0.5f);
		}
		StopCoroutine (takeFireDmg());
	}
	IEnumerator takeAcidDmg () {
		for (int i=0;i <= 4; i++){
			bShieldHP -= 7;
			yield return new WaitForSeconds (0.8f);
		}
		StopCoroutine (takeAcidDmg());
	}
}
