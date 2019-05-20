using UnityEngine;

public abstract class Activity
{
    protected Actor owner;
    protected Transform transform;
    protected Rigidbody2D rigidbody2D;
    protected Collider2D collider2D;
    protected Animator animator;

    public Activity(Actor owner)
    {
        this.owner = owner;
        transform = this.owner.transform;
        rigidbody2D = this.owner.GetComponent<Rigidbody2D>();
        collider2D = this.owner.GetComponent<Collider2D>();
        animator = this.owner.GetComponent<Animator>();
    }

    public abstract void Update();
}
