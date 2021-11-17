using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

	protected void Update()
	{
		transform.Rotate(Vector3.forward, Time.deltaTime * _speed);
	}
}
