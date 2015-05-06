using UnityEngine;

public class DistanceLerp : MonoBehaviour 
{
	public float maxDistance = 30;			// 10
	public float threshold = 0.01f;				// 0.05
	public AnimationCurve easing;		// ease-in
	public AnimationCurve damping;		// ease-out, linear

	Ray inputRay;
	public bool useRockets;

	public bool touching;
	Plane touchPlane = new Plane(Vector3.back, 0f);
	
	Vector3 targetPosition;
	Rigidbody body;

	float nextFire;
	
	void Start()
	{
		targetPosition = transform.position;
		
		body = GetComponent<Rigidbody>();
		body.isKinematic = true;
	}
	
	void Update()
	{
		ProcessInput();
	}
	
	void FixedUpdate()
	{
		Vector3 delta = targetPosition - transform.position;
		
		float mt = Mathf.Clamp((delta.magnitude / maxDistance), threshold, 1f-threshold);
		float dampingTime = damping.Evaluate(mt);
		float easingTime = easing.Evaluate(mt * (1f-dampingTime));
		
		body.position = Vector3.Lerp(transform.position, targetPosition, easingTime);
	}
	
	void ProcessInput()
	{
		if (Input.GetMouseButtonDown(0))
			touching = true;
			
		if (Input.GetMouseButtonUp(0))
			touching = false;
		
		if (touching) {
			inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			float distance = 0f;

			if (touchPlane.Raycast (inputRay, out distance)) {
				targetPosition = inputRay.GetPoint (distance);
			}
		}
	}  
}
