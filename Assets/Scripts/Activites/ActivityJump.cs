using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityJump : Activity
{
    public float jumpForce { get; set; }

    private bool onGround = false;
    private bool hadJumpKeyDown = false;
    private bool hadJumpKeyUp = false;

    public ActivityJump(Actor owner) : base(owner)
    {

    }

    public  ActivityJump(Actor owner,  float jumpForce) : this(owner)
    {
        this.jumpForce = jumpForce;
    }

    public override void Update()
    {
        Jump();
    }

    private void Jump()
    {
        Activity activity = null;
        if (owner.activites.TryGetValue(typeof(ActivityInput), out activity))
        {
            if (onGround && ((ActivityInput)activity).jumpKeyDown)
            {
                rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                hadJumpKeyDown = true;
                hadJumpKeyUp = false;
            }

            if (!onGround && hadJumpKeyDown && !hadJumpKeyUp && ((ActivityInput)activity).jumpKeyUp)
            {
                rigidbody2D.velocity = Vector2.right * rigidbody2D.velocity.x + Vector2.up * rigidbody2D.velocity.y * 0.25f;
                hadJumpKeyUp = true;
                hadJumpKeyDown = false;
            }
        }
    }
}
