using UnityEngine;

public abstract class MonoBehaviourStateBase<TParent> : MonoBehaviour, IState<TParent>
		where TParent : class, IStatesParent
{
	public TParent StateParent
	{
		get; private set;
	}

	public bool IsCurrentState
	{
		get; private set;
	}

	public virtual void Initialize(TParent parent)
	{
		StateParent = parent;
	}

	public virtual void Deinitialize()
	{
		StateParent = null;
	}

	public void Enter()
	{
		IsCurrentState = true;
		OnEnter();
	}

	public void Exit()
	{
		OnExit();
		IsCurrentState = false;
	}

	protected abstract void OnEnter();
	protected abstract void OnExit();
}