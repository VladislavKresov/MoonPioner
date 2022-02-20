using ECM.Examples;
using UnityEngine;

public class JoystickCharacterController : EthanPlatformerController
{
    [SerializeField] protected Joystick joystick;

    protected override void HandleInput()
    {
        base.HandleInput();
        moveDirection = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
    }
}
