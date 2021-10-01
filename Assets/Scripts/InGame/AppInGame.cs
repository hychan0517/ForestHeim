using System;
using UnityEngine;

public class AppInGame : MonoBehaviour
{
	#region SINGLETON
	static AppInGame _instance = null;

	public static AppInGame Ins
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(AppInGame)) as AppInGame;
				if (_instance == null)
				{
					_instance = new GameObject("AppInGame", typeof(AppInGame)).GetComponent<AppInGame>();
				}
			}
			return _instance;
		}
	}

	public float GetPlayerSpeed()
	{
		return _playerController.GetPlayerSpeed();
	}
	#endregion

	private PlayerController _playerController;
	private Canvas _mainCanvas;
	private InGameUI _inGameUI;

	private void Awake()
	{
		_playerController = FindObjectOfType<PlayerController>();
	}

	public Canvas GetCanvas()
	{
		return _mainCanvas;
	}
}
