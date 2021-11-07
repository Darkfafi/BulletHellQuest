using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    [SerializeField]
    private float _scrollSpeed = 5f;

    [SerializeField]
    private MeshRenderer _renderer = null;

    private float _offset = 0f;

    protected void Update()
    {
        _offset += (Time.deltaTime * _scrollSpeed) * 0.01f;
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(0, _offset));

    }
}
