using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    public static InteractionManager Instance { get; set;}


    public Weapon hoveredWeapon = null;

    public Throwable hoveredThrowable = null;

    
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics. Raycast(ray, out hit)) 
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isEquipped == false) 
            {
                hoveredWeapon = objectHitByRaycast.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                WeaponManager.Instance.EquipWeapon(objectHitByRaycast.gameObject);

            } 
            else 
            {
                if(hoveredWeapon) 
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }


            if (objectHitByRaycast.GetComponent<Throwable>()) 
            {
                hoveredThrowable = objectHitByRaycast.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true; 

                if (Input.GetKeyDown(KeyCode.E)) 
                {
                    WeaponManager.Instance.EquipThrowable(hoveredThrowable);
                    Destroy(objectHitByRaycast.gameObject);
                }
                
            } 
            else 
            {
                if(hoveredThrowable) 
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }

        
    }
}
