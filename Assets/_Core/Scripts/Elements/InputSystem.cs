using System;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
	public event Action<ActionType> ActionDownEvent;
	public event Action<ActionType> ActionUpEvent;

	private const string HorizontalConst = "Horizontal";
	private const string VerticalConst = "Vertical";

	public static InputSystem Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameObject("<InputSystem>").AddComponent<InputSystem>();
				DontDestroyOnLoad(_instance);
			}
			return _instance;
		}
	}

	private static InputSystem _instance = null;

	protected void Update()
	{
		Horizontal = Input.GetAxisRaw(HorizontalConst);
		Vertical = Input.GetAxisRaw(VerticalConst);

		ActionsPressed = 0;

		ActionType[] actions = Enum.GetValues(typeof(ActionType)) as ActionType[];

		for (int i = 0; i < actions.Length; i++)
		{
			ActionType action = actions[i];
			string actionName = action.ToString();

			if (Input.GetButton(actionName))
			{
				ActionsPressed |= action;
			}

			if (Input.GetButtonDown(actionName))
			{
				ActionDownEvent?.Invoke(action);
			}

			if (Input.GetButtonUp(actionName))
			{
				ActionUpEvent?.Invoke(action);
			}
		}
	}

	public float Horizontal
	{
		get; private set;
	}

	public float Vertical
	{

		get; private set;
	}

	public ActionType ActionsPressed
	{
		get; private set;
	}

	public bool IsActionPressed(ActionType action)
	{
		return ActionsPressed.HasFlag(action);
	}

	public enum ActionType
	{
		Fire1 = 1,
		Fire2 = 2
	}
}