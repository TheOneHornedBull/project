using UnityEngine;
using System.Collections;

public class cameraFollowScript : MonoBehaviour {

	private GameObject target;
	private float verticalOffset ;
	
	void Start () {
		verticalOffset = transform.position.y;
		target = GameObject.Find("player");
	}
	
	void LateUpdate () {
			float targetHeight = target.transform.position.y + verticalOffset;
			transform.position  = new Vector3 (transform.position.x,targetHeight,transform.position.z);
	}
}
