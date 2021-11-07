using UnityEngine;

[CreateAssetMenu(fileName = "Game/Configs/Create DialogCharacter")]
public class DialogCharacter : ScriptableObject
{
	#region Editor Variables

	[SerializeField]
	private Sprite _portrait = null;

	[SerializeField]
	private string _name = null;

	#endregion

	#region Properties

	public Sprite Portrait => _portrait;
	public string Name => _name;

	#endregion
}