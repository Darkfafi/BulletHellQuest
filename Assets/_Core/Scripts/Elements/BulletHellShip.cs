using System;
using UnityEngine;

public class BulletHellShip : MonoBehaviour
{
	[SerializeField]
	private float _speed = 3f;

	protected void Awake()
	{
		InputSystem.Instance.ActionDownEvent += OnActionDownEvent;
	}

	protected void Update()
	{
		InputSystem inputSystem = InputSystem.Instance;
		transform.Translate(new Vector3(inputSystem.Horizontal, inputSystem.Vertical).normalized * Time.deltaTime * _speed);
	}

	private void OnActionDownEvent(InputSystem.ActionType actionType)
	{
		Debug.Log(actionType);
	}
}
