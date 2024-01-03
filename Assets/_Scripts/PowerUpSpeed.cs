using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : PowerUp
{
    protected override void PickUp(SnakeController snakeController)
    {
        snakeController.SpeedPickedUp(_duration);
        base.PickUp(snakeController);
    }
}
