using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraState {

    void Update();

    void FixedUpdate();

    void LateUpdate();

    void ToAim();

    void ToBasic();

    void ToGround();

    void ToRun();

    CamStats GetStats();





}
