using UnityEngine;
using System.Collections;

public class PinWrapper : MonoBehaviour {
	
	public Figure figure;
	
	public void Play(string name)
	{
		animation.Play(name);
	}
	
	public void OnAnimate()
	{
		StopAll();
	}
	
	public void StopAll()
	{
		animation.Stop();
		transform.localRotation = new Quaternion(0, 0, 0, 10);
		transform.localPosition = new Vector3(0, 0, 0);
		figure.UpdatePosition();
	}
	
}
