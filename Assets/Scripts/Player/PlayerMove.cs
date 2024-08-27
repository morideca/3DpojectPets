using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerMoveHumanoid
{
    private void Start()
    {
        base.Start();
        if (CameraManager.SceneType == SceneType.battle)
        {
            canMove = false;
        }
    }

}
