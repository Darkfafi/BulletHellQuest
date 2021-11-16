using System;

public class FiniteStateMachine<TStatesParent> : IDisposable
	where TStatesParent : IStatesParent
{
	public readonly IState<TStatesParent>[] States;
	public readonly TStatesParent StatesParent;

	public IState<TStatesParent> CurrentState
	{
		get; private set;
	}

	public bool IsRunning => CurrentState != null;

	public FiniteStateMachine(TStatesParent parent, IState<TStatesParent>[] states, bool startStateMachine = true)
	{
		StatesParent = parent;
		States = states;

		for (int i = 0; i < States.Length; i++)
		{
			IState<TStatesParent> state = States[i];
			state.Initialize(StatesParent);
		}

		if(startStateMachine)
		{
			StartStateMachine();
		}
	}

	public void StartStateMachine()
	{
		if(!IsRunning && States.Length > 0)
		{
			SetState(States[0]);
		}
	}

	public void StopStateMachine()
	{
		if (IsRunning)
		{
			ClearState();
		}
	}

	public bool GoToNextState()
	{
		int nextStateIndex = Array.IndexOf(States, CurrentState) + 1;
		if (nextStateIndex < 0 || nextStateIndex >= States.Length)
		{
			SetState(null);
			return false;
		}
		else
		{
			SetState(States[nextStateIndex]);
			return true;
		}
	}

	public void SetState(IState<TStatesParent> state)
	{
		if (state != null && state.StateParent != null && (StatesParent == null || !state.StateParent.Equals(StatesParent)))
		{
			throw new Exception($"State is not part of StateMachine `{state.StateParent}` != `{StatesParent}`");
		}

		ClearState();

		CurrentState = state;

		if (CurrentState != null)
		{
			CurrentState.Enter();
		}
	}

	public void ClearState()
	{
		if (CurrentState != null)
		{
			CurrentState.Exit();
			CurrentState = null;
		}
	}

	public void Dispose()
	{
		StopStateMachine();

		for (int i = States.Length - 1; i >= 0; i--)
		{
			IState<TStatesParent> state = States[i];
			state.Deinitialize();
		}
	}
}
