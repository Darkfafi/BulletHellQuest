public interface IState<TParent>
		where TParent : class, IStatesParent
{
	TParent StateParent
	{
		get;
	}

	bool IsCurrentState
	{
		get;
	}

	void Initialize(TParent parent);
	void Deinitialize();
	void Enter();
	void Exit();
}
