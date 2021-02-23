using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable] public class TriggerEvent : UnityEvent<Collider> { }

public class TriggerCallback : MonoBehaviour
{
	[SerializeField] TriggerEvent triggerEnterEvent;
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

	public void AddOnTriggerEnterEvent(UnityAction<Collider> action)
	{
		triggerEnterEvent.AddListener(action);
	}
}
