using System;
using UnityEngine;

public class DialogGamePhaseState : GamePhaseStateBase
{
	[SerializeField]
	private DialogEntry[] _dialog;

	[SerializeField]
	private DialogUI _dialogUI = null;

	private int _currentIndex = 0;

	protected override void OnEnter()
	{
		_currentIndex = 0;
		ShowDialog();
	}

	protected override void OnExit()
	{
		if (_dialogUI != null)
		{
			_dialogUI.HideDialog();
		}
	}

	private void ShowDialog()
	{
		_dialogUI.ShowDialog(_dialog[_currentIndex].CreateData(OnContinueDialog));
	}

	private void OnContinueDialog()
	{
		_currentIndex++;
		if (_currentIndex >= _dialog.Length)
		{
			StateParent.GoToNextPhase();
		}
		else
		{
			ShowDialog();
		}
	}

	[Serializable]
	private struct DialogEntry
	{
		public DialogCharacter Character;
		[TextArea]
		public string DialogText;

		public DialogUI.DialogData CreateData(Action callback)
		{
			return DialogUI.DialogData.Create(DialogText)
				.SetPortrait(Character.Portrait)
				.SetName(Character.Name)
				.SetContinueCallback(callback);
		}
	}
}
