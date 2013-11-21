using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MusicManager : MonoBehaviour {
	public AudioPlayer[] players; 
	private AudioPlayer lastPlayer;
		
	public void ResetMainThemeSong() {
		
	}
	
	public AudioPlayer GetCurrentPlayer() {
		foreach(AudioPlayer player in players) {
			if (player.audio.isPlaying) {
				return player;
			}
		}
		
		return null;
	}
	
	private AudioPlayer GetPlayerById(string id) {
		foreach(AudioPlayer player in players) {
			if (player.id == id) {
				return player;
			}
		}
		
		return null;
	}
	
	public void FailSong() {
		PlayPlayer("FailSong");
	}
	
	public void WinnerSong() {
		PlayPlayer("WinnerSong");
	}
	
	public void MainThemeSong() {
		PlayPlayer("MainThemeSong");
	}
	
	private void PlayPlayer(string playerId) {
		lastPlayer = GetCurrentPlayer();
		PausePlayers();
		GetPlayerById(playerId).Play();
	}
	
	private void PlayLastPlayer() {
		if (lastPlayer != null) {
			PlayPlayer(lastPlayer.id);
		}
	}
	
	void OnMusicFinished(AudioPlayer player) {
		switch(player.id) {
			case "FailSong": PlayLastPlayer(); break;
			case "WinnerSong": PlayLastPlayer(); break;
			case "MainThemeSong": player.nextClip(); break;
		}
	}
	
	public void PausePlayers() {
		foreach(AudioPlayer player in players) {
			player.Pause();
		}
	}
	
}
