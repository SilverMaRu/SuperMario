using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class InputShoot : Action
{
    public KeyCode shootKey;
    public int maxBulletAliveNum;
    public float shootFrequency;
    public GameObject bulletFirePrefab;
    public Transform shootPointTrans;

    private int currentBulletAliveNum = 0;
    private float lastShootTime = 0;
    private bool canShoot = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        bulletFirePrefab = Resources.Load<GameObject>("Prefabs/Fire");

        EventManager.BindingEvent<bool>("CanShootChanged", OnCanShootChanged);
        EventManager.BindingEvent("BulletDistroy", OnBulletDistroy);
    }

    // Update is called once per frame
    protected override void Update()
    {
        Shoot();
    }


    private void Shoot()
    {
        if (canShoot && Input.GetKeyDown(shootKey) && currentBulletAliveNum < maxBulletAliveNum && Time.time - lastShootTime > shootFrequency)
        {
            //animator.SetTrigger("shoot");
            Debug.Log("Do Shoot");
            if (bulletFirePrefab != null && shootPointTrans != null)
            {
                Instantiate(bulletFirePrefab, shootPointTrans.position, shootPointTrans.rotation);
                currentBulletAliveNum++;
                EventManager.OnEvent("Shoot");
            }
            lastShootTime = Time.time;
        }
    }

    private void OnCanShootChanged(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    private void OnBulletDistroy()
    {
        currentBulletAliveNum--;
    }
}
