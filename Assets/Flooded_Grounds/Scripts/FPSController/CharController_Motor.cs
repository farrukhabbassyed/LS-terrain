using UnityEngine;
using UnityEngine.InputSystem;

public class CharController_Motor : MonoBehaviour
{
    public float speed = 10.0f;
    public float sensitivity = 30.0f;
    public float WaterHeight = 15.5f;

    private CharacterController character;

    public GameObject cam;

    private float moveFB;
    private float moveLR;

    private float rotX;
    private float rotY;

    public bool webGLRightClickRotation = true;

    private float gravity = -9.8f;

    void Start()
    {
        character = GetComponent<CharacterController>();

        if (Application.isEditor)
        {
            webGLRightClickRotation = false;
            sensitivity = sensitivity * 1.5f;
        }
    }

    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            gravity = 0f;
        }
        else
        {
            gravity = -9.8f;
        }
    }

    void Update()
    {
        // -------------------------
        // MOVEMENT - New Input System
        // -------------------------

        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed)
                input.x -= 1;

            if (Keyboard.current.dKey.isPressed)
                input.x += 1;

            if (Keyboard.current.sKey.isPressed)
                input.y -= 1;

            if (Keyboard.current.wKey.isPressed)
                input.y += 1;
        }

        moveLR = input.x * speed;
        moveFB = input.y * speed;


        // -------------------------
        // MOUSE - New Input System
        // -------------------------

        rotX = 0f;
        rotY = 0f;

        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            rotX = mouseDelta.x * sensitivity;
            rotY = mouseDelta.y * sensitivity;
        }


        // -------------------------
        // WATER / GRAVITY
        // -------------------------

        CheckForWaterHeight();


        Vector3 movement = new Vector3(
            moveLR,
            gravity,
            moveFB
        );


        // -------------------------
        // CAMERA ROTATION
        // -------------------------

        if (webGLRightClickRotation)
        {
            if (Mouse.current != null &&
                Mouse.current.leftButton.isPressed)
            {
                CameraRotation(cam, rotX, rotY);
            }
        }
        else
        {
            CameraRotation(cam, rotX, rotY);
        }


        // -------------------------
        // APPLY MOVEMENT
        // -------------------------

        movement = transform.rotation * movement;

        character.Move(movement * Time.deltaTime);
    }


    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(
            0,
            rotX * Time.deltaTime,
            0
        );

        cam.transform.Rotate(
            -rotY * Time.deltaTime,
            0,
            0
        );
    }
}