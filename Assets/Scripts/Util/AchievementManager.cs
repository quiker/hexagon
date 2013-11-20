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
			if (progress != null && progress.Length > 0) {
				return _achievementManager.ProcessProgressText(id, progress);
			}
			return "";
		}
	}

	private Achievement[] _achievements;
	public Achievement[] achievements {
		get {
			Sort();
			return this._achievements;
		}
	}
	
	private float sumDeltaTime;
	
	// tracking vars ------------------------------
	int failsCount;
	int levelConnects;
	bool levelCompleted;
	// --------------------------------------------
	
	// Use this for initialization
	void Awake ()
	{
		Load();
		failsCount = PlayerPrefs.GetInt("achi_fails_count", 0);
		levelConnects = 0;
	}
	
	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("achi_fails_count", failsCount);
	}
	
	void Sort()
	{
		Achievement temp;
		for (int i = 0; i < _achievements.Length-1; i++) {
			for (int j = i+1; j < _achievements.Length; j++) {
				if (
					!_achievements[i].IsCompleted() && _achievements[j].IsCompleted() ||
					(_achievements[i].IsCompleted() == _achievements[j].IsCompleted() && _achievements[i].name.CompareTo(_achievements[j].name) > 0)
				) {
					temp = _achievements[i];
					_achievements[i] = _achievements[j];
					_achievements[j] = temp;
				}
			}
		}
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
			_achievements[i] = new Achievement();
			_achievements[i].id          = N[i]["id"];
			_achievements[i].name        = N[i]["name"];
			_achievements[i].description = N[i]["description"];
			_achievements[i].progress    = N[i]["progress"];
			_achievements[i].achievementManager = this;
		}
	}
	
	public Achievement GetAchieveById(string id) {
		foreach(Achievement achieve in _achievements) {
			if (achieve.id == id ) {
				return achieve;
			}
		}
		
		return null;
	}
	
	public string ProcessProgressText(string id, string text)
	{
		if (text != null && text.Length > 0) {
			MethodInfo magicMethod = this.GetType().GetMethod(UppercaseFirst(id)+"ProgressParams");
			if (magicMethod == null) {
				return text;
			}
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
				if (magicMethod == null) {
					continue;
				}
				magicValue = magicMethod.Invoke(this, new object[0]{});
				completed = (bool)magicValue;
				if (completed) {
					PlayerPrefs.SetInt("achi_"+a.id+"_completed", 1);
					Game.GetInstance().AlertUnlockAchievement(a);					
				}
			}
		}
	}

	public void ClearTrackingVars()
	{
		failsCount = 0;
		levelConnects = 0;
		levelCompleted = false;
	}
	
	
	
	
	public void EventStart(int level)
	{
		//
		levelCompleted = false;
		levelConnects = 0;
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
		levelCompleted = true;
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
		levelConnects++;
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
	public int[] Achi4ProgressParams()
	{
		return new int[2]{Math.Min(failsCount,4),4};
	}
	
	public bool Achi4Trigger()
	{
		Debug.Log ("Achi1Trigger");
		if (failsCount >= 4) {
			return true;
		}
		return false;
	}
	public int[] Achi7ProgressParams()
	{
		return new int[2]{Math.Min(failsCount,3),3};
	}
	
	public bool Achi7Trigger()
	{
		Debug.Log ("Achi1Trigger");
		if (failsCount >= 3) {
			return true;
		}
		return false;
	}
	
	public bool Achi2Trigger()
	{
		return levelCompleted && levelConnects == 1;
	}
	
}
