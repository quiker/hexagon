using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour 
{
	public int xTiles = 10;
	public int yTiles = 10;
	public int fps = 20;
	public int frames = 85;
 
	private Vector2 _size;
	private Renderer _myRenderer;
	private int _lastIndex = -1;
 
	void Start () 
	{
		_size = new Vector2 (1.0f / xTiles , 1.0f / yTiles);
		_myRenderer = renderer;
		frames = Mathf.Min(xTiles * yTiles, frames);
		if(_myRenderer == null)
			enabled = false;
	}
	// Update is called once per frame
	void Update()
	{
		// Calculate index
		int index = (int)(Time.timeSinceLevelLoad * fps) % (frames);
    	if(index != _lastIndex)
		{
			// split into horizontal and vertical index
			int uIndex = index % xTiles;
			int vIndex = index / xTiles;
 
			// build offset
			// v coordinate is the bottom of the image in opengl so we need to invert.
			Vector2 offset = new Vector2 (uIndex * _size.x, 1.0f - _size.y - vIndex * _size.y);
 
			_myRenderer.material.SetTextureOffset ("_MainTex", offset);
			_myRenderer.material.SetTextureScale ("_MainTex", _size);
 
			_lastIndex = index;
		}
	}
}