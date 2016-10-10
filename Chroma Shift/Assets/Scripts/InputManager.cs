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
	public delegate void KeyDownEvent();
	public delegate void KeyUpEvent();
	//delegate for input with a bool parameter. The bool is needed because the function will have 2 seperate events
	public delegate void AdvancedKeyEvent(bool param);
	//public delegate void KeyEvent(float param);
	public delegate void AxisEvent(float axisValue);

	//Input events
	public event KeyDownEvent Jump;
	public event KeyDownEvent DoubleJump;
	public event KeyDownEvent Hover;
	public event KeyDownEvent Attack;
	public event KeyDownEvent Block;
	public event KeyUpEvent UnBlock;
	public event KeyDownEvent Pause;
	public event AxisEvent Run;
	private string buttonName;

	// Use this for initialization
	void Start () 
	{
		buttonName = "";
	}

	void Update()
	{
		//if the Jump button is pushed and there are any subscribers
		if (Input.GetButtonDown("Jump"))
		{
			//subscribe all different jump event;
			if (Jump != null)
				Jump();

			if (DoubleJump != null)
				DoubleJump();

			if (Hover != null)
				Hover();
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
		//if the Block Button is pushed and there are subscribers
		if (Input.GetButtonDown("Block") && Block != null)
		{
			//Call the block event
			Block();
		}
		if (Input.GetButtonUp("Block") && UnBlock != null)
		{
			UnBlock();
		}
		//if the Pause Button is pushed and there are subscriberss
		if (Input.GetButtonDown("Pause") && Pause != null)
		{
			//Call the pause event
			Pause();
		}
	}
}
