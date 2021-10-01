using UnityEngine;
using GlobalDefine;

public class App : MonoBehaviour
{
	#region SINGLETON
	static App _instance = null;

	public static App Ins
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(App)) as App;
				if (_instance == null)
				{
					_instance = new GameObject("App", typeof(App)).GetComponent<App>();
				}
			}

			return _instance;
		}
	}
	#endregion

	//Scene
	private eSceneType _sceneState;

	//Init
	private void Awake()
	{
	}


	//Scene=====================================================================
	#region Scene
	public eSceneType GetSceneState()
	{
		return _sceneState;
	}
	#endregion
	//=====================================================================Scene
}
