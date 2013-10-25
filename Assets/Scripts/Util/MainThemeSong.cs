﻿using UnityEngine;
using System.Collections;
using System;

public class MainThemeSong : MonoBehaviour {
	public AudioClip[] playlist = null;
	private int currentPlaylistIndex = -1;
	
	void Update() {
		if (SettingsContainer.GetMusicFlag()) {
			if (playlist.Length > 0) {
				if(!audio.isPlaying) {
					if (!audio.clip != null) {
						onClipComplete();
					}else{
						playClipByIndex(0);
					}
				}
			}else{
				throw new Exception("Playlist is empty");
			}
		}
	}
	
	private void playClipByIndex(int index) {
		currentPlaylistIndex = index;
		audio.clip = playlist[index];		
		play();
	}
	
	private void onClipComplete() {
		nextClip();
	}
	
	public void nextClip() {
		if (currentPlaylistIndex == playlist.Length - 1) {
			currentPlaylistIndex = 0;
		}else{
			currentPlaylistIndex ++;	
		}
		
		playClipByIndex(currentPlaylistIndex);
	}
	
		
	public void pause() {
		audio.Pause();
	}
	
	
	public void play() {
		audio.Play();
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
