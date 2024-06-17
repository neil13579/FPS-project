using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

     public static WeaponManager Instance { get; set;}

     public List<GameObject> weaponSlots;

     public GameObject activeSlot;

     public int grenades = 0;
     public float throwForce = 10f;
     public GameObject grenadePrefab;
     public GameObject throwableSpawn;
     public float forceMultiplier = 0;
     public float forceMultiplierLimit = 2f;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            Instance = this;
        }
    }

    private void Start() 
    {
        activeSlot = weaponSlots[0];
    }

    private void Update() 
    {
        foreach (GameObject weaponSlot in weaponSlots) 
        {
            if (weaponSlot == activeSlot) 
            {
               weaponSlot.SetActive(true);

            }
            else 
            {
                weaponSlot.SetActive(false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            switchSlot(0);
        }
         if(Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            switchSlot(1);
        }

        if(Input.GetKey(KeyCode.G)) 
        {
            forceMultiplier += Time.deltaTime;

            if(forceMultiplier > forceMultiplierLimit) 
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if(Input.GetKeyUp(KeyCode.G)) 
        {
            if(grenades > 0) 
            {
                ThrowLethal();

                
            }

            forceMultiplier = 0;
        }
    }





    public void EquipWeapon(GameObject eweapon) 
    {
       AddWeaponIntoSlot(eweapon); 
    }

    private void AddWeaponIntoSlot(GameObject eweapon) 
    {
        DropCurrentWeapon(eweapon);
        
        eweapon.transform.SetParent(activeSlot.transform, false); 

        Weapon weapon = eweapon.GetComponent<Weapon>();

        eweapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        eweapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isEquipped = true;
        weapon.animator.enabled = true; 

    }

    private void  DropCurrentWeapon(GameObject eweapon) 
    {
       if(activeSlot.transform.childCount > 0) 
       {
         var weaponToDrop = activeSlot.transform.GetChild(0).gameObject;

         weaponToDrop.GetComponent<Weapon>().isEquipped = false;
          weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

         weaponToDrop.transform.SetParent(eweapon.transform.parent);
         weaponToDrop.transform.localPosition = eweapon.transform.localPosition;
         weaponToDrop.transform.localRotation = eweapon.transform.localRotation; 
       }
    }


    private void switchSlot(int slotNum) 
    {
       if (activeSlot.transform.childCount > 0) 
       {
          Weapon currentWeapon = activeSlot.transform.GetChild(0).GetComponent<Weapon>();
          currentWeapon.isEquipped = false; 
       }

       activeSlot = weaponSlots[slotNum];

       if (activeSlot.transform.childCount > 0) 
       {
          Weapon newWeapon = activeSlot.transform.GetChild(0).GetComponent<Weapon>();
          newWeapon.isEquipped = true; 
       }
    }
    public void EquipThrowable(Throwable throwable) 
    {
       switch(throwable.throwableType) 
       {
         case Throwable.ThrowableType.Grenade:
            EquipGrenade();
            break;
       }
    }

    public void EquipGrenade() 
    {
        grenades += 1;

        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
        
    }

    private void ThrowLethal() 
    {
        GameObject lethalPrefab = grenadePrefab;

        GameObject throawble = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throawble.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throawble.GetComponent<Throwable>().hasBeenThrown = true;

        grenades -= 1;
        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);


    }
}    


