using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioStatusChange : MonoBehaviour
{
    // 切换状态用时间
    public float changeUseTime = 2f;
    // 切换状态时闪烁频率
    public float twinklingFrequency = 0.1f;

    private float timeScaleMark = 0;
    private bool changing = false;
    private float changeStartTime = 0;

    private Sprite[] loadedSprites;
    private Sprite[] twinklingSprites = new Sprite[2];
    private int currentSpriteIdx = 0;
    private float lastTwinkling = 0;

    private SpriteRenderer sr;
    private Animator animator;
    private Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        loadedSprites = Resources.LoadAll<Sprite>("Sprites/Mario &  Luigi");
    }

    // Update is called once per frame
    void Update()
    {
        if (changing)
        {
            StatusChange();
        }
    }

    private void StatusChange()
    {
        if(Time.unscaledTime - lastTwinkling >= twinklingFrequency)
        {
            currentSpriteIdx = (currentSpriteIdx + 1) % twinklingSprites.Length;
            sr.sprite = twinklingSprites[currentSpriteIdx];
        }

        if(Time.unscaledTime - changeStartTime >= changeUseTime)
        {
            CompleteChange();
        }
    }

    public void DoChange(Mario.Status targetStatus)
    {
        animator.enabled = false;
        changeStartTime = Time.unscaledTime;
        timeScaleMark = Time.timeScale;
        Time.timeScale = 0;
        twinklingSprites[0] = sr.sprite;
        twinklingSprites[1] = ResultNewSprite(sr.sprite.name, targetStatus);
        changing = true;
    }

    /// <summary>
    /// 根据当前sprite名字获取目标状态的对应的sprite
    /// </summary>
    /// <param name="spriteName">当前sprite名字</param>
    /// <param name="targetStatus">目标状态</param>
    /// <returns>目标状态的对应的sprite</returns>
    private Sprite ResultNewSprite(string spriteName, Mario.Status targetStatus)
    {
        Sprite resultSprite = null;
        // 查找最后一个"_"下标 并获取下标开始的字符
        // NormalMario_0 -> _0
        string strIndex = spriteName.Substring(spriteName.LastIndexOf("_"));
        string resultSpriteName = string.Empty;
        // 根据目标状态拼接对应sprite名字字符
        switch (targetStatus)
        {
            case Mario.Status.NormalSmall:
                resultSpriteName = "NormalMario" + strIndex;
                break;
            case Mario.Status.NormalBig:
                resultSpriteName = "NormalMario_B" + strIndex;
                break;
            case Mario.Status.FireSmall:
                resultSpriteName = "FireMario" + strIndex;
                break;
            case Mario.Status.FireBig:
                resultSpriteName = "FireMario_B" + strIndex;
                break;
        }
        // 从loadedSprites数组中匹配相同名字的sprite
        for (int i = 0; loadedSprites != null && i < loadedSprites.Length; i++)
        {
            if (loadedSprites[i].name == resultSpriteName)
            {
                resultSprite = loadedSprites[i];
            }
        }
        // 如果没有匹配的sprite,则load,并添加到数组中
        if (resultSprite == null)
        {
            resultSprite = Resources.Load<Sprite>(string.Format("Sprites/{0}", resultSpriteName));
            Assets.Scripts.Others.Tool.Append(resultSprite, loadedSprites, true);
        }
        return resultSprite;
    }

    private void CompleteChange()
    {
        // 指定为目标值状态图片
        sr.sprite = twinklingSprites[1];
        ResetCollider2D();
        animator.enabled = true;
        changing = false;
        Time.timeScale = timeScaleMark;
    }

    private void ResetCollider2D()
    {
        Vector3 spriteSize = sr.sprite.bounds.size;
        // 匹配"_B"(大Mario标志)
        // 不是大Mario
        if(sr.sprite.name.IndexOf("_B") <=0)
        {
            spriteSize = Vector3.right * spriteSize.x * 0.75f + Vector3.up * spriteSize.y;
        }
        ((BoxCollider2D)coll).size = spriteSize;
        coll.offset = Vector2.right * coll.offset.x + Vector2.up * spriteSize.y * 0.5f;
    }
}
