using UnityEngine;
using System.Collections;

public class PinWrapper : MonoBehaviour {
	
	public Figure figure;
	
	public void Play(string name)
	{
		animation.Play(name);
		if (name == "RotateCW" || name == "RotateCCW") {
			foreach(Pin pin in gameObject.GetComponentsInChildren<Pin>())
			{
				pin.gameObject.animation.Play(name == "RotateCW"?"RotateCCW":"RotateCW");
			}
		}
	}
	
	public void StopAll()
	{
		animation.Stop();
		transform.localRotation = new Quaternion(0, 0, 0, 10);
		transform.localPosition = new Vector3(0, 0, 0);
		
		foreach(Pin pin in gameObject.GetComponentsInChildren<Pin>())
		{
			pin.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 10);
			pin.gameObject.animation.Stop();
		}
		
		figure.UpdatePosition();
	}
	
}
