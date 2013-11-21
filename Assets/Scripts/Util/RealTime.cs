using UnityEngine;
using System.Collections;

public class RealTime : MonoBehaviour
{
	static float mTime = 0f;
	static float mDelta = 0f;

	void OnEnable () { 
		mTime = Time.realtimeSinceStartup; 
	}
	
	public static float deltaTime { get { return GetDeltaTime(); } }
	

	public static float GetDeltaTime ()
	{
		float time = Time.realtimeSinceStartup;
		mDelta = time - mTime;
		mTime = time;
		return mDelta;
	}
}
