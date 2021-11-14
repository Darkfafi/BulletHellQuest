public interface IState<TParent>
		where TParent : IStatesParent
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