using UnityEngine;
using System.Collections;

public class bossMovementScript : MonoBehaviour {
	public float maxSpeed;
	public bool move;
	public bool moveDefault;
	public bool defaultLeft;
	public Vector3 bounds;
	public float timeToReachTarget;
	public Vector3 nextPosition;
	Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void defaultMovement(){
		Vector3 vel = Vector3.zero;
		if (move == false) {
			moveDefault = false;
		} else {
			transform.position = Vector3.SmoothDamp (transform.position, nextPosition, ref vel, Time.deltaTime * timeToReachTarget, maxSpeed);
		}

		if (moveDefault) {
			if (defaultLeft) {
				if(transform.position.x > bounds.x - 0.2f){
					bounds = bounds*-1;
					defaultLeft = false;
				}
			}
			if (defaultLeft == false) {
				if(transform.position.x < bounds.x + 0.2f){
					bounds = bounds*-1;
					defaultLeft = true;
				}
			}
			nextPosition = new Vector3 (bounds.x, 20, 0);
			transform.position = Vector3.SmoothDamp (new Vector3 (transform.position.x, 20, 0), nextPosition, ref vel, Time.deltaTime * timeToReachTarget, maxSpeed);
		}

	}

}
