using UnityEngine;

[CreateAssetMenu( menuName = "Channels/ProjectilesPoolChannel", fileName = "ProjectilesPoolChannel")]
public class ProjectilesPoolChannel : ScriptableObject
{
	[SerializeField]
	private int _initialPoolCount = 200;

	[SerializeField]
	private Projectile _prefab = null;

	private ProjectilesPool _pool = null;

	public ProjectilesPool ProjectilesPool
	{
		get
		{
			Init();
			return _pool;
		}
	}

	public void Init()
	{
		if (_pool == null)
		{
			_pool = new GameObject($"<{name} Pool>").AddComponent<ProjectilesPool>();
			_pool.Init(_prefab, _initialPoolCount);
			DontDestroyOnLoad(ProjectilesPool);
		}
	}

	public void Deinit()
	{
		if (_pool != null)
		{
			Destroy(_pool.gameObject);
			_pool = null;
		}
	}
}
