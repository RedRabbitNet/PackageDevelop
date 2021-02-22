using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class CollisionEvent : UnityEvent<Collision> { }

public class CollisionCallback : MonoBehaviour
{
	[SerializeField] CollisionEvent collisionEnterEvent;

	private void OnCollisionEnter(Collision other)
	{
		collisionEnterEvent.Invoke(other);
	}

	public void AddOnCollisionEnterEvent(UnityAction<Collision> action)
	{
		collisionEnterEvent.AddListener(action);
	}
}
