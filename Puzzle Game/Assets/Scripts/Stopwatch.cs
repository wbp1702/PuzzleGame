using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{
	public Text text;

	private float startTime;

	private void Start()
	{
		
		startTime = Time.time;
	}

	private void Update()
	{
		float diff = Time.time - startTime;
		text.text = string.Format("Time {0:00}:{1:00}", (int)(diff / 60f), (int)(diff % 60f));
	}

	public float CurrentTime()
	{
		return Time.time - startTime;
	}
}
