using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour, IPlayerStateManager
{
    private List<PlayerStateType> states = new List<PlayerStateType>();

    public void StartState(PlayerStateType state)
    {
        if (states.Contains(state)) return;
        states.Add(state);
	}

    public void EndState(PlayerStateType state)
    {
        if (!states.Contains(state)) return;
        states.Remove(state);
	}

	public bool ContainsState(params PlayerStateType[] states)
	{
        bool contains = false;
        Array.ForEach(states, x =>
        {
            if(this.states.Contains(x))
            {
                contains = true;
                return;
            }
        });
        return contains;
	}

	public bool HaveAllState(params PlayerStateType[] states)
	{
        bool contains = true;
        Array.ForEach(states, x =>
        {
            if (!this.states.Contains(x))
            {
                contains = false;
                return;
            }
        });
        return contains;
    }
}
