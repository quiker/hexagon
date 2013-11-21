using UnityEngine;
using System.Collections;
using System;

public class AudioPlayer : MonoBehaviour {
	public AudioClip[] playlist = null;
	public GameObject Manager= null;
	private int currentPlaylistIndex = 0;
	private bool applicationPause = false;
	public string id;
	
	void Update() {
		if (playlist.Length > 0) {
			if(applicationPause == false && !audio.isPlaying) {
				if (audio.clip != null) {
					onClipComplete();
				}
			}
		}else{
			throw new Exception("Playlist is empty");
		}
	}
	
	public void Pause() {
		enabled = false;
		audio.Pause();
	}
	
	public void Play() {
		enabled = true;
		audio.Play();
	}
	
	public void Reset() {
		audio.clip = playlist[currentPlaylistIndex];
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
		if (gameObject.GetComponent<AudioSource>() == null) {
			gameObject.AddComponent<AudioSource>();
		}
		
		audio.playOnAwake = false;
		audio.loop = false;
		audio.clip = playlist[currentPlaylistIndex];
		Pause();
	}
}