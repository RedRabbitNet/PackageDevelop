using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class CollisionEvent : UnityEvent<Collision> { }

public class CollisionCallback : MonoBehaviour
{
	[SerializeField] CollisionEvent collisionEnterEvent;
	public Collider collider;

	void Awake()
	{
		if (collider == null)
		{
			Debug.LogError(this.transform.root.gameObject.name + "以下の" + this.gameObject.name + "にCollider2Dがアタッチされていません。");	
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		collisionEnterEvent.Invoke(other);
	}

	public void AddOnCollisionEnterEvent(UnityAction<Collision> action)
	{
		collisionEnterEvent.AddListener(action);
	}
}
