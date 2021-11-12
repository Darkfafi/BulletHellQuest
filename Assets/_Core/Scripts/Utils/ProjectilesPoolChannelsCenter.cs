using UnityEngine;

public class ProjectilesPoolChannelsCenter : MonoBehaviour
{
	[SerializeField]
	private ProjectilesPoolChannel[] _pools = new ProjectilesPoolChannel[0];

	protected void Awake()
	{
		for(int i = 0; i < _pools.Length; i++)
		{
			_pools[i].Init();
		}
	}

	protected void OnDestroy()
	{
		for (int i = _pools.Length - 1; i >= 0; i--)
		{
			_pools[i].Deinit();
		}
	}
}
