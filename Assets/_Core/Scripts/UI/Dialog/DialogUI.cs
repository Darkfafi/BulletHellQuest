using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class DialogUI : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private GameObject _container = null;
	[SerializeField]
	private Image _portraitImage = null;
	[SerializeField]
	private TextMeshProUGUI _textLabel = null;
	[SerializeField]
	private TextMeshProUGUI _nameLabel = null;
	[SerializeField]
	private GameObject _continueArrow = null;

	[SerializeField]
	private AudioSource _audioSource = null;
	[SerializeField]
	private AudioClip _charSound = null;

	private Coroutine _revealTextRoutine = null;
	private Coroutine _durationRoutine = null;
	private DialogData _dialogData;

	private float _revealDuration = 1f;

	public bool IsBeingShown
	{
		get; private set;
	}

	protected void Awake()
	{
		HideDialog();
	}

	public void ShowDialog(DialogData dialogData)
	{
		HideDialog();

		IsBeingShown = true;
		_container.SetActive(true);
		_dialogData = dialogData;

		_portraitImage.sprite = _dialogData.Portrait;
		_portraitImage.gameObject.SetActive(_dialogData.Portrait != null);

		_textLabel.text = FormatDisplayText(0f, out _);
		_nameLabel.text = _dialogData.Name;

		_revealDuration = 1f;

		if (_dialogData.Duration > 0f)
		{
			_durationRoutine = StartCoroutine(DurationRoutine(_dialogData.Duration));
			_revealDuration = Mathf.Min(_dialogData.Duration * 0.5f, _revealDuration);
		}

		_revealTextRoutine = StartCoroutine(RevealTextRoutine());
	}

	public void ContinueDialog()
	{
		if (_dialogData.IsSkippable)
		{
			if (_revealTextRoutine != null)
			{
				StopCoroutine(_revealTextRoutine);
				_revealTextRoutine = null;
				_textLabel.text = FormatDisplayText(1f, out _);
				_continueArrow.SetActive(true);
			}
			else
			{
				Action callback = _dialogData.Callback;
				HideDialog();
				callback?.Invoke();
			}
		}
		
	}

	public void HideDialog()
	{
		_container.SetActive(false);

		if (_continueArrow != null)
		{
			_continueArrow.SetActive(false);
		}

		if (_revealTextRoutine != null)
		{
			StopCoroutine(_revealTextRoutine);
			_revealTextRoutine = null;
		}

		if (_durationRoutine != null)
		{
			StopCoroutine(_durationRoutine);
			_durationRoutine = null;
		}
		IsBeingShown = false;

		_dialogData = default;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		ContinueDialog();
	}

	private IEnumerator RevealTextRoutine()
	{
		float progress = 0f;
		int previousRevealedCount = 0;
		float step = 1f / _revealDuration;
		float timeSinceSound = 1f;

		while (progress < 1f)
		{
			progress += Time.deltaTime * step;
			timeSinceSound += Time.deltaTime;
			if (progress >= 1f)
			{
				progress = 1f;
				_continueArrow.SetActive(true);
			}
			_textLabel.text = FormatDisplayText(progress, out int revealedCount);

			if (previousRevealedCount != revealedCount)
			{
				previousRevealedCount = revealedCount;
				if (timeSinceSound > 0.1f)
				{
					_audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
					_audioSource.PlayOneShot(_charSound, 0.5f);
					timeSinceSound = 0f;
				}
			}

			yield return null;
		}
		_revealTextRoutine = null;
	}

	private IEnumerator DurationRoutine(float duration)
	{
		yield return new WaitForSeconds(duration);
		ContinueDialog();
		_durationRoutine = null;
	}

	private string FormatDisplayText(float progress, out int revealedCount)
	{
		string hiddenText = _dialogData.Text;
		revealedCount = Mathf.FloorToInt(hiddenText.Length * progress);
		string revealedText = hiddenText.Substring(0, revealedCount);
		hiddenText = hiddenText.Remove(0, revealedCount);

		return string.Format("{0}<color=#0000>{1}</color>", revealedText, hiddenText);
	}

	public struct DialogData
	{
		public string Text
		{
			get; private set;
		}

		public string Name
		{
			get; private set;
		}

		public Sprite Portrait
		{
			get; private set;
		}

		public Action Callback
		{
			get; private set;
		}

		public float Duration
		{
			get; private set;
		}

		public bool IsSkippable
		{
			get; private set;
		}

		public static DialogData Create(string text)
		{
			return new DialogData()
			{
				Text = text,
				IsSkippable = true,
			};
		}

		public DialogData SetName(string name)
		{
			Name = name;
			return this;
		}

		public DialogData SetPortrait(Sprite portrait)
		{
			Portrait = portrait;
			return this;
		}

		public DialogData SetContinueCallback(Action action)
		{
			Callback = action;
			return this;
		}

		public DialogData SetDuration(float duration)
		{
			Duration = duration;
			return this;
		}

		public DialogData SetClickability(bool isClickable)
		{
			IsSkippable = isClickable;
			return this;
		}
	}
}