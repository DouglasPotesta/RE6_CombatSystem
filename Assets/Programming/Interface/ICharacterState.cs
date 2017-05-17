using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState {

    void Update();

    void OnTriggerEnter(Collider other);

    void OnTriggerStay(Collider other);

    void OnTriggerExit(Collider other);

    void SwitchWeapon(WeaponBehaviour weapon);

    void OnAnimatorMove();

    // Use this for initialization
    void ToCasual();

    // Update is called once per frame
    void ToCombat();

    void ToHurt();

    void ToInteraction();

    void ToQuickTime();

    void ToGround();


}
