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
	int completesCount;
	int levelConnects;
	bool levelCompleted = false;
	bool isPlaying = false;
	float playingTime;
	
	int pillCount;
	int pill1Count;
	int pill2Count;
	int pill3Count;
	int pill4Count;
	int pill5Count;
	int pill6Count;
	int pill7Count;
	int pill8Count;
	int pill9Count;
	int pill10Count;
	
	int mobKills;
	int mob1Kills;
	int mob2Kills;
	int mob3Kills;
	int mob4Kills;
	int mob5Kills;
	int mobColor1Kills;
	int mobColor2Kills;
	int mobColor3Kills;
	int mobColor4Kills;
	int mobColor5Kills;
	int mobColor6Kills;
	int mobColor7Kills;
	int mobColor8Kills;
	int mobColor9Kills;
	int mobColor10Kills;
	
	// --------------------------------------------
	
	// Use this for initialization
	void Awake ()
	{
		Load();
		failsCount = PlayerPrefs.GetInt("achi_fails_count", 0);
		completesCount = PlayerPrefs.GetInt("achi_completes_count", 0);
		playingTime = PlayerPrefs.GetFloat("achi_playing_time", 0f);
		levelConnects = 0;
		
		pillCount   = PlayerPrefs.GetInt("achi_pill_count", 0);
		pill1Count  = PlayerPrefs.GetInt("achi_pill1_count", 0);
		pill2Count  = PlayerPrefs.GetInt("achi_pill2_count", 0);
		pill3Count  = PlayerPrefs.GetInt("achi_pill3_count", 0);
		pill4Count  = PlayerPrefs.GetInt("achi_pill4_count", 0);
		pill5Count  = PlayerPrefs.GetInt("achi_pill5_count", 0);
		pill6Count  = PlayerPrefs.GetInt("achi_pill6_count", 0);
		pill7Count  = PlayerPrefs.GetInt("achi_pill7_count", 0);
		pill8Count  = PlayerPrefs.GetInt("achi_pill8_count", 0);
		pill9Count  = PlayerPrefs.GetInt("achi_pill9_count", 0);
		pill10Count = PlayerPrefs.GetInt("achi_pill10_count", 0);
		mobKills    = PlayerPrefs.GetInt("achi_mob_kills", 0);
		mob1Kills   = PlayerPrefs.GetInt("achi_mob1_kills", 0);
		mob2Kills   = PlayerPrefs.GetInt("achi_mob2_kills", 0);
		mob3Kills   = PlayerPrefs.GetInt("achi_mob3_kills", 0);
		mob4Kills   = PlayerPrefs.GetInt("achi_mob4_kills", 0);
		mob5Kills   = PlayerPrefs.GetInt("achi_mob5_kills", 0);
		mobColor1Kills  = PlayerPrefs.GetInt("achi_mob_color1_kills", 0);
		mobColor2Kills  = PlayerPrefs.GetInt("achi_mob_color2_kills", 0);
		mobColor3Kills  = PlayerPrefs.GetInt("achi_mob_color3_kills", 0);
		mobColor4Kills  = PlayerPrefs.GetInt("achi_mob_color4_kills", 0);
		mobColor5Kills  = PlayerPrefs.GetInt("achi_mob_color5_kills", 0);
		mobColor6Kills  = PlayerPrefs.GetInt("achi_mob_color6_kills", 0);
		mobColor7Kills  = PlayerPrefs.GetInt("achi_mob_color7_kills", 0);
		mobColor8Kills  = PlayerPrefs.GetInt("achi_mob_color8_kills", 0);
		mobColor9Kills  = PlayerPrefs.GetInt("achi_mob_color9_kills", 0);
		mobColor10Kills = PlayerPrefs.GetInt("achi_mob_color10_kills", 0);
	}
	
	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("achi_fails_count", failsCount);
		PlayerPrefs.SetInt("achi_completes_count", completesCount);
		PlayerPrefs.SetFloat("achi_playing_time", playingTime);
		
		PlayerPrefs.SetInt("achi_pill_count", pillCount);
		PlayerPrefs.SetInt("achi_pill1_count", pill1Count);
		PlayerPrefs.SetInt("achi_pill2_count", pill2Count);
		PlayerPrefs.SetInt("achi_pill3_count", pill3Count);
		PlayerPrefs.SetInt("achi_pill4_count", pill4Count);
		PlayerPrefs.SetInt("achi_pill5_count", pill5Count);
		PlayerPrefs.SetInt("achi_pill6_count", pill6Count);
		PlayerPrefs.SetInt("achi_pill7_count", pill7Count);
		PlayerPrefs.SetInt("achi_pill8_count", pill8Count);
		PlayerPrefs.SetInt("achi_pill9_count", pill9Count);
		PlayerPrefs.SetInt("achi_pill10_count", pill10Count);
		PlayerPrefs.SetInt("achi_mob_kills", mobKills);
		PlayerPrefs.SetInt("achi_mob1_kills", mob1Kills);
		PlayerPrefs.SetInt("achi_mob2_kills", mob2Kills);
		PlayerPrefs.SetInt("achi_mob3_kills", mob3Kills);
		PlayerPrefs.SetInt("achi_mob4_kills", mob4Kills);
		PlayerPrefs.SetInt("achi_mob5_kills", mob5Kills);
		PlayerPrefs.SetInt("achi_mob_color1_kills", mobColor1Kills);
		PlayerPrefs.SetInt("achi_mob_color2_kills", mobColor2Kills);
		PlayerPrefs.SetInt("achi_mob_color3_kills", mobColor3Kills);
		PlayerPrefs.SetInt("achi_mob_color4_kills", mobColor4Kills);
		PlayerPrefs.SetInt("achi_mob_color5_kills", mobColor5Kills);
		PlayerPrefs.SetInt("achi_mob_color6_kills", mobColor6Kills);
		PlayerPrefs.SetInt("achi_mob_color7_kills", mobColor7Kills);
		PlayerPrefs.SetInt("achi_mob_color8_kills", mobColor8Kills);
		PlayerPrefs.SetInt("achi_mob_color9_kills", mobColor9Kills);
		PlayerPrefs.SetInt("achi_mob_color10_kills", mobColor10Kills);
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
		if (isPlaying) {
			playingTime += Time.deltaTime;
		}
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
		completesCount = 0;
		levelConnects = 0;
		levelCompleted = false;
		isPlaying = false;
		playingTime = 0f;
		
		pillCount   = 0;
		pill1Count  = 0;
		pill2Count  = 0;
		pill3Count  = 0;
		pill4Count  = 0;
		pill5Count  = 0;
		pill6Count  = 0;
		pill7Count  = 0;
		pill8Count  = 0;
		pill9Count  = 0;
		pill10Count = 0;
		mobKills    = 0;
		mob1Kills   = 0;
		mob2Kills   = 0;
		mob3Kills   = 0;
		mob4Kills   = 0;
		mob5Kills   = 0;
		mobColor1Kills  = 0;
		mobColor2Kills  = 0;
		mobColor3Kills  = 0;
		mobColor4Kills  = 0;
		mobColor5Kills  = 0;
		mobColor6Kills  = 0;
		mobColor7Kills  = 0;
		mobColor8Kills  = 0;
		mobColor9Kills  = 0;
		mobColor10Kills = 0;
	}
	
	
	
	
	public void EventStart(int level)
	{
		isPlaying = true;
		levelCompleted = false;
		levelConnects = 0;
	}
	
	public void EventFail()
	{
		failsCount++;
		isPlaying = false;
		CheckAll();
	}
	
	public void EventComplete()
	{
		completesCount++;
		isPlaying = false;
		levelCompleted = true;
		CheckAll();
	}
	
	public void EventDestroyPin(int pinType, int color)
	{
		if (pinType == Pin.PIN_TYPE_PILL) {
			pillCount++;
			switch (color) {
				case 1: pill1Count++; break;
				case 2: pill2Count++; break;
				case 3: pill3Count++; break;
				case 4: pill4Count++; break;
				case 5: pill5Count++; break;
				case 6: pill6Count++; break;
				case 7: pill7Count++; break;
				case 8: pill8Count++; break;
				case 9: pill9Count++; break;
				case 10: pill10Count++; break;
			}
		} else {
			mobKills++;
			switch (color) {
				case 1: mobColor1Kills++; break;
				case 2: mobColor2Kills++; break;
				case 3: mobColor3Kills++; break;
				case 4: mobColor4Kills++; break;
				case 5: mobColor5Kills++; break;
				case 6: mobColor6Kills++; break;
				case 7: mobColor7Kills++; break;
				case 8: mobColor8Kills++; break;
				case 9: mobColor9Kills++; break;
				case 10: mobColor10Kills++; break;
			}
			switch (pinType) {
				case Pin.PIN_TYPE_MOB1: mob1Kills++; break;
				case Pin.PIN_TYPE_MOB2: mob2Kills++; break;
				case Pin.PIN_TYPE_MOB3: mob3Kills++; break;
				case Pin.PIN_TYPE_MOB4: mob4Kills++; break;
				case Pin.PIN_TYPE_MOB5: mob5Kills++; break;
			}
		}
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
	
	
	
	// looser
	public int[] LooserProgressParams()
	{
		return new int[2]{Math.Min(failsCount,4),4};
	}
	public bool LooserTrigger()
	{
		if (failsCount >= 4) {
			return true;
		}
		return false;
	}
	
	// winner
	public int[] WinnerProgressParams()
	{
		return new int[2]{Math.Min(completesCount,4),4};
	}
	public bool WinnerTrigger()
	{
		if (completesCount >= 4) {
			return true;
		}
		return false;
	}
	
	// brutalus
	public bool BrutalusTrigger()
	{
		return levelCompleted && levelConnects == 1;
	}
	
	// practicant
	public int[] PracticantProgressParams()
	{
		return new int[2]{Math.Min((int)Math.Floor(playingTime/60),10),10};
	}
	public bool PracticantTrigger()
	{
		if (playingTime >= 600) {
			return true;
		}
		return false;
	}
	
	// pharmacist
	public int[] PharmacistProgressParams()
	{
		return new int[2]{Math.Min(pillCount,300),300};
	}
	public bool PharmacistTrigger()
	{
		if (pillCount >= 300) {
			return true;
		}
		return false;
	}
	
	// killer
	public int[] KillerProgressParams()
	{
		return new int[2]{Math.Min(mobKills,50),50};
	}
	public bool KillerTrigger()
	{
		if (mobKills >= 50) {
			return true;
		}
		return false;
	}
	
	// white_killer
	public int[] White_killerProgressParams()
	{
		return new int[2]{Math.Min(mobColor1Kills,10),10};
	}
	public bool White_killerTrigger()
	{
		if (mobColor1Kills >= 10) {
			return true;
		}
		return false;
	}
	
	// red_killer
	public int[] Red_killerProgressParams()
	{
		return new int[2]{Math.Min(mobColor2Kills,10),10};
	}
	public bool Red_killerTrigger()
	{
		if (mobColor2Kills >= 10) {
			return true;
		}
		return false;
	}
	
	// green_killer
	public int[] Green_killerProgressParams()
	{
		return new int[2]{Math.Min(mobColor3Kills,10),10};
	}
	public bool Green_killerTrigger()
	{
		if (mobColor3Kills >= 10) {
			return true;
		}
		return false;
	}
	
	// blue_killer
	public int[] Blue_killerProgressParams()
	{
		return new int[2]{Math.Min(mobColor4Kills,10),10};
	}
	public bool Blue_killerTrigger()
	{
		if (mobColor4Kills >= 10) {
			return true;
		}
		return false;
	}
	
	// yellow_killer
	public int[] Yellow_killerProgressParams()
	{
		return new int[2]{Math.Min(mobColor5Kills,10),10};
	}
	public bool Yellow_killerTrigger()
	{
		if (mobColor5Kills >= 10) {
			return true;
		}
		return false;
	}
	
}
