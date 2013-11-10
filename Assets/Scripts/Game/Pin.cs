using UnityEngine;
using System.Collections;

public class Pin : MonoBehaviour
{
	public const int PIN_TYPE_PILL = 1;
	public const int PIN_TYPE_MOB1 = 2;
	public const int PIN_TYPE_MOB2 = 3;
	public const int PIN_TYPE_MOB3 = 4;
	
	public int color;
	public int type;
	public GameObject pillSprite =  null;
	public GameObject splashSprite =  null;
	public GameObject[] mobSprites =  null;

	public Vector2 position;
	private float destroyTime = 0;
	
	// interpolation movement
	private bool inMoving = false;
	private float startTime;
	private Vector3 startPos;
	private Vector3 endPos;
	
	// Use this for initialization
	void Start () {
		SetType(type, false);
		SetColor(color);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad >= destroyTime && destroyTime != 0) {
			Destroy(gameObject);
		}
		if (inMoving) {
			float tLerp = Time.timeSinceLevelLoad - startTime;
			float seconds = 0.3f;
			if (tLerp < seconds) {
				transform.localPosition = Vector3.Lerp(startPos, endPos, tLerp/seconds);
			} else {
				inMoving = false;
				UpdatePosition();
			}
		}
	}
	
	public void SetColor(int color, bool updateSprite = true)
	{
		this.color = color;
		
		if (updateSprite) {
			UpdateSprite();
		}
	}
	
	public void SetType(int type, bool updateSprite = true)
	{
		this.type = type;
		if (updateSprite) {
			UpdateSprite();
		}
	}
	
	public void UpdateSprite() 
	{	
		if (type == PIN_TYPE_PILL) {
			foreach (GameObject mobSprite in mobSprites) {
				mobSprite.SetActive(false);
			}
			pillSprite.SetActive(true);
			Vector2 offset = new Vector2 ((color-1) * 0.1f, 1.0f);
			pillSprite.renderer.material.SetTextureOffset ("_MainTex", offset);
		} else {
			mobSprites[(type-2)*5+color-1].SetActive(true);
			pillSprite.SetActive(false);
		}
	}
	
	public void DestroyPin()
	{
		foreach (GameObject mobSprite in mobSprites) {
			mobSprite.SetActive(false);
		}
		pillSprite.SetActive(false);
		splashSprite.SetActive(true);
		destroyTime = Time.timeSinceLevelLoad + 0.5f;
	}
	
	public void MoveOn(Vector2 vector)
	{
		startTime = Time.timeSinceLevelLoad;
		startPos = transform.localPosition;
		position += vector;
		endPos = HexVector2.ConvertHexVector(position);
		inMoving = true;
	}
	
	public void UpdatePosition()
	{
		transform.localPosition = HexVector2.ConvertHexVector(position);
	}
}
