using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ColourWheel : MonoBehaviour {

	public enum ColourType { Purple, Blue, Green, Yellow, Orange, Red };
	public ColourType currentColourTop;
	public ColourType currentColourBottom;

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
	public bool startSpinning;
	private string[] colors;

	// Use this for initialization
	void Start () 
	{
		startSpinning = false;

		colors = new string[2];
	}
	
	// Update is called once per frame
	void Update () 
	{
		rotationAngle = Mathf.RoundToInt(transform.localEulerAngles.x);

		if (startSpinning)
		{
			timer += Time.deltaTime;

			if (timer >= spinTime)
			{
				SlowDown();
			}

			Spin();
		}
		if (!startSpinning)
			Shift();
	}
	void Spin()
	{
		//spin right
		if (randDirection == 0)
			gameObject.transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
		else //spin left
			gameObject.transform.Rotate(Vector3.left * (rotationSpeed * Time.deltaTime));

		rotationAngle = Mathf.RoundToInt(transform.localEulerAngles.x);
	}
	void SlowDown()
	{
		//rotationSpeed -= 0.1f;

		if (rotationAngle % 30 == 0)
			StopSpinning();
	}
	void StopSpinning()
	{
		var angle = (rotationAngle + 360) % 360;
		faceIndex = (int)(angle / 30f);

		colors = colourWheelFaceColours[faceIndex].name.Split('_');

		currentColourTop = ParseColour(colors[0]);
		currentColourBottom = ParseColour(colors[1]);

		Debug.Log(currentColourTop.ToString());
		Debug.Log(currentColourBottom.ToString());

		//reset variables
		rotationSpeed = 0;
		timer = 0.0f;
	}
	public void Shift()
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
	private ColourType ParseColour(string colour)
	{
		return (ColourType)Enum.Parse(typeof(ColourType), colour);
	}
}
