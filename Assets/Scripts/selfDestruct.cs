using UnityEngine;
using System.Collections;

public class selfDestruct : MonoBehaviour {
	void Start () {
		Destroy (gameObject,5);
	}
}
