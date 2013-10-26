using UnityEngine;
using System.Collections;
using System;

public class AudioPlayer : MonoBehaviour {
	public AudioClip[] playlist = null;
	private int currentPlaylistIndex = -1;
	
	void Update() {
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
	
		
	private void pause() {
		audio.Pause();
	}
	
	
	private void play() {
		audio.Play();
	}
	
	public void setMute(bool isMute) {
		audio.volume = (isMute ? 0 : 1);
	}
}