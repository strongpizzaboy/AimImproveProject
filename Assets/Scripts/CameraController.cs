using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private float mouseSensitivity = 2f;
    [SerializeField]
    private float maxLookAngle = 90f;

    private float xRotation = 0f;
    private bool is_cursor_locked = true;

    void Start()
    {
        if (playerBody == null)
        {
            Debug.LogError("CameraController: playerBody is not assigned.");
            enabled = false;
            return;
        }

        LockCursor();
    }

    void Update()
    {
        // PauseMenu owns the cursor and Escape key while the game is paused
        if (PauseMenu.IsPaused)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        if (Input.GetMouseButtonDown(0) && !is_cursor_locked)
        {
            LockCursor();
        }

        if (is_cursor_locked)
        {
            HandleMouseLook();
        }
    }

    void LateUpdate()
    {
        if (playerBody)
        {
            transform.position = playerBody.position;
            transform.rotation = Quaternion.Euler(xRotation, playerBody.eulerAngles.y, 0f);
        }
    }

    void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        is_cursor_locked = true;
    }

    void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        is_cursor_locked = false;
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
    }

    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}