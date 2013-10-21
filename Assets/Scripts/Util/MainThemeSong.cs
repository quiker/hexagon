using UnityEngine;
using System.Collections;
using System;

public class MainThemeSong : MonoBehaviour {
	public AudioClip[] playlist = null;
	private int currentPlaylistIndex = -1;
	
	
	void Update() {
		if (SettingsContainer.GetMusicFlag()) {
			if (playlist.Length > 0) {
				if(!audio.isPlaying) {
					nextMusic();
				}
			}else{
				throw new Exception("Playlist is empty");
			}
		}
	}
	
	private void playMusicByIndex(int index) {
		audio.clip = playlist[index];
		play();
	}
	
	public void nextMusic() {
		if (currentPlaylistIndex == playlist.Length - 1) {
			currentPlaylistIndex = 0;
		}else{
			currentPlaylistIndex ++;	
		}
		
		playMusicByIndex(currentPlaylistIndex);
	}
	
	void Awake() {
		play();
	}
		
	public void pause() {
		audio.Pause();
	}
	
	
	public void play() {
		if (SettingsContainer.GetMusicFlag() && !audio.isPlaying) {
			audio.Play();
		}
	}
	
	void onGameResumed() {
		play();
	}
	
	void onGameMute(bool isMute) {
		if (isMute) {
			pause();
		}else{
			play();
		}
	}
}
