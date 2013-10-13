using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UISprites.
/// </summary>

[CustomEditor(typeof(UIImageButton))]
public class UIImageButtonInspector : Editor
{
	UIImageButton mButton;

	/// <summary>
	/// Atlas selection callback.
	/// </summary>

	void OnSelectAtlas (MonoBehaviour obj)
	{
		if (mButton.target != null)
		{
			Undo.RegisterUndo(mButton.target, "Atlas Selection");
			mButton.target.atlas = obj as UIAtlas;
			mButton.target.MakePixelPerfect();
			EditorUtility.SetDirty(mButton.gameObject);
		}
	}

	public override void OnInspectorGUI ()
	{
		EditorGUIUtility.LookLikeControls(80f);
		mButton = target as UIImageButton;

		UISprite sprite = EditorGUILayout.ObjectField("Sprite", mButton.target, typeof(UISprite), true) as UISprite;

		if (mButton.target != sprite)
		{
			Undo.RegisterUndo(mButton, "Image Button Change");
			mButton.target = sprite;
			if (sprite != null) sprite.spriteName = mButton.normalSprite;
		}

		if (mButton.target != null)
		{
			ComponentSelector.Draw<UIAtlas>(sprite.atlas, OnSelectAtlas);

			if (sprite.atlas != null)
			{
				string normal = UISpriteInspector.SpriteField(sprite.atlas, "Normal", mButton.normalSprite);
				string hover  = UISpriteInspector.SpriteField(sprite.atlas, "Hover", mButton.hoverSprite);
				string press  = UISpriteInspector.SpriteField(sprite.atlas, "Pressed", mButton.pressedSprite);

				if (mButton.normalSprite != normal ||
					mButton.hoverSprite != hover ||
					mButton.pressedSprite != press)
				{
					Undo.RegisterSceneUndo("Image Button Change");
					mButton.normalSprite = normal;
					mButton.hoverSprite = hover;
					mButton.pressedSprite = press;
					sprite.spriteName = normal;
					sprite.MakePixelPerfect();
					NGUITools.AddWidgetCollider(mButton.gameObject);
				}
			}
		}
	}
}