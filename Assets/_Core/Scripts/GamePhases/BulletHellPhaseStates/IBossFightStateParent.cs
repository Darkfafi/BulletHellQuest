public interface IBossFightStateParent : IStatesParent
{
	BossShip BossInstance
	{
		get;
	}

	Health BossHealth
	{
		get;
	}

	void GoToNextState();
}