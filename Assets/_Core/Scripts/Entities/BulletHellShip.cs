﻿using UnityEngine;

public class BulletHellShip : MonoBehaviour
{
	[SerializeField]
	private float _speed = 3f;

	[SerializeField]
	private ProjectilesEmitterSystemGroup _emitter = null;

	protected void Update()
	{
		InputSystem inputSystem = InputSystem.Instance;

		float speed = _speed;
		if (inputSystem.IsActionPressed( InputSystem.ActionType.Fire2))
		{
			speed *= 0.5f;
		}

		transform.Translate(new Vector3(inputSystem.Horizontal, inputSystem.Vertical).normalized * Time.deltaTime * speed);

		if(inputSystem.IsActionPressed( InputSystem.ActionType.Fire1))
		{
			_emitter.StartEmitters();
		}
		else
		{
			_emitter.StopEmitters();
		}
	}
}