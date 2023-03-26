using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController Local { get; set; }
    NetworkRigidbody2D PlayerRigidbody;
    [SerializeField] private float Speed = 20f;
    public Vector2 PlayerInput = new Vector2(0, 0);
    PlayerControls InputActions;

    public override void Spawned()
    {
        Local = this;
        InputActions = new PlayerControls();
        InputActions.Player.Enable();
        PlayerRigidbody = GetComponent<NetworkRigidbody2D>();
    }

    public override void FixedUpdateNetwork()
    {
        SetPlayerInput();
        if (!GetInput(out NetworkInputData data)) return;
        data.direction.Normalize();
        PlayerRigidbody.Rigidbody.velocity = data.direction * Speed;
    }

    void SetPlayerInput()
    {
        var left = InputActions.Player.Left.ReadValue<float>();
        var right = InputActions.Player.Right.ReadValue<float>();
        PlayerInput.x = left > 0 ? -left : right > 0 ? right : 0;
    }
}
