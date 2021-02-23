using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class TriggerEvent2D : UnityEvent<Collider2D> { }

public class TriggerCallback2D : MonoBehaviour
{
	[SerializeField] TriggerEvent2D triggerEnterEvent;
	public Collider2D collider;

	void Awake()
	{
		if (collider == null)
		{
			Debug.LogError(this.transform.root.gameObject.name + "以下の" + this.gameObject.name + "にCollider2Dがアタッチされていません。");	
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		triggerEnterEvent.Invoke(other);
	}

	public void AddOnTriggerEnterEvent2D(UnityAction<Collider2D> action)
	{
		triggerEnterEvent.AddListener(action);
	}
}
