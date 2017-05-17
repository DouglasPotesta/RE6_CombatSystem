using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : ScriptableObject {

    public int Ammo = 10;
    public int clip = 10;
    public int clipSize = 10;
    public int projectiles = 1;
    public float variance = 0.1f;
    public float delay = 0.01f;
    public float blastsize;
    public GameObject particle;
    public Transform tipOfBarrel;
    public Animator anim;
    public AudioClip fire, click, reload, aim, alternate;
    public AudioSource audioSource;

	public void SwitchInEvent()
    {
        audioSource.PlayOneShot(click);
    }

    public void SwitchOutEvent()
    {

    }

    public void AimEvent()
    {
        audioSource.PlayOneShot(aim);
        
    }

    public void FireEvent ()
    {
        if (clip > 0)
        {
            clip--;
            audioSource.PlayOneShot(fire);
            Instantiate(particle, tipOfBarrel);
        }
        else if(Ammo > 0)
        {
            clip = 0;
        } else
        {
            audioSource.PlayOneShot(click);
        }
    }

    public void ReloadEvent()
    {
        if (Ammo > clipSize - clip)
        {
            audioSource.PlayOneShot(reload);
            clip = clipSize;
            Ammo -= (clipSize - clip);
        }
        else if (Ammo > 0)
        {
            audioSource.PlayOneShot(reload);
            clip += Ammo;
            Ammo = 0;
        }
    }

    public void Fire()
    {
        if (clip > 0)
        {
            anim.SetTrigger("Fire");
        }
        else if (Ammo > 0)
        {
            Reload();
            clip = 0;
        }
        else
        {
            anim.SetTrigger("Fire");
            audioSource.PlayOneShot(click);
        }
    }

    public void QuickShot()
    {
        if (clip > 0)
        {

        }
        else if (Ammo > 0)
        {
            Reload();
        }
        else
        {

        }
    }

	
	public void Reload ()
    {
        if(clip != clipSize && Ammo > 0)
        {
            anim.SetTrigger("Reload");
        }
	}

    public void AlternateFire()
    {
        anim.SetTrigger("Alternate");
    }
}
