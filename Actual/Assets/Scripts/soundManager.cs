using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class soundManager : MonoBehaviour
{
    public static soundManager Instance { get; set;}

    public AudioSource shootingChannel;
   
    public AudioClip M4Shot;
    public AudioClip P1911Shot;
    
    public AudioSource emptySound1911;
     public AudioSource reloadSound1911;
    public AudioSource reloadSoundM4A1;

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

     public void PlayShootingSound(WeaponModel weapon)
{
   switch(weapon)
   {
      case WeaponModel.Pistol1911:
           shootingChannel.PlayOneShot(P1911Shot);
           break;
      case WeaponModel.M4A1:
           shootingChannel.PlayOneShot(M4Shot);
            break; 
   }
}

public void PlayReloadSound(WeaponModel weapon)
{
  switch(weapon)
   {
      case WeaponModel.Pistol1911:
           reloadSound1911.Play();
           break;
      case WeaponModel.M4A1:
           reloadSoundM4A1.Play();     
           break;
   }
}

}


