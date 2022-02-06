using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class CollisionEvent : UnityEvent<Collision> { }

public class CollisionCallback : MonoBehaviour
{
	[SerializeField] CollisionEvent collisionEnterEvent;
	[SerializeField] CollisionEvent collisionStayEvent;
	[SerializeField] CollisionEvent collisionExitEvent;
	public Collider collider;

	void Awake()
	{
		if (collider == null)
		{
			collider = GetComponent<Collider>();

			if (collider == null)
				Debug.LogError(this.transform.root.gameObject.name + "以下の" + this.gameObject.name + "にColliderがアタッチされていません。");	
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		collisionEnterEvent.Invoke(other);
	}

	private void OnCollisionStay(Collision other)
	{
		collisionStayEvent.Invoke(other);
	}
	
	private void OnCollisionExit(Collision other)
	{
		collisionExitEvent.Invoke(other);
	}

	public void AddOnCollisionEnterEvent(UnityAction<Collision> action)
	{
		collisionEnterEvent.AddListener(action);
	}

	public void AddOnCollisionStayEvent(UnityAction<Collision> action)
	{
		collisionStayEvent.AddListener(action);
	}

	public void AddOnCollisionExitEvent(UnityAction<Collision> action)
	{
		collisionExitEvent.AddListener(action);
	}
	
	
}
