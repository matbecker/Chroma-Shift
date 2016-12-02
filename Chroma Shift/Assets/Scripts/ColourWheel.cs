using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ColourWheel : MonoBehaviour {

	private static ColourWheel instance;
	public static ColourWheel Instance
	{
		get
		{
			if (!instance)
				instance = GameObject.FindObjectOfType(typeof(ColourWheel)) as ColourWheel;

			return instance;
		}
	}
	[SerializeField] List<Material> colourWheelFaceColours;
	[SerializeField] int faceIndex;
	[SerializeField] int randDirection;
	[SerializeField] float rotationSpeed;
	[SerializeField] float spinTime;
	[SerializeField] int rotationAngle;
	private float timer;
	private bool startSpinning;
	private Char delimiter = '_';

	// Use this for initialization
	void Start () 
	{
		startSpinning = false;

	}
	
	// Update is called once per frame
	void Update () 
	{
		rotationAngle = (int)transform.localEulerAngles.x;

		if (startSpinning)
		{
			Spin();

			timer += Time.deltaTime;

			if (timer >= spinTime)
			{
				SlowDown();
			}
		}
		if (!startSpinning)
			Shift();

		if (rotationAngle % 30 == 0)
			faceIndex++;

		if (faceIndex > colourWheelFaceColours.Count)
			faceIndex = 0;

		Debug.Log(faceIndex);

		foreach (string colourString in colourWheelFaceColours[faceIndex].name)
		{
			//string[] split = colourString.Split(delimiter);
			colourString.Split(delimiter, 2);
		}
			
	}
	void Spin()
	{
		//spin right
		if (randDirection == 0)
			gameObject.transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
		else //spin left
			gameObject.transform.Rotate(Vector3.left * (rotationSpeed * Time.deltaTime));
	}
	void SlowDown()
	{
		//rotationSpeed -= 0.1f;

		if (rotationAngle % 30 == 0)
			StopSpinning();
	}
	void StopSpinning()
	{
		//reset variables
		rotationSpeed = 0;
		//startSpinning = false;
		timer = 0.0f;
	}
	void Shift()
	{
		//coin flip to determine if the wheel should spin right or left
		randDirection = UnityEngine.Random.Range(0,2);
		//wheel will spin at random speed between 50 and 100
		rotationSpeed = UnityEngine.Random.Range(50.0f, 100.0f);
		//spin for 5 to 10 seconds
		spinTime = UnityEngine.Random.Range(3.0f,6.0f);
		//start spinning the wheel
		startSpinning = true;

	}
}
