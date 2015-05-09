using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent(typeof(UnityEngine.UI.Text))]
public class FPSCounter : MonoBehaviour {
	public float updateInterval = 0.5F;
	
	private Text fpsLabel;
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	
	void Start()
	{
		fpsLabel = GetComponent<Text>();
		
		timeleft = updateInterval;
	}
	
	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		if( timeleft <= 0.0 )
		{
			float fps = accum/frames;
			string format = System.String.Format("{0:F1} fps",fps);
			
			fpsLabel.text = format;
			
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}