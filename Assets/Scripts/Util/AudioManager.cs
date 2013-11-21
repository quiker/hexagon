using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour {
	public AudioPlayer[] players;
		
	private AudioPlayer GetPlayerById(string id) {
		foreach(AudioPlayer player in players) {
			if (player.id == id) {
				return player;
			}
		}
		
		return null;
	}
	
	public void PlayFailSong() {
		PlayPlayer("FailSong", true);
	}
	
	public void PlayWinnerSong() {
		PlayPlayer("WinnerSong", true);
	}
	
	public void PlayMainThemeSong(bool reset = true) {
		PlayPlayer("MainThemeSong", reset);
	}
	
	public void PlayPlayer(string playerId, bool reset = false, bool pausePlayers = true) {
		AudioPlayer ap = GetPlayerById(playerId);
		
		if (pausePlayers) PausePlayers();
		if (reset) ap.Reset();		
		
		ap.Play();
	}
	
	void OnMusicFinished(AudioPlayer player) {
		switch(player.id) {
			case "FailSong": player.Pause(); GetPlayerById("MainThemeSong").Play(); break;
			case "WinnerSong": player.Pause(); GetPlayerById("MainThemeSong").Play(); break;
			case "MainThemeSong": player.nextClip(); break;
		}
	}
	
	// Остановить проигрывание всех включенных плееров
	public void PausePlayers() {
		foreach(AudioPlayer player in players) {
			if (player.audio.isPlaying) {
				player.Pause();
			}
		}
	}
	
	public void Mute() {
		foreach(AudioPlayer player in players) {
			player.audio.mute = true;
		}
	}
	
	public void UnMute() {
		foreach(AudioPlayer player in players) {
			player.audio.mute = false;
		}
	}
	
	
	
	public void SetVolume(float volume) {
		foreach(AudioPlayer player in players) {
			player.audio.volume = volume;
		}
	}
	
}
