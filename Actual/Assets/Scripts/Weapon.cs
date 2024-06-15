using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
   public bool isShooting, readyToShoot;
   bool allowReset = true;
   public float shootingDelay = 2f;
   public int bulletsPerBurst = 3;
   public int burstBulletsLeft;
   public float spreadIntensity;
   
   public GameObject bulletPrefab;
   public Transform bulletSpawn;
   public float bulletVelocity = 30;
   public float bulletPrefabLifeTime = 3f;

   public GameObject muzzleEffect;
   private Animator animator;

   public float reloadTime;
   public int magazineSize; 
   public int bulletsLeft;
   public bool isReloading;

   public enum WeaponModel 
   {
      Pistol1911,
      M4A1,
   }

   public WeaponModel thisWeaponModel;

    public enum  ShootingMode 
   {
    Single,
    Burst,
    Auto
   }

   public ShootingMode currentShootingMode;
    void Awake() 
   {
     readyToShoot = true;
     burstBulletsLeft = bulletsPerBurst;
     animator = GetComponent<Animator>();
     bulletsLeft = magazineSize;
   }

    // Update is called once per frame
    void Update()
    {
        if(bulletsLeft == 0 && isShooting)
        {
           soundManager.Instance.emptySound1911.Play();
        }
        
        if (currentShootingMode == ShootingMode.Auto) 
     {
        isShooting = Input.GetKey(KeyCode.Mouse0);
     }
     else if (currentShootingMode == ShootingMode.Single || 
     currentShootingMode == ShootingMode.Burst) 
     {
        isShooting = Input.GetKeyDown(KeyCode.Mouse0);
     }

     if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
     {
        Reload();
     }

     if( readyToShoot && !isShooting && isReloading == false && bulletsLeft <= 0)
     {
        //Reload(); 
     }

     if (readyToShoot && isShooting && bulletsLeft > 0) 
     {
     burstBulletsLeft = bulletsPerBurst;
        FireWeapon(); 
     }

     if(AmmoManager.Instance.ammoDisplay != null)
     {
        AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft/bulletsPerBurst}/{magazineSize/bulletsPerBurst}";
     }
    }

         private void FireWeapon() 
        {
           bulletsLeft --;
           
            muzzleEffect.GetComponent<ParticleSystem>().Play();
            animator.SetTrigger("RECOIL");

            //soundManager.Instance.shootingSound1911.Play();
            soundManager.Instance.PlayShootingSound(thisWeaponModel);
 
            readyToShoot = false;
            Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
            
            
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bullet.transform.forward = shootingDirection;
            bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
            StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

            if (allowReset)
            {
                Invoke("Resetshot", shootingDelay);
                allowReset = false;
            }

            if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) 
            {
               burstBulletsLeft--;                
               Invoke("FireWeapon", shootingDelay);
            }

        }


        private void Reload() 
        {
           //soundManager.Instance.reloadSound1911.Play();
           soundManager.Instance.PlayReloadSound(thisWeaponModel);

           animator.SetTrigger("RELOAD");

           isReloading = true;
           Invoke("ReloadCompleted", reloadTime);
           
        }

        private void ReloadCompleted() 
        {
           bulletsLeft = magazineSize;
           isReloading = false;
        }
        void Resetshot() 
        {
            readyToShoot = true;
            allowReset = true;
        }

           public Vector3 CalculateDirectionAndSpread() 
         {
           Ray ray  = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
           RaycastHit hit;

           Vector3 targetPoint;
           if (Physics.Raycast(ray, out hit)) 
           {
            targetPoint = hit.point;
           }
           else 
           {
             targetPoint = ray.GetPoint(100);
           }

           Vector3 direction = targetPoint - bulletSpawn.position;

           float x =  UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
           float y =  UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

           return direction + new Vector3(x, y, 0);
         }
         
         
         
         IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(bullet);
        }
    }
