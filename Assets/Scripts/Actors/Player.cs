using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;

    private void Awake()
    {
        InitializeControls();
    }

    private void Start()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        }
        else
        {
            Debug.LogError("Main Camera not found.");
        }

        var actor = GetComponent<Actor>();
        if (actor != null)
        {
            GameManager.Get.Player = actor;
        }
        else
        {
            Debug.LogError("Actor component not found on Player.");
        }
    }

    private void OnEnable()
    {
        if (controls == null)
        {
            InitializeControls();
        }

        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.SetCallbacks(null);
            controls.Disable();
        }
    }

    private void InitializeControls()
    {
        controls = new Controls();  
        if (controls == null)
        {
            Debug.LogError("Failed to initialize controls.");
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MoveOrHit();
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        // Handle exit action if needed
    }

    private void MoveOrHit()
    {
        if (controls != null)
        {
            Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
            Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
            var actor = GetComponent<Actor>();
            if (actor != null)
            {
                Action.MoveOrHit(actor, roundedDirection);
                if (Camera.main != null)
                {
                    Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
                }
            }
            else
            {
                Debug.LogError("Actor component not found on Player.");
            }
        }
        else
        {
            Debug.LogError("Controls are not initialized.");
        }
    }
}
