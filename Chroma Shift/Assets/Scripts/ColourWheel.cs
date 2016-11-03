using UnityEngine;
using System.Collections;

public class ColourWheel : MonoBehaviour {

	[SerializeField] int randDirection;
	[SerializeField] float rotationSpeed;
	[SerializeField] float spinTime;
	[SerializeField] int rotationAngle;
	private float timer;
	private bool startSpinning;
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
		//set variables up

		//coin flip to determine if the wheel should spin right or left
		randDirection = Random.Range(0,2);
		//wheel will spin at random speed between 50 and 100
		rotationSpeed = Random.Range(50.0f, 100.0f);
		//spin for 5 to 10 seconds
		spinTime = Random.Range(5.0f,10.0f);

		startSpinning = true;

	}
}
