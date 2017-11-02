using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyPlane : GeneralPlane {

    public AudioClip flyingSound;
    public AudioSource flyingSource;

    public AudioClip damagedSound;
    public AudioSource damagedSource;

    new void Update() {
        base.Update();
        PlaySounds();
    }

    void PlaySounds() {

        if (!flyingSource.isPlaying) {
            flyingSource.PlayOneShot(flyingSound, 0.5f);
        }

        if (!damagedSource.isPlaying && this.health < 100.0f) {
            damagedSource.PlayOneShot(damagedSound, 0.5f);
        }

    }

}
