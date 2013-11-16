using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))] 
internal class Ortho2dCamera : MonoBehaviour
{
	[SerializeField] private bool uniform = true;
	[SerializeField] private bool autoSetUniform = false;
	private bool devMode = false;

	private void Awake()
	{
		camera.orthographic = true;

		if (uniform) {
			SetUniform();
		}
  	}
	
	private void LateUpdate()
	{
		if (autoSetUniform && uniform) {
			SetUniform();
		}
	}
	
	private void SetUniform()
	{
		float orthographicSize;

		if ( camera.pixelWidth * 3 / 2 / camera.pixelHeight >= 1) {
			orthographicSize = 480;
		} else {
			orthographicSize = 480 / (camera.pixelWidth * 3 / 2 / camera.pixelHeight);
		}

		if (devMode) {
			orthographicSize = 480;
		}

		if (orthographicSize != camera.orthographicSize) {
			camera.orthographicSize = orthographicSize;
		}
	}
}
