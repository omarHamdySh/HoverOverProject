using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SimpleTouchController : MonoBehaviour {

	// PUBLIC
	public delegate void TouchDelegate(Vector2 value);
	public event TouchDelegate TouchEvent;

	public delegate void TouchStateDelegate(bool touchPresent);
	public event TouchStateDelegate TouchStateEvent;

	[SerializeField]private Canvas canvas;
	public float deltaX = 1, deltaY = 1;
	private int flipX = 1, flipY = 1;

	// PRIVATE
	[SerializeField]
	private RectTransform joystickArea;
	private bool touchPresent = false;
	private Vector2 movementVector;
	private bool isEnable = false;
	public static SimpleTouchController instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		canvas = transform.parent.GetComponent<Canvas> ();
	}

	void LateUpdate()
	{
		if (isEnable) {

			if (GameManager.instance.IsGameOver () || GameManager.instance.IsGameCompleted ())
				canvas.enabled = false;
			else {
				if (!canvas.enabled)
					canvas.enabled = true;
			}

			return;
		}

		if(Timer.instance != null)
		{
			if(Timer.instance.IsReadyToPlay())
			{
				canvas.enabled = true;
				isEnable = true;
			}
		}
	}

	public Vector2 GetTouchPosition
	{
		get { return movementVector;}
	}


	public void BeginDrag()
	{
		touchPresent = true;
		if(TouchStateEvent != null)
			TouchStateEvent(touchPresent);
	}

	public void EndDrag()
	{
		touchPresent = false;
		movementVector = joystickArea.anchoredPosition = Vector2.zero;

		if(TouchStateEvent != null)
			TouchStateEvent(touchPresent);

	}

	void OnEnable()
	{
		UpdateFlip ();
	}


	public void UpdateFlip()
	{
		int x = 1;
		int y = 1;

//		print ("X = " + x);
//		print ("Y = " + y);

		if (x == 0)
			x = 1;
		else if (x == 1)
			x = -1;

		if (y == 0)
			y = 1;
		else if (y == 1)
			y = -1;
		

		flipX = x;
		flipY = y;
	}

	public void OnValueChanged(Vector2 value)
	{
		if(touchPresent)
		{
			// convert the value between 1 0 to -1 +1
			movementVector.x = (((1 - value.x) - 0.5f) * 2f) * deltaX * flipX;
			movementVector.y = (((1 - value.y) - 0.5f) * 2f) * deltaY * flipY;

			if(TouchEvent != null)
			{
				TouchEvent(movementVector);
			}
		}

	}

	public float GetHorizontal()
	{
		return movementVector.y;
	}

	public float GetVertical()
	{
		return movementVector.x;
	}

}
