using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;

public class LevelController : MonoBehaviour, Ticker.TickListener
{
	public Core core = null;
	public CurrentFigure currentFigure = null;
	public PinFactory pinFactory = null;
	public FigureFactory figureFactory = null;
	public Ticker ticker = null;
	
	private int id = 0;
	private string name = "test";
	private int star2 = 0;
	private int star3 = 0;
	private int figureLimit = 0;
	private int tick = 0;
	private int[] colors;
	private float[][] actionsArr;
	private Pin.MobAction[] _actions;
	public Pin.MobAction[] actions {
		get {
			return this._actions;
		}
	}

	private int score = 0;
	private bool ticking = true;
	
	public UIController ui = null;
	
	public void OnTick()
	{
		currentFigure.Tick();
	}
	
	public void AutoConnect()
	{
		if (ticking) {
			while(currentFigure.Tick()){}
		}
	}
	
	public void OnConnectStart()
	{
		ticking = false;
		ticker.Stop();
		core.Connect(currentFigure.figure);
	}
	
	public void OnConnectFinish()
	{
		ticking = true;
		ticker.Resume();
		currentFigure.NewFigure();
		figureLimit--;
		
		// Update figure limit
		ui.updateFigureLimit(figureLimit);
		
		AchievementManager.GetInstance().EventConnect();
		
		int mobCount = 0;
		foreach(Pin pin in core.figure.pins) {
			if (pin.type != Pin.PIN_TYPE_PILL) {
				mobCount++;
			}
		}
		if (mobCount == 0) {
			LevelComplete();
		}
	}
	
	private void LoadLevel(int level)
	{
		if (core == null) throw new Exception("Core is empty!");
		
		SetScore(0);
		
		TextAsset asset = Resources.Load("Levels/level_"+level, typeof(TextAsset)) as TextAsset;
		var N = JSONNode.Parse(asset.text);
		
		id                  = N["id"].AsInt;
		name                = N["name"];
		star2               = N["star2"].AsInt;
		star3               = N["star3"].AsInt;
		figureLimit         = N["figureLimit"].AsInt;
		core.enableRings    = N["enableRings"].AsInt;
		core.enableLines    = N["enableLines"].AsInt;
		core.enableGroups   = N["enableGroups"].AsInt;
		core.enableHexagons = N["enableHexagons"].AsInt;
		core.lineLength     = N["lineLength"].AsInt;
		core.groupSize      = N["groupSize"].AsInt;
		tick                = N["tick"].AsInt;
		ticker.SetTick(tick / 1000f);
		
		colors = new int[N["colors"].Count];
		for(int i = 0; i < N["colors"].Count; i++) {
			colors[i] = N["colors"][i].AsInt;
		}
		
		int[] tutorialSlides = new int[N["tutorialSlides"].Count];
		for(int i = 0; i < N["tutorialSlides"].Count; i++) {
			tutorialSlides[i] = N["tutorialSlides"][i].AsInt;
		}
		
		int[][] pins = new int[N["pins"].Count][];
		for(int i = 0; i < N["pins"].Count; i++) {
			pins[i] = new int[N["pins"][i].Count];
			for(int j = 0; j < N["pins"][i].Count; j++) {
				pins[i][j] = N["pins"][i][j].AsInt;
			}
		}
		
		actionsArr = new float[N["actions"].Count][];
		for(int i = 0; i < N["actions"].Count; i++) {
			actionsArr[i] = new float[N["actions"][i].Count];
			for(int j = 0; j < N["actions"][i].Count; j++) {
				actionsArr[i][j] = N["actions"][i][j].AsFloat;
			}
		}
		this._actions = new Pin.MobAction[actionsArr.Length];
		for (int i = 0; i < actionsArr.Length; i++) {
			this._actions[i] = new Pin.MobAction();
			this._actions[i].inactiveInterval = actionsArr[i][0];
			this._actions[i].activeInterval = actionsArr[i][1];
			this._actions[i].chance = actionsArr[i][2];
			this._actions[i].id = (int)actionsArr[i][3];
			this._actions[i].parameters = ArrayUtils.SliceF(actionsArr[i], 4);
		}
		
		Pin[] pinArr = pinFactory.GetPins(core.GetComponent<Figure>(), pins);
		
		core.SetPins(pinArr);
		
		figureFactory.SetColors(colors);
		if (tutorialSlides.Length > 0 && SettingsContainer.SetAvailableSlides(tutorialSlides)) {
			Game.GetInstance().MenuTutorial(tutorialSlides);
		} else {
			Game.GetInstance().MenuResume();
		}
		currentFigure.Reinit();
		currentFigure.NewFigure();
	}
	
	public void LevelFail()
	{
		AchievementManager.GetInstance().EventFail();
		Game.GetInstance().FailScreen();
	}

	protected void LevelComplete()
	{
		// calculate stars
		score = Math.Max(figureLimit * 10, 0);
		int stars = 1;
		if (score >= star2) stars = 2;
		if (score >= star3) stars = 3;
		
		SettingsContainer.SetLevelMaxScore(id, score);
		SettingsContainer.SetLevelStars(id, stars);
		
		AchievementManager.GetInstance().EventComplete();
		
		Game.GetInstance().CompleteScreen(score, stars);
	}
	
	private void onLevelStarted(int level)
	{
		LoadLevel(level);
		
		// Show highscore
		ui.updateHighscore(SettingsContainer.GetLevelMaxScore(level));
		
		// Show figure Limit
		ui.updateFigureLimit(figureLimit);
		
		AchievementManager.GetInstance().EventStart(level);
	}
	
	public void AddScore(int type, int mobs)
	{
		// calculate score
		int score = 0;
		
		AddScore(score);
	}
	
	public void AddScore(int score)
	{
		SetScore(this.score + score);
	}
	
	private void SetScore(int score)
	{
		this.score = score;
		if (ui != null) {
			ui.updateScore(score);
		}		
	}
}
