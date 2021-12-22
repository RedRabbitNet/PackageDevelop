using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class TriggerEvent2D : UnityEvent<Collider2D> { }

public class TriggerCallback2D : MonoBehaviour
{
	[SerializeField] TriggerEvent2D triggerEnterEvent;
	[SerializeField] TriggerEvent2D triggerStayEvent;
	[SerializeField] TriggerEvent2D triggerExitEvent;
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
	
	private void OnTriggerStay2D(Collider2D other)
	{
		triggerStayEvent.Invoke(other);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		triggerExitEvent.Invoke(other);
	}

	public void AddOnTriggerEnterEvent2D(UnityAction<Collider2D> action)
	{
		triggerEnterEvent.AddListener(action);
	}

	public void AddOnTriggerStayEvent2D(UnityAction<Collider2D> action)
	{
		triggerStayEvent.AddListener(action);
	}

	public void AddOnTriggerExitEvent2D(UnityAction<Collider2D> action)
	{
		triggerExitEvent.AddListener(action);
	}
}
