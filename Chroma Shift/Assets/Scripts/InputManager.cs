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
	public delegate void AxisEvent(float axisValue);

	//Input events
	public event KeyEvent Jump;
	public event AxisEvent Run;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//if the Jump button is pushed and there are any subscribers
		if (Input.GetButtonDown("Jump") && Jump != null)
		{
			//Call the Jump event
			Jump();
		}
		//if there are any Run Subscribers
		if (Run != null)
		{
			//Call the Run Event and link it to the horiztonal axis
			Run(Input.GetAxis("Horizontal"));
		}
	}
}
