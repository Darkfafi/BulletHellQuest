using System.Collections.Generic;
using UnityEngine;

public static class StateMachineUtils
{
	public static IState<TStatesParent>[] GetStates<TStatesParent>(this Transform self)
		where TStatesParent : IStatesParent
	{
		List<IState<TStatesParent>> states = new List<IState<TStatesParent>>();
		foreach (Transform child in self)
		{
			var state = child.GetComponent<IState<TStatesParent>>();
			if(state != null)
			{
				states.Add(state);
			}
		}
		return states.ToArray();
	}
}