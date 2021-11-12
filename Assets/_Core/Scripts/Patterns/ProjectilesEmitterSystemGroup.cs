#if UNITY_EDITOR
#endif
using UnityEngine;

public class ProjectilesEmitterSystemGroup : MonoBehaviour
{
	[SerializeField]
	private ProjectilesEmitterSystem[] _emitters = new ProjectilesEmitterSystem[0];

	public void StartEmitters()
	{
		for(int i = 0; i < _emitters.Length; i++)
		{
			_emitters[i].StartEmitter();
		}
	}

	public void StopEmitters()
	{
		for (int i = 0; i < _emitters.Length; i++)
		{
			_emitters[i].StopEmitter();
		}
	}
}


