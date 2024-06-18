using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageradis = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;

    bool hasExploded = false;
    public bool hasBeenThrown = false; 

    public enum ThrowableType 
    {
        
        None,
        Grenade,
        Smoke_Grenade,
        Stun_Grenade

    } 

    public ThrowableType throwableType;

    private void Start() 
    {
        countdown = delay;
    }

    private void Update() 
    {
        if(hasBeenThrown) 
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0f && !hasExploded) 
            {
                Explode();
                hasExploded = true;  
            }
        }
    }

    private void Explode() 
    {
        GetThrowableEffect();

        Destroy(gameObject);
    }

    private void GetThrowableEffect() 
    {
        switch (throwableType) 
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;  
            case ThrowableType.Smoke_Grenade:
                SmokeGrenadeEffect();
                break;  
            case ThrowableType.Stun_Grenade:
                StunGrenadeEffect();
                break;         
        }
    }

    
    private void StunGrenadeEffect() 
    {
        GameObject stunEffect = GlobalReferences.Instance.stunGrenadeEffect;
        Instantiate(stunEffect, transform.position, transform.rotation);

        soundManager.Instance.throwablesChannel.PlayOneShot(soundManager.Instance.stunGrenadeSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageradis); 
        foreach(Collider objectInRange in colliders) 
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();

            if(rb != null) 
            {
               // apply stun

            }


        }
    }
    
    private void SmokeGrenadeEffect() 
    {
        GameObject smokeEffect = GlobalReferences.Instance.smokeGrenadeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        soundManager.Instance.throwablesChannel.PlayOneShot(soundManager.Instance.grenadeSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageradis); 
        foreach(Collider objectInRange in colliders) 
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();

            if(rb != null) 
            {
               // apply blindness

            }


        }
    }

    private void GrenadeEffect() 
    {
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        soundManager.Instance.throwablesChannel.PlayOneShot(soundManager.Instance.grenadeSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageradis); 
        foreach(Collider objectInRange in colliders) 
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();

            if(rb != null) 
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageradis);

            }


        }
    }
}
