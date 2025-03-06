using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [System.Serializable]
    public class Vector2Event : UnityEvent<Vector2> { }

    // These events allow other scripts to subscribe to movement, jump, and reset actions.
    public Vector2Event OnMove;
    public UnityEvent OnJump;
    public UnityEvent OnReset;

    private void Update()
    {
        // Handle movement input using the horizontal and vertical axes.
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (moveInput != Vector2.zero)
        {
            OnMove?.Invoke(moveInput);
        }

        // When space is pressed, invoke jump.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump?.Invoke();
        }

        // For resetting the player, here we use the R key.
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnReset?.Invoke();
        }
    }
}
