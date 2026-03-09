using Code.Enemy;
using System.Security.Cryptography;
using UnityEngine;

public class FlyMovement : EnemyMovement
{
    protected override void MoveNormal()
    {
        float xSpeed = Vector2.left.x * enemy.data.moveSpeed;

        float ySpeed = Mathf.Sin(Time.time * 10f) * 2f;

        rigid.linearVelocity = new Vector2(xSpeed, ySpeed);
    }
}
