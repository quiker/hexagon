using UnityEngine;
using System.Collections;
using System;

public class AudioPlayer : MonoBehaviour {
	public AudioClip[] playlist = null;
	public GameObject Manager= null;
	public float volumePerFrame = 1;
	private int currentPlaylistIndex = 0;
	private bool applicationPause = false;
	public string id;
	
	void Update() {
		if (playlist.Length > 0) {
			if(applicationPause == false && !audio.isPlaying) {
				if (audio.clip != null) {
					onClipComplete();
				}
			}else{
				if (audio.volume <= SettingsContainer.GetMusicValue()) {
					audio.volume += volumePerFrame * RealTime.deltaTime;
				}
			}
		}else{
			throw new Exception("Playlist is empty");
		}
	}
	
	public void Pause() {
		enabled = false;
		audio.volume = 0;
		audio.Pause();
	}
	
	public void Play() {
		enabled = true;
		audio.Play();
	}
	
	public void Reset() {
		audio.playOnAwake = false;
		audio.loop = false;
		
		if (playlist.Length > 0) {
			audio.clip = playlist[currentPlaylistIndex];
		}
		
		audio.Stop();
	}
	
	
	void OnApplicationPause(bool pauseStatus) {
		applicationPause = pauseStatus;
	}
	
	private void playClipByIndex(int index) {
		currentPlaylistIndex = index;
		audio.clip = playlist[index];
		Play();
	}
	
	private void onClipComplete() {
		if (Manager != null) {
			Manager.SendMessage("OnMusicFinished", this, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void nextClip() {
		if (currentPlaylistIndex == playlist.Length - 1) {
			currentPlaylistIndex = 0;
			
			if (Manager != null) {
				Manager.SendMessage("OnPlaylistFinished", this, SendMessageOptions.DontRequireReceiver);
			}
			
		}else{
			currentPlaylistIndex ++;	
		}
		
		playClipByIndex(currentPlaylistIndex);
	}
	
	public void resetPlaylist() {
		currentPlaylistIndex = -1;
	}
	
	void Start() {
		Reset();
		Pause();
	}
}