using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class CollisionEvent2D : UnityEvent<Collision2D> { }

public class CollisionCallback2D : MonoBehaviour
{
	[SerializeField] CollisionEvent2D collisionEnterEvent;
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

	public void AddOnCollisionEnterEvent2D(UnityAction<Collision2D> action)
	{
		collisionEnterEvent.AddListener(action);
	}
}
