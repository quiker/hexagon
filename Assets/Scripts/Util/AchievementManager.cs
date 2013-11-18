using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using SimpleJSON;

public class AchievementManager : MonoBehaviour
{
	private static AchievementManager _instance = null;
	
	protected AchievementManager() {}
	
	public static AchievementManager GetInstance()
	{
		if (_instance == null) {
			_instance = (AchievementManager)FindObjectOfType(typeof(AchievementManager));
 
			if ( FindObjectsOfType(typeof(AchievementManager)).Length > 1 )
			{
				Debug.LogError("[Singleton] Something went really wrong " +
					" - there should never be more than 1 singleton!" +
					" Reopenning the scene might fix it.");
			}
		}
		if (_instance == null) {
			_instance = new AchievementManager();
		}
		return _instance;
	}
	
	public class Achievement
	{
		public string id;
		public string name;
		public string description;
		public string progress;
		
		private AchievementManager _achievementManager;
		public AchievementManager achievementManager {
			get {
				return this._achievementManager;
			}
			set {
				_achievementManager = value;
			}
		}
		
		public bool IsCompleted()
		{
			return _achievementManager.IsCompleted(id);
		}
		
		public string GetProgressText()
		{
			if (progress.Length > 0) {
				return _achievementManager.ProcessProgressText(id, progress);
			}
			return "";
		}
	}

	private Achievement[] _achievements;
	public Achievement[] achievements {
		get {
			return this._achievements;
		}
	}
	
	private float sumDeltaTime;
	
	// tracking vars ------------------------------
	int failsCount;
	// --------------------------------------------
	
	// Use this for initialization
	void Start ()
	{
		Load();
		failsCount = PlayerPrefs.GetInt("achi_fails_count", 0);
	}
	
	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("achi_fails_count", failsCount);
	}
	
	// Update is called once per frame
	void Update()
	{
		sumDeltaTime += Time.deltaTime;
		if (sumDeltaTime >= 5) {
			CheckAll();
			sumDeltaTime = 0;
		}
	}
	
	public void Load()
	{
		TextAsset asset = Resources.Load("achievements", typeof(TextAsset)) as TextAsset;
		var N = JSONNode.Parse(asset.text);
		
		_achievements = new Achievement[N.Count];
		
		for(int i = 0; i < N.Count; i++) {
			achievements[i] = new Achievement();
			achievements[i].id          = N[i]["id"];
			achievements[i].name        = N[i]["name"];
			achievements[i].description = N[i]["description"];
			achievements[i].progress    = N[i]["progress"];
			achievements[i].achievementManager = this;
		}
	}
	
	public string ProcessProgressText(string id, string text)
	{
		if (text.Length > 0) {
			MethodInfo magicMethod = this.GetType().GetMethod(UppercaseFirst(id)+"ProgressParams");
	       	object magicValue = magicMethod.Invoke(this, new object[0]{});
			int[] progressParams = (int[])magicValue;
			object[] args = new object[progressParams.Length];
			progressParams.CopyTo(args, 0);			
			return String.Format(text, args);
		}
		return "";
	}
	
	public string UppercaseFirst(string s)
    {
		if (string.IsNullOrEmpty(s)) {
	    	return string.Empty;
		}
		return char.ToUpper(s[0]) + s.Substring(1);
    }
	
	public bool IsCompleted(string id)
	{
		return PlayerPrefs.GetInt("achi_"+id+"_completed", 0) != 0;
	}
	
	public void CheckAll()
	{
		MethodInfo magicMethod;
		object magicValue;
		bool completed;
		foreach(Achievement a in _achievements) {
			if (!a.IsCompleted()) {
				magicMethod = this.GetType().GetMethod(UppercaseFirst(a.id)+"Trigger");
        		magicValue = magicMethod.Invoke(this, new object[0]{});
				completed = (bool)magicValue;
				if (completed) {
					PlayerPrefs.SetInt("achi_"+a.id+"_completed", 1);
					// TODO: open achievement alert
				}
			}
		}
	}
	
	
	
	
	
	public void EventStart(int level)
	{
		//
	}
	
	public void EventFail()
	{
		failsCount++;
		//
		CheckAll();
	}
	
	public void EventComplete()
	{
		//
		CheckAll();
	}
	
	public void EventMobKill(int pinType, int color)
	{
		//
	}
	
	public void EventMobAction(int actionId)
	{
		//
	}
	
	public void EventConnect()
	{
		//
	}
	
	public void EventDestroy()
	{
		//
	}
	
	public void EventUsePowerUp(int powerUpId)
	{
		//
		CheckAll();
	}
	
	
	
	
	public int[] Achi1ProgressParams()
	{
		return new int[2]{Math.Min(failsCount,4),4};
	}
	
	public bool Achi1Trigger()
	{
		Debug.Log ("Achi1Trigger");
		if (failsCount >= 4) {
			return true;
		}
		return false;
	}
	
	public int[] Achi2ProgressParams()
	{
		return new int[2]{1,5};
	}
	
	public bool Achi2Trigger()
	{
		return false;
	}
	
	public int[] Achi3ProgressParams()
	{
		return new int[2]{1,6};
	}
	
	public bool Achi3Trigger()
	{
		return false;
	}
	
}
