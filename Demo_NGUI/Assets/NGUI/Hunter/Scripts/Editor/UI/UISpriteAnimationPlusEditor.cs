/*------------------------------------------------------------------
Hunter Source Code
Copyright (c) 2011 International Games System CO., Inc.

Last Updated --
$Rev:: 9503             $
$Author:: efahsueh      $
$Date:: 2012-08-05 14:17:41#$
------------------------------------------------------------------
*/
using UnityEngine;
using UnityEditor;

/// <summary>
/// Inspector class used to edit UISpriteAnimations.
/// </summary>

[CustomEditor(typeof(UISpriteAnimationPlus))]
public class UISpriteAnimationPlusEditor : Editor
{
	private bool m_foldOut = false;
	
	/// <summary>
	/// Draw the inspector widget.
	/// </summary>

	public override void OnInspectorGUI ()
	{
		NGUIEditorTools.DrawSeparator();
		EditorGUIUtility.LookLikeControls(200f);
		UISpriteAnimationPlus anim = target as UISpriteAnimationPlus;

		int fps = EditorGUILayout.IntField("Framerate", anim.framesPerSecond);
		fps = Mathf.Clamp(fps, 1, 60);

		if (anim.framesPerSecond != fps)
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.framesPerSecond = fps;
			EditorUtility.SetDirty(anim);
		}

		string namePrefix = EditorGUILayout.TextField("Name Prefix", (anim.namePrefix != null) ? anim.namePrefix : "");

		if (anim.namePrefix != namePrefix)
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.namePrefix = namePrefix;
			EditorUtility.SetDirty(anim);
		}
		
		UITweener.Style style = (UITweener.Style)EditorGUILayout.EnumPopup("Style", anim.style);
		if(anim.style != style)
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.style = style;
			EditorUtility.SetDirty(anim);
		}
		
		switch(style)
		{
		case UITweener.Style.Once:
			break;
		case UITweener.Style.Loop:
		case UITweener.Style.PingPong:
			int loop = EditorGUILayout.IntField("Loop", anim.loop);
			loop = Mathf.Max(0, loop);
			if(anim.loop != loop)
			{
				NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
				anim.loop = loop;
				EditorUtility.SetDirty(anim);
			}
			break;
		}
		
		bool showFirstSprite = EditorGUILayout.Toggle("Show First Sprite When End", anim.showFirstSpriteWhenEnd);
		if(anim.showFirstSpriteWhenEnd != showFirstSprite)
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.showFirstSpriteWhenEnd = showFirstSprite;
			EditorUtility.SetDirty(anim);
		}
		
		bool hideEnd = EditorGUILayout.Toggle("Hide When End", anim.hideWhenEnd);
		if(anim.hideWhenEnd != hideEnd)
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.hideWhenEnd = hideEnd;
			EditorUtility.SetDirty(anim);
		}
		
		// display Behavior's methods
		EditorGUILayout.Space();
		EditorGUIUtility.LookLikeInspector();
		GUI.color = Color.cyan;
		m_foldOut = EditorGUILayout.Foldout(m_foldOut,"Public Method( Can use in TimeLine):");	
		GUI.color = Color.white;
		if(m_foldOut)
		{
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(30);
				GUI.color = Color.yellow;
				EditorGUILayout.LabelField("Reset the animation");
				GUI.color = Color.white;
				EditorGUILayout.TextField("ResetSpriteAnim");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(30);
				GUI.color = Color.yellow;
				EditorGUILayout.LabelField("Play or pause animation(T/F)");
				GUI.color = Color.white;
				EditorGUILayout.TextField("PlaySpriteAnim");
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}