using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class CollisionEvent2D : UnityEvent<Collision2D> { }

public class CollisionCallback2D : MonoBehaviour
{
	[SerializeField] CollisionEvent2D collisionEnterEvent;

	private void OnCollisionEnter2D(Collision2D other)
	{
		collisionEnterEvent.Invoke(other);
	}

	public void AddOnCollisionEnterEvent2D(UnityAction<Collision2D> action)
	{
		collisionEnterEvent.AddListener(action);
	}
}
