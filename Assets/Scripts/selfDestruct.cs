using UnityEngine;
using System.Collections;

public class selfDestruct : MonoBehaviour {
	public float a=5;
	void Start () {
		Destroy (gameObject,a);
	}
}
