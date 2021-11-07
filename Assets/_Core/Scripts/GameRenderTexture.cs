using UnityEngine;

public class GameRenderTexture : MonoBehaviour
{
    [SerializeField]
    private Camera _camera = null;

	protected void Awake()
	{
        _camera.aspect = 9f / 16f;
    }
}