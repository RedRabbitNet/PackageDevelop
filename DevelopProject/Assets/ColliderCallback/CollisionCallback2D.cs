using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class CollisionEvent2D : UnityEvent<Collision2D> { }

public class CollisionCallback2D : MonoBehaviour
{
	[SerializeField] CollisionEvent2D collisionEnterEvent;
	[SerializeField] CollisionEvent2D collisionStayEvent;
	[SerializeField] CollisionEvent2D collisionExitEvent;
	public Collider2D collider;

	void Awake()
	{
		if (collider == null)
		{
			Debug.LogError(this.transform.root.gameObject.name + "以下の" + this.gameObject.name + "にCollider2Dがアタッチされていません。");	
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		collisionEnterEvent.Invoke(other);
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		collisionStayEvent.Invoke(other);
	}
	
	private void OnCollisionExit2D(Collision2D other)
	{
		collisionExitEvent.Invoke(other);
	}

	public void AddOnCollisionEnterEvent2D(UnityAction<Collision2D> action)
	{
		collisionEnterEvent.AddListener(action);
	}

	public void AddOnCollisionStayEvent2D(UnityAction<Collision2D> action)
	{
		collisionStayEvent.AddListener(action);
	}

	public void AddOnCollisionExitEvent2D(UnityAction<Collision2D> action)
	{
		collisionExitEvent.AddListener(action);
	}
}
