using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Others;

public class InputShoot : MonoBehaviour
{
    public KeyCode shootKey { get; set; }
    public int maxBulletAliveNum { get; set; }
    public float shootFrequency { get; set; }
    public GameObject bulletFirePrefab { get; set; }
    public Transform shootPointTrans { get; set; }

    private int bulletNum = 0;
    private float lastShootTime = 0;
    private bool canShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }


    private void Shoot()
    {
        if (canShoot)
        {
            return;
        }
        if (Input.GetKeyDown(shootKey) && bulletNum < maxBulletAliveNum && Time.time - lastShootTime > shootFrequency)
        {
            //animator.SetTrigger("shoot");
            EventManager.OnEvent("OnShoot");
            if (bulletFirePrefab != null && shootPointTrans != null)
            {
                Instantiate(bulletFirePrefab, shootPointTrans.position, shootPointTrans.rotation);
                bulletNum++;
            }
            lastShootTime = Time.time;
        }
    }

    private void OnStatusChanged(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    private void OnBulletDistroy()
    {
        bulletNum--;
    }

    //public void Init(KeyCode shootKey, int maxBulletAliveNum, float shootFrequency, GameObject bulletFirePrefab, Transform shootPointTrans)
    public void Init<T>(T owner) where T : MonoBehaviour
    {
        //System.Reflection.FieldInfo[] publicFields = typeof(T).GetFields(System.Reflection.BindingFlags.Public);
        //System.Reflection.PropertyInfo[] publicPropertys = GetType().GetProperties(System.Reflection.BindingFlags.Public);
        System.Reflection.FieldInfo[] publicFields = typeof(T).GetFields();
        System.Reflection.PropertyInfo[] publicPropertys = GetType().GetProperties();
        foreach (System.Reflection.FieldInfo field in publicFields)
        {
            foreach (System.Reflection.PropertyInfo property in publicPropertys)
            {
                if (field.Name == property.Name)
                {
                    property.SetValue(this, field.GetValue(owner));
                }
            }
        }

        Init();
    }

    private void Init()
    {
        EventManager.BindingEvent<bool>("StatusChanged", OnStatusChanged);
        EventManager.BindingEvent("BulletDistroy", OnBulletDistroy);
    }
}
