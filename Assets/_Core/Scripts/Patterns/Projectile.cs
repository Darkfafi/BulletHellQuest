using UnityEngine;

public class Projectile : MonoBehaviour
{
	#region Editor Variables

	[SerializeField]
	private Alignment _alignment = default;

	[SerializeField]
	private float _colliderRadius = 1f;

	#endregion

	#region Properties

	public Alignment Alignment => _alignment;

	public float ColliderRadius => _colliderRadius;

	#endregion

	protected void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, _colliderRadius);
	}
}