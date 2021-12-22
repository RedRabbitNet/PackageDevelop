using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class TriggerEvent : UnityEvent<Collider> { }

public class TriggerCallback : MonoBehaviour
{
	[SerializeField] TriggerEvent triggerEnterEvent;
	[SerializeField] TriggerEvent triggerStayEvent;
	[SerializeField] TriggerEvent triggerExitEvent;
	public Collider collider;

	void Awake()
	{
		if (collider == null)
		{
			Debug.LogError(this.transform.root.gameObject.name + "以下の" + this.gameObject.name + "にCollider2Dがアタッチされていません。");	
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		triggerEnterEvent.Invoke(other);
	}

	private void OnTriggerStay(Collider other)
	{
		triggerStayEvent.Invoke(other);
	}

	private void OnTriggerExit(Collider other)
	{
		triggerExitEvent.Invoke(other);
	}

	public void AddOnTriggerEnterEvent(UnityAction<Collider> action)
	{
		triggerEnterEvent.AddListener(action);
	}

	public void AddOnTriggerStayEvent(UnityAction<Collider> action)
	{
		triggerStayEvent.AddListener(action);
	}

	public void AddOnTriggerExitEvent(UnityAction<Collider> action)
	{
		triggerExitEvent.AddListener(action);
	}
}
