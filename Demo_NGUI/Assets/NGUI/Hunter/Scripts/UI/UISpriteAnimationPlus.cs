/*------------------------------------------------------------------
Hunter Source Code
Copyright (c) 2011 International Games System CO., Inc.

Last Updated --
$Rev:: 11445            $
$Author:: efahsueh      $
$Date:: 2012-09-21 11:22:08#$
------------------------------------------------------------------
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Like UISpriteAnimation, but you can control display method
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation Plus")]
public class UISpriteAnimationPlus : MonoBehaviour
{
	public bool hideWhenEnd = false;
	public bool showFirstSpriteWhenEnd = false;
	
	[HideInInspector][SerializeField] int mFPS = 30;
	[HideInInspector][SerializeField] string mPrefix = "";
	
	[HideInInspector][SerializeField] private UITweener.Style mStyle = UITweener.Style.Once;
	[HideInInspector][SerializeField] private int mLoop = 0;
	
	UISprite mSprite;
	float mDelta = 0f;
	int mIndex = 0;
	List<string> mSpriteNames = new List<string>();
	
	private int mLoopCount = 0;
	private bool mReverse = false;
	
	/// <summary>
	/// Animation framerate.
	/// </summary>

	public int framesPerSecond { get { return mFPS; } set { mFPS = value; } }

	/// <summary>
	/// Set the name prefix used to filter sprites from the atlas.
	/// </summary>

	public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }
	
	public UITweener.Style style{
		get{ return mStyle; }
		set
		{
			if(value != mStyle)
			{
				mDelta = 0;
				mLoopCount = 0;
				mReverse = false;
				mStyle = value;
			}
		}
	}
	
	public int loop {
		get{ return mLoop; }
		set{ mLoop = Mathf.Max(0, value); }
	}

	/// <summary>
	/// Rebuild the sprite list first thing.
	/// </summary>

	void Start () { RebuildSpriteList(); }

	/// <summary>
	/// Advance the sprite animation process.
	/// </summary>
	
	void Update ()
	{
		if (mSpriteNames.Count > 1 && Application.isPlaying)
		{
			mDelta += Time.deltaTime;
			float rate = mFPS > 0f ? 1f / mFPS : 0f;
			if(rate >= mDelta)
				return;
			
			while (rate < mDelta)
			{
				mDelta = (rate > 0f) ? mDelta - rate : 0f;
				
				switch(mStyle)
				{
				case UITweener.Style.Once:
					if (++mIndex >= mSpriteNames.Count)
					{
						enabled = false;
						if(showFirstSpriteWhenEnd)
						{
							mIndex = 0;
							mSprite.spriteName = mSpriteNames[0];
							mSprite.MakePixelPerfect();
						}
						if(hideWhenEnd)
							mSprite.enabled = false;
						return;
					}
					break;
				case UITweener.Style.Loop:
					if (++mIndex >= mSpriteNames.Count)
					{
						if(mLoop != 0 && ++mLoopCount >= mLoop)
						{
							enabled = false;
							if(showFirstSpriteWhenEnd)
							{
								mIndex = 0;
								mSprite.spriteName = mSpriteNames[0];
								mSprite.MakePixelPerfect();
							}
							if(hideWhenEnd)
								mSprite.enabled = false;
							return;
						}
						mIndex = 0;
					}
					break;
				case UITweener.Style.PingPong:
					if(!mReverse)
					{
						if (++mIndex >= mSpriteNames.Count)
						{
							mReverse = true;
							mIndex -= 2;
						}
					}
					else
					{
						if (--mIndex <= -1)
						{
							mReverse = false;
							if(mLoop != 0 && ++mLoopCount >= mLoop)
							{
								enabled = false;
								mIndex = 0;
								if(hideWhenEnd)
									mSprite.enabled = false;
								return;
							}
							mIndex += 2;
						}
					}
					break;
				}
			}
			mSprite.spriteName = mSpriteNames[mIndex];
			mSprite.MakePixelPerfect();
		}
	}
	
	/// <summary>
	/// Rebuild the sprite list after changing the sprite name.
	/// </summary>

	void RebuildSpriteList ()
	{
		if (mSprite == null) mSprite = GetComponent<UISprite>();
		mSpriteNames.Clear();

		if (mSprite != null && mSprite.atlas != null)
		{
			List<UIAtlas.Sprite> sprites = mSprite.atlas.spriteList;

			for (int i = 0, imax = sprites.Count; i < imax; ++i)
			{
				UIAtlas.Sprite sprite = sprites[i];

				if (string.IsNullOrEmpty(mPrefix) || sprite.name.StartsWith(mPrefix))
				{
					mSpriteNames.Add(sprite.name);
				}
			}
			mSpriteNames.Sort();
		}
		
		ResetSpriteAnim();
	}
	
	/// <summary>
	/// Reset the sprite, begin form index 0 sprite
	/// </summary>
	
	public void ResetSpriteAnim()
	{
		mIndex = 0;
		mDelta = 0;
		mLoopCount = 0;
		mReverse = false;
		if(mSpriteNames.Count != 0 || mSpriteNames[0] != mSprite.spriteName)
		{
			mSprite.spriteName = mSpriteNames[0];
			mSprite.MakePixelPerfect();
		}
	}
	
	/// <summary>
	/// play or pause sprite animation
	/// </summary>
	public void PlaySpriteAnim(bool _play)
	{
		enabled = _play;
	}
	
	public void PlaySpriteAnim(ArrayList _param)
	{
		bool play = false;
		try
		{
			play = (bool)_param[0];
		}
		catch
		{
			Debug.LogWarning("UISpriteAnimationPlus::PlaySpriteAnim(), param parse errow", this);
		}
		finally
		{
			PlaySpriteAnim(play);
		}
	}
	
	/// <summary>
	/// Whether to show or hide the sprite
	/// </summary>
	public void ShowSprite(bool _show)
	{
		mSprite.enabled = _show;
	}
}