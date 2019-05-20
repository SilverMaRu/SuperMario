using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityShoot : Activity
{
    public static int bulletNum { get; set; }
    public int maxBulletAliveNum { get; set; }
    public float shootFrequency { get; set; }
    public Transform shootPointTrans { get; set; }
    public bool openShoot { get; set; }

    private GameObject bulletFirePrefab;

    private float lastShootTime = 0;

    public ActivityShoot(Actor owner) : base(owner)
    {

    }

    public ActivityShoot(Actor owner, int maxBulletAliveNum, float shootFrequency, Transform shootPointTrans) : this(owner)
    {
        this.maxBulletAliveNum = maxBulletAliveNum;
        this.shootFrequency = shootFrequency;
        this.shootPointTrans = shootPointTrans;
    }

    public override void Update()
    {
        Shoot();
    }

    protected virtual void Shoot()
    {
        Activity activity = null;
        if (openShoot && owner.activites.TryGetValue(typeof(ActivityInput), out activity))
        {
            if (((ActivityInput)activity).shootKeyDown && bulletNum < maxBulletAliveNum && Time.time - lastShootTime > shootFrequency)
            {
                animator.SetTrigger("shoot");
                if (bulletFirePrefab != null && shootPointTrans != null)
                {
                    Object.Instantiate(bulletFirePrefab, shootPointTrans.position, shootPointTrans.rotation);
                    bulletNum++;
                }
                lastShootTime = Time.time;
            }
        }
    }
}
