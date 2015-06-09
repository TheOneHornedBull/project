using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class touchInput : MonoBehaviour {
	 
	public GameObject sparks;
	public Slider playerHPBar;
	public Text HPText;
	public GameObject startButton;
	public bool rockets;
	public int rocketCount;
	Vector3 sparksPosition;
	GameObject playerBody;
	Vector3 touchPoint;
	Quaternion startRotation;
	int HP = 300;
	RaycastHit hit;
	GameObject fill;
	bool isPaused;
    playerShooting ps;

	void Awake () {
		Time.timeScale = 0;
		isPaused = true;
		fill = GameObject.Find ("PlayerFill");
		playerBody = GameObject.Find ("playerBody");
        ps = GetComponent<playerShooting>();
	}
	
	void FixedUpdate () {

		if (HP == 200 || HP == 150 || HP == 100 || HP == 50) {
			HP -= 5;
			StartCoroutine (colorChanger());
		}

		playerHPBar.value = HP;
		if (HP >= 200) {
			fill.GetComponent<Image>().color = new Color32 (10, 255, 0, 255);
		}else if (HP < 150 && HP >= 50) {
			fill.GetComponent<Image>().color = new Color32 (255, 180, 40, 255);
		}else if (HP < 50) {
			fill.GetComponent<Image>().color = new Color32 (255, 0, 0, 255);
		}
		HPText.text = "300 / " + HP.ToString ();
		if (HP <= 0) {
			Debug.Log("Game over ! You have been defeated.");
			Time.timeScale = 0;
		}

	}

	void OnTriggerEnter (Collider other){
		if (other.tag == "enemyBullet") {
			HP -= 5;

			if (Random.Range(1,5) == 1) {
				sparksPosition = new Vector3 (0.5f,-1,0);
			}
			if (Random.Range(1,5) == 2){
				sparksPosition = new Vector3 (-0.5f,-1,0);
			}
			if (Random.Range(1,5) == 3){
				sparksPosition = new Vector3 (0.15f,0.45f,0);
			}
			if (Random.Range(1,5) == 4){
				sparksPosition = new Vector3 (-0.5f,0.25f,0);
			}
			Instantiate(sparks, sparksPosition + transform.position,transform.rotation);
		}

		if (other.tag == "HPCrate") {
			HP += HP/2;
			if (HP > 300){
				HP = 300;
			}
		}

		if (other.tag == "RocketCrate") {
			rockets = true;
			rocketCount = 0;

            if (ps.bm == playerShooting.bulletModifier.fireBullet){
              if (  Random.Range (1,3) == 1) {
                    ps.bm = playerShooting.bulletModifier.acidBullet;
                }else {
                    ps.bm = playerShooting.bulletModifier.electricBullet;
                }
            }else if (ps.bm == playerShooting.bulletModifier.acidBullet){
                if (  Random.Range (1,3) == 1) {
                    ps.bm = playerShooting.bulletModifier.fireBullet;
                }else {
                    ps.bm = playerShooting.bulletModifier.electricBullet;
                }
            }else {
                if (  Random.Range (1,3) == 1) {
                    ps.bm = playerShooting.bulletModifier.fireBullet;
                }else {
                    ps.bm = playerShooting.bulletModifier.acidBullet;
                }
            }
		}

	}

	IEnumerator colorChanger () {
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,75,75,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,255,255,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,75,75,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,255,255,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,75,75,255);
		yield return new WaitForSeconds (0.1f);
		playerBody.GetComponent<Renderer>().material.color = new Color32(255,255,255,255);
		yield return new WaitForSeconds (0.1f);
		StopCoroutine (colorChanger ());
	}

	public void Play () {
		Time.timeScale = 1;
		isPaused = false;
		Destroy (startButton);
	}

	public void Pause () {
		if (isPaused == true) {
			Time.timeScale = 1;
			isPaused = false;
		} else {
			Time.timeScale = 0;
			isPaused = true;
		}
	}

	public void Restart () {
		Time.timeScale = 0;
		Application.LoadLevel (0);
	}
}
