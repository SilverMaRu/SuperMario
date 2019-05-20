using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityInput : Activity
{
    public float h { get; private set; }
    public float v { get; private set; }
    public bool jumpKeyDown { get; private set; }
    public bool jumpKeyUp { get; private set; }
    public bool shootKeyDown { get; private set; }

    private KeyCode jumpKey;
    private KeyCode shootKey;

    public ActivityInput(Actor owner, KeyCode jumpKey, KeyCode shootKey) : base(owner)
    {
        this.jumpKey = jumpKey;
        this.shootKey = shootKey;
    }

    public override void Update()
    {
        h = Input.GetAxis("Horizontal");
        jumpKeyDown = Input.GetKeyDown(jumpKey);
        shootKeyDown = Input.GetKeyDown(shootKey);
        jumpKeyUp = Input.GetKeyUp(jumpKey);
    }
}
