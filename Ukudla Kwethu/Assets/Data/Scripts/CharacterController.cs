using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 move; 
    public float moveSpeed = 5f;
    public float rotationSpeed = 0.15f;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    void Update()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0, move.y);//take in move x and y from out inout action(s)
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    public void Sprinting()
    {

    }
}
