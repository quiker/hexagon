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
	private int star1 = 0;
	private int star2 = 0;
	private int star3 = 0;
	private int figureLimit = 0;
	private int tick = 0;
	private int lineLength = 0;
	private int groupSize = 0;
	private int enableHexagons = 0;
	private int[] colors;
	private int[][] actions;
	private int score = 0;
	
	public UIController ui = null;
	
	public void OnTick()
	{
		currentFigure.Tick();
	}
	
	public void OnConnectStart()
	{
		ticker.Stop();
		core.Connect(currentFigure.figure);
	}
	
	public void OnConnectFinish()
	{
		currentFigure.NewFigure();
		figureLimit--;
		ticker.Resume();
		
		// Update figure limit
		ui.updateFigureLimit(figureLimit);
		
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
		
		id             = N["id"].AsInt;
		name           = N["name"];
		star1          = N["star1"].AsInt;
		star2          = N["star2"].AsInt;
		star3          = N["star3"].AsInt;
		figureLimit    = N["figureLimit"].AsInt;
		lineLength     = N["lineLength"].AsInt;
		groupSize      = N["groupSize"].AsInt;
		enableHexagons = N["enableHexagons"].AsInt;
		tick           = N["tick"].AsInt;
		ticker.SetTick(tick / 1000f);
		
		colors = new int[N["colors"].Count];
		for(int i = 0; i < N["colors"].Count; i++) {
			colors[i] = N["colors"][i].AsInt;
		}
		
		int[][] pins = new int[N["pins"].Count][];
		for(int i = 0; i < N["pins"].Count; i++) {
			pins[i] = new int[N["pins"][i].Count];
			for(int j = 0; j < N["pins"][i].Count; j++) {
				pins[i][j] = N["pins"][i][j].AsInt;
			}
		}
		
		actions = new int[N["actions"].Count][];
		for(int i = 0; i < N["actions"].Count; i++) {
			actions[i] = new int[N["actions"][i].Count];
			for(int j = 0; j < N["actions"][i].Count; j++) {
				actions[i][j] = N["actions"][i][j].AsInt;
			}
		}
		pinFactory.SetActions(actions);
		
		Pin[] pinArr = pinFactory.GetPins(core.GetComponent<Figure>(), pins);
		
		core.SetPins(pinArr);
		
		figureFactory.SetColors(colors);
		currentFigure.Reinit();
		currentFigure.NewFigure();
	}

	protected void LevelComplete()
	{
		// calculate stars
		int stars = 0;
		if (score >= star1) stars = 1;
		if (score >= star2) stars = 2;
		if (score >= star3) stars = 3;
		Game.GetInstance().CompleteScreen(score, stars);
		
		SettingsContainer.SetLevelMaxScore(id, score);
		SettingsContainer.SetLevelStars(id, stars);
	}
	
	private void onLevelStarted(int level)
	{
		LoadLevel(level);
		
		// Show highscore
		ui.updateHighscore(SettingsContainer.GetLevelMaxScore(level));
		
		// Show figure Limit
		ui.updateFigureLimit(figureLimit);
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
