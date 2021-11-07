using System;
using UnityEngine;

public class GameManager : MonoBehaviour, IStatesParent
{
	#region Editor Variables

	[SerializeField]
	private DialogUI _dialogUI = null;

	#endregion

	#region Variables

	private FiniteStateMachine<GameManager> _fsm = null;
	private InputSystem _inputSystem = null;

	#endregion

	#region Lifecycle

	protected void Awake()
	{
		_fsm = new FiniteStateMachine<GameManager>(this, GetComponentsInChildren<GamePhaseStateBase>(), false);

		_inputSystem = InputSystem.Instance;
		_inputSystem.ActionDownEvent += OnActionDownEvent;
	}

	protected void Start()
	{
		_fsm.StartStateMachine();
	}

	protected void OnDestroy()
	{
		_inputSystem.ActionDownEvent -= OnActionDownEvent;
		_inputSystem = null;

		_fsm.Dispose();
	}

	private void OnActionDownEvent(InputSystem.ActionType actionType)
	{
		if (_dialogUI.IsBeingShown)
		{
			_dialogUI.ContinueDialog();
		}
	}

	#endregion

	#region Public Methods

	public void GoToNextPhase()
	{
		_fsm.GoToNextState();
	}

	#endregion
}
