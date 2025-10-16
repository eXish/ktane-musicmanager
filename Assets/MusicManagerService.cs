using System;
using System.Collections;
using MusicManagerAssembly;
using UnityEngine;

[RequireComponent(typeof(KMService))]
[RequireComponent(typeof(KMGameInfo))]
[RequireComponent(typeof(KMModSettings))]
public class MusicManagerService : MonoBehaviour
{
	private KMGameInfo _gameInfo;
	private KMModSettings _modSettings;
	
	private void Awake()
	{
		_gameInfo = GetComponent<KMGameInfo>();
		_modSettings = GetComponent<KMModSettings>();
	}

	private void OnEnable()
	{
		KMGameInfo gameInfo = _gameInfo;
		gameInfo.OnStateChange = (KMGameInfo.KMStateChangeDelegate)Delegate.Combine(gameInfo.OnStateChange, new KMGameInfo.KMStateChangeDelegate(OnStateChange));
		StartCoroutine(Setup());
	}

	private void OnDisable()
	{
		ModMusicManager.Reset();
		KMGameInfo gameInfo = _gameInfo;
		gameInfo.OnStateChange = (KMGameInfo.KMStateChangeDelegate)Delegate.Remove(gameInfo.OnStateChange, new KMGameInfo.KMStateChangeDelegate(OnStateChange));
	}

	private void OnStateChange(KMGameInfo.State state)
	{
		if (state == KMGameInfo.State.Gameplay)
		{
			ModMusicManager.OnEnterGameplay();
		}
	}

	private IEnumerator Setup()
	{
		yield return null;
		ModMusicManager.Setup();
		ModSettingsLoader.AddFromModSettings(_modSettings.Settings);
		yield break;
	}
}