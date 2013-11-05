using UnityEngine;
using System.Collections;
using System;

public class LevelController : MonoBehaviour {
	public Core core = null;
	private int score = 0;
	
	public UIController ui = null;
	
	void Start() {
		updateLevel(Game.GetInstance().GetCurrentLevel());
	}
	
	public void AddScore(int score) {
		SetScore(this.score + score);
	}
	
	private void updateLevel(int level) {
		SetScore(0);
		if (core != null) {
			/*
			 * level = LevelFactory.getLevel(level);
			 * core.setLevel(level);
			 * 
			 * */
			core.Reinit();
		}else{
			throw new Exception("Core is empty!");
		}
	}
	
	private void onLevelStarted(int level) {
		updateLevel(level);
	}	
	
	private void SetScore(int score)
	{
		this.score = score;
		if (ui != null) {
			ui.updateScore(score);
		}		
	}
}
