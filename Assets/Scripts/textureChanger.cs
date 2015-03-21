using UnityEngine;
using System.Collections;

public class textureChanger : MonoBehaviour {
	public UnityEngine.Texture red;
	public UnityEngine.Texture yellow;
	private Renderer r;

	void Start () {
		r = GetComponent<Renderer> ();
	}

	void FixedUpdate () {
		if (Random.Range (1, 5) <= 2) {
			r.material.mainTexture = yellow;
		} else {
			r.material.mainTexture = red;
		}
	}

}
