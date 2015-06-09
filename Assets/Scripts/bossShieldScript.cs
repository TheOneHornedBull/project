using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bossShieldScript : MonoBehaviour {
	public int bShieldHP = 5000;
	public Slider shieldSlider;
	public Text shieldText;
	GameObject shieldFill;

	void Start () {
		shieldFill = GameObject.Find ("BossShieldFill");
	}

	void Update () {
		shieldSlider.value = bShieldHP;
		if (bShieldHP > 3500) {
			shieldFill.GetComponent<Image>().color = new Color32 (0, 105, 105, 255);
		} else if (bShieldHP < 3500 && bShieldHP > 1500) {
			shieldFill.GetComponent<Image>().color = new Color32 (40, 160, 240, 255);
		} else {
			shieldFill.GetComponent<Image>().color = new Color32 (80, 140, 200, 255);
		}

        shieldText.text = bShieldHP.ToString() + " / " + bShieldHP.ToString();

		if (bShieldHP <= 0) {
			enabled = false;
		}

	}

    void OnTriggerEnter(Collider other){

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
			bShieldHP -= 5;
			yield return new WaitForSeconds (0.8f);
		}
		StopCoroutine (takeAcidDmg());
	}
}
