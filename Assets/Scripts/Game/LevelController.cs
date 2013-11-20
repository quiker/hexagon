﻿using UnityEngine;
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
	private float[][] actions;
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
		
		actions = new float[N["actions"].Count][];
		for(int i = 0; i < N["actions"].Count; i++) {
			actions[i] = new float[N["actions"][i].Count];
			for(int j = 0; j < N["actions"][i].Count; j++) {
				actions[i][j] = N["actions"][i][j].AsFloat;
			}
		}
		pinFactory.SetActions(actions);
		
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
