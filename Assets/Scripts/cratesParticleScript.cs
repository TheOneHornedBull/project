using UnityEngine;
using System.Collections;

public class cratesParticleScript : MonoBehaviour {
	public GameObject crate;

	void Update () {
		if (crate == null){
			Destroy(gameObject, 5);
		}
	}
}
