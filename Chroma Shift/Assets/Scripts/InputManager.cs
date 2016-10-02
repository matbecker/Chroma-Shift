using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {


	public static InputManager instance;
	public static InputManager Instance
	{
		get
		{
			if (!instance) 
				instance = GameObject.FindObjectOfType (typeof(InputManager)) as InputManager;

			return instance;
		}

	}

	//delegate for input
	public delegate void KeyEvent();
	//public delegate void KeyEvent(float param);
	public delegate void AxisEvent(float axisValue);

	//Input events
	public event KeyEvent Jump;
	public event KeyEvent Attack;
	public event KeyEvent Block;
	public event KeyEvent Pause;
	public event AxisEvent Run;

	// Use this for initialization
	void Start () 
	{
	}

	void Update()
	{
		//if the Jump button is pushed and there are any subscribers
		if (Input.GetButtonDown("Jump") && Jump != null)
		{
			//Call the Jump event
			Jump();
		}
		//if the Attack Button is pushed and there are subscribers
		if (Input.GetButtonDown("Attack") && Attack != null)
		{
			//Call the Attack event
			Attack();
		}
		//if there are any Run Subscribers
		if (Run != null)
		{
			//Call the Run Event and link it to the horiztonal axis
			Run(Input.GetAxis("Horizontal"));
		}
		//if the Block Button is pushed and there are sunscribers
		if (Input.GetButton("Block") && Block != null)
		{
			//Call the block event
			Block();
		}
		//if the Pause Button is pushed and there are subscriberss
		if (Input.GetButtonDown("Pause") && Pause != null)
		{
			//Call the pause event
			Pause();
		}
	}
}
