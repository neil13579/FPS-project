using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

     public static WeaponManager Instance { get; set;}

     public List<GameObject> weaponSlots;

     public GameObject activeSlot;

     public float throwForce = 10f;
    
     public GameObject throwableSpawn;
     public float forceMultiplier = 0;
     public float forceMultiplierLimit = 2f;

     public int lethalsCount = 0;
     public int maxLethals = 2;
     public Throwable.ThrowableType equippedLethalType;
     public GameObject grenadePrefab;

     public int tacticalsCount = 0;
     public int maxTacticals = 2;
     public Throwable.ThrowableType equippedTacticalType;
     public GameObject smokeGrenadePrefab;
     public GameObject stunGrenadePrefab;

    

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

        equippedLethalType = Throwable.ThrowableType.None;
        equippedTacticalType = Throwable.ThrowableType.None;
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

        if(Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T)) 
        {
            forceMultiplier += Time.deltaTime;

            if(forceMultiplier > forceMultiplierLimit) 
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if(Input.GetKeyUp(KeyCode.G)) 
        {
            if(lethalsCount > 0) 
            {
                ThrowLethal();
            }

            forceMultiplier = 0;
        }

        if(Input.GetKeyUp(KeyCode.T)) 
        {
            if(tacticalsCount > 0) 
            {
                ThrowTactical();
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
            EquipLethal(Throwable.ThrowableType.Grenade);
            break;
         case Throwable.ThrowableType.Smoke_Grenade:
            EquipTactical(Throwable.ThrowableType.Smoke_Grenade);
            break;   
         case Throwable.ThrowableType.Stun_Grenade:
            EquipTactical(Throwable.ThrowableType.Stun_Grenade);
            break;    
       }
    }

    private void EquipTactical(Throwable.ThrowableType tactical) 
    {
       if(equippedTacticalType == tactical || equippedTacticalType == Throwable.ThrowableType.None) 
        {
            equippedTacticalType = tactical;

            if(tacticalsCount < maxTacticals) 
            {
                tacticalsCount += 1; 
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowables();
            }
            else 
            {
                print("tactical limit reached");
            }
        }
        else 
        {
            //Cannot pickup different tactical
            // option to switch tacticals
        }
    }

    private void EquipLethal(Throwable.ThrowableType lethal) 
    {
        if(equippedLethalType == lethal || equippedLethalType == Throwable.ThrowableType.None) 
        {
            equippedLethalType = lethal;

            if(lethalsCount < maxLethals) 
            {
                lethalsCount += 1; 
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateThrowables();
            }
            else 
            {
                print("Lethals limit reached");
            }
        }
        else 
        {
            //Cannot pickup different lethal
            // option to switch lethals
        }
    }

    
    
    
   

    private void ThrowLethal() 
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType);

        GameObject throawble = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throawble.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throawble.GetComponent<Throwable>().hasBeenThrown = true;

        lethalsCount -= 1;
        
        if (lethalsCount <= 0) 
        {
            equippedLethalType = Throwable.ThrowableType.None;
        }


        HUDManager.Instance.UpdateThrowables();
    }

    private void ThrowTactical() 
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType);

        GameObject throawble = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throawble.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throawble.GetComponent<Throwable>().hasBeenThrown = true;

        tacticalsCount   -= 1;
        
        if (tacticalsCount <= 0) 
        {
            equippedTacticalType = Throwable.ThrowableType.None;
        }


        HUDManager.Instance.UpdateThrowables();
    }

    private GameObject GetThrowablePrefab(Throwable.ThrowableType throwableType) 
    {
        switch(throwableType) 
        {
           case Throwable.ThrowableType.Grenade: 
                return grenadePrefab;
           case Throwable.ThrowableType.Smoke_Grenade: 
                return smokeGrenadePrefab; 
           case Throwable.ThrowableType.Stun_Grenade: 
                return stunGrenadePrefab;       
        }
        return new();
    }
}    


