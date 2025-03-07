using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Evetns/ Game Event")]
public class GameEvent : ScriptableObject
{
    public UnityAction<int> OnEventRaised;
	public void RaiseEvent(int value)
	{
		OnEventRaised?.Invoke(value);
	}
}
