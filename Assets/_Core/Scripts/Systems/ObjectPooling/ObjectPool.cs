using System;
using System.Collections.Generic;
using UnityEngine;


public sealed class ObjectPool : ObjectPool<Transform> { }

public class ObjectPool<TComponent> : MonoBehaviour
	where TComponent : Component
{
	#region Editor Variables
#pragma warning disable 0649

	[SerializeField]
	private TComponent _prefab = null;

	[SerializeField]
	private int _initialPoolCount = 10;

	[SerializeField, Tooltip("Zero or lower disables feature")]
	private float _pushInstancesOnDisabledRate = -1f;

#pragma warning restore 0649
	#endregion

	#region Variables

	private int _poolSize = 0;

	private Stack<TComponent> _poolReferences = new Stack<TComponent>();
	private List<TComponent> _usedReferences = new List<TComponent>();

	#endregion

	#region Properties

	public TComponent Prefab => _prefab;

	public int PoolSize => _poolSize;

	public int AvailableInstances => _poolReferences.Count;

	public int ClaimedInstancesCount => _usedReferences.Count;

	public IReadOnlyList<TComponent> ClaimedInstances => _usedReferences;

	#endregion

	#region Lifecycle

	public virtual void Start()
	{
		AddToPool(_initialPoolCount);

		if (_pushInstancesOnDisabledRate > 0f)
		{
			InvokeRepeating(nameof(PushDisabledInstances), _pushInstancesOnDisabledRate, _pushInstancesOnDisabledRate);
		}
	}

	#endregion

	#region Public Methods

	public void Init(TComponent prefab, int count, float pushInstancesOnDisabledRate = -1f)
	{
		_prefab = prefab;
		_initialPoolCount = count;
		_pushInstancesOnDisabledRate = pushInstancesOnDisabledRate;
	}

	public void Push(List<TComponent> objects)
	{
		for (int i = 0; i < objects.Count; i++)
		{
			Push(objects[i]);
		}
	}

	public void Push(TComponent[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			Push(objects[i]);
		}
	}

	public void Push(TComponent obj)
	{
		_usedReferences.Remove(obj);
		if (!_poolReferences.Contains(obj))
		{
			_poolReferences.Push(obj);

			obj.gameObject.SetActive(false);
			obj.transform.SetParent(transform);
		}

#if UNITY_EDITOR
		name = "vPool in Use (" + _usedReferences.Count + "/" + _poolSize + ")";
#endif
	}

	public TComponent Pop(Transform parent)
	{
		if (_poolReferences.Count == 0)
		{
#if UNITY_EDITOR
			Debug.LogWarning($"Poolsize {_poolSize} was not enough, increasing size to {_poolSize + 1}");
#endif
			AddToPool();
		}

		TComponent obj = _poolReferences.Pop();
		_usedReferences.Add(obj);

		ResetObject(obj);

		obj.transform.SetParent(parent, false);
		obj.gameObject.SetActive(true);

#if UNITY_EDITOR
		name = "^Pool in Use (" + _usedReferences.Count + "/" + _poolSize + ")";
#endif
		return obj;
	}

	public T Pop<T>(Transform parent) where T : Component
	{
		TComponent go = Pop(parent);
		T comp = go.GetComponent<T>();

		if (comp == null)
		{
			Push(go);
			return null;
		}

		return comp;
	}

	#endregion

	#region Protected Methods

	protected virtual void ResetObject(TComponent obj)
	{
		obj.transform.localPosition = _prefab.transform.localPosition;
		obj.transform.localRotation = _prefab.transform.localRotation;
		obj.transform.localScale = _prefab.transform.localScale;
	}

	#endregion

	#region Private Methods

	private void PushDisabledInstances()
	{
		for (int i = _usedReferences.Count - 1; i >= 0; i--)
		{
			TComponent reference = _usedReferences[i];
			if (!reference.gameObject.activeSelf)
			{
				Push(reference);
			}
		}
	}

	private void AddToPool(int amount = 1)
	{
		for (int i = 0; i < amount; i++)
		{
			_poolSize++;
			Push(Instantiate(_prefab, transform));
		}
	}

	#endregion
}
