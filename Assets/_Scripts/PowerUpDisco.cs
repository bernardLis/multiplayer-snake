using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using System.Threading;

public class PowerUpDisco : PowerUp
{
    protected override void PickUp(SnakeController snakeController)
    {
        snakeController.Disco(_duration);
        base.PickUp(snakeController);
    }

}
