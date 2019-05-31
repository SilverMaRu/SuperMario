using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class MarioStatusChange : Action
{
    // 切换状态用时间
    public float changeUseTime = 2f;
    // 切换状态时闪烁频率
    public float twinklingFrequency = 0.1f;
    public RuntimeAnimatorController[] controllers;

    private float timeScaleMark = 0;
    private bool changing = false;
    private float changeStartTime = 0;

    private Sprite[] loadedSprites;
    private Sprite[] twinklingSprites = new Sprite[2];
    private int currentSpriteIdx = 0;
    private float lastTwinkling = 0;
    //private bool isFireMario = false;

    private SpriteRenderer sr;
    private Animator animator;
    private Collider2D coll;

    private Type mushroomType = typeof(Mushroom);
    private Type flowerType = typeof(Flower);
    private Mario.Status targetStatus = Mario.Status.NormalSmall;

    // Start is called before the first frame update
    protected override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        loadedSprites = Resources.LoadAll<Sprite>("Sprites/Mario &  Luigi");

        EventManager.BindingEvent<Type>("GetItem", OnGetItem);
        EventManager.BindingEvent("CollideWithEnemy", OnCollideWithEnemy);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (changing)
        {
            StatusChange();
        }
    }

    private void StatusChange()
    {
        if (Time.unscaledTime - lastTwinkling >= twinklingFrequency)
        {
            currentSpriteIdx = (currentSpriteIdx + 1) % twinklingSprites.Length;
            sr.sprite = twinklingSprites[currentSpriteIdx];
            lastTwinkling = Time.unscaledTime;
        }

        if (Time.unscaledTime - changeStartTime >= changeUseTime)
        {
            CompleteChange();
        }
    }

    private void OnGetItem(Type itemType)
    {
        if (itemType == mushroomType)
        {
            DoChange(Mario.Status.NormalBig);
        }
        else if (itemType == flowerType)
        {
            DoChange(Mario.Status.FireBig);
        }
    }

    private void OnCollideWithEnemy()
    {
        DoChange(Mario.Status.NormalSmall);
    }

    private void DoChange(Mario.Status targetStatus)
    {
        this.targetStatus = targetStatus;
        // 停止运行时间
        changeStartTime = Time.unscaledTime;
        timeScaleMark = Time.timeScale;
        Time.timeScale = 0;
        
        //isFireMario = targetStatus == Mario.Status.FireBig;
        animator.enabled = false;
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
            Tool.Append(resultSprite, loadedSprites, true);
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

        EventManager.OnEvent("StatusChanged", targetStatus);
        EventManager.OnEvent("CanShootChanged", targetStatus == Mario.Status.FireBig);
    }

    private void ResetCollider2D()
    {
        Vector3 spriteSize = sr.sprite.bounds.size;
        // 匹配"_B"(大Mario标志)
        // 不是大Mario
        if (sr.sprite.name.IndexOf("_B") <= 0)
        {
            spriteSize = Vector3.right * spriteSize.x * 0.75f + Vector3.up * spriteSize.y;
        }
        ((BoxCollider2D)coll).size = spriteSize;
        coll.offset = Vector2.right * coll.offset.x + Vector2.up * spriteSize.y * 0.5f;
    }
}
