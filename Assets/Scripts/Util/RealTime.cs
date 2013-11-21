using UnityEngine;
using System.Collections;

public class RealTime : MonoBehaviour
{
	private static RealTime _instance = null;
	
	protected RealTime () {}
	
	public static RealTime GetInstance()
	{
		if (_instance == null) {
			_instance = (RealTime)FindObjectOfType(typeof(RealTime));
 
			if ( FindObjectsOfType(typeof(RealTime)).Length > 1 )
			{
				Debug.LogError("[Singleton] Something went really wrong " +
					" - there should never be more than 1 singleton!" +
					" Reopenning the scene might fix it.");
			}
		}
		return _instance;
	}
	
	static float mTime = 0f;
	static float mDelta = 0f;

	void OnEnable () { 
		mTime = Time.realtimeSinceStartup; 
	}
	
	void Update()
	{
		float time = Time.realtimeSinceStartup;
		mDelta = time - mTime;
		mTime = time;
	}
	
	public static float deltaTime {
		get {
			return mDelta;
		}
	}
}
