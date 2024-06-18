using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HUDManager : MonoBehaviour
{  
    public static HUDManager Instance { get; set;}

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;
    public Image TacticalUI;
    public TextMeshProUGUI tactialAmountUI;
    public Sprite emptySlot;
    public Sprite greySlot;

    public GameObject crosshair;

    
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

   private void Update() 
    {
        Weapon activeWeapon = WeaponManager.Instance.activeSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon) 
        {
         magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
         totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBurst}";

         Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
         ammoTypeUI.sprite = GetAmmoSprite(model);

         activeWeaponUI.sprite = GetWeaponSprite(model);

         if (unActiveWeapon) 
          {
            unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
          }
        }       
        else 
        {
         magazineAmmoUI.text = "";
         totalAmmoUI.text =  "";

         ammoTypeUI.sprite = emptySlot;

         activeWeaponUI.sprite = emptySlot; 
         unActiveWeaponUI.sprite = emptySlot;
        }

        if (WeaponManager.Instance.lethalsCount <= 0) 
        {
            lethalUI.sprite = greySlot;
        }

        if (WeaponManager.Instance.tacticalsCount <= 0) 
        {
            TacticalUI.sprite = greySlot;
        }
    }
    private GameObject GetUnActiveSlot() 
    {
        foreach(GameObject WeaponSlot in WeaponManager.Instance.weaponSlots) 
        {
            if(WeaponSlot != WeaponManager.Instance.activeSlot) 
            {
                return WeaponSlot;
            }
        }

        return null; 
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model) 
    {
        switch(model) 
        {
           case Weapon.WeaponModel.Pistol1911:
                 return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;
           
           case Weapon.WeaponModel.M4A1:
                 return Resources.Load<GameObject>("M4A1_Weapon").GetComponent<SpriteRenderer>().sprite;

           default:
                return null;
           
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model) 
    {
        switch(model) 
        {
           case Weapon.WeaponModel.Pistol1911:
                 return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
           
           case Weapon.WeaponModel.M4A1:
                 return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;
 
           default:
                return null;
           
        }
    }

    public void UpdateThrowables() 
    {

        lethalAmountUI.text = $"{WeaponManager.Instance.lethalsCount}";
        tactialAmountUI.text = $"{WeaponManager.Instance.tacticalsCount}";
        
        switch(WeaponManager.Instance.equippedLethalType) 
        {

            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        switch(WeaponManager.Instance.equippedTacticalType) 
        {

            case Throwable.ThrowableType.Smoke_Grenade:
                TacticalUI.sprite = Resources.Load<GameObject>("Smoke_Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
            case Throwable.ThrowableType.Stun_Grenade:
                TacticalUI.sprite = Resources.Load<GameObject>("Stun_Grenade").GetComponent<SpriteRenderer>().sprite;
                break;    
        }
    }
}
