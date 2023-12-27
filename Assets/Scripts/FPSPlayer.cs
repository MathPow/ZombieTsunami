using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayer : MonoBehaviour
{
    [SerializeField] private GameObject soundManager;
    private SoundManager soundManagerScript;
    private GameObject gameManager;
    private GameManager gameManagerScript;
    [SerializeField] public float walkingSpeed = 5.5f;
    [SerializeField] public float runningSpeed = 8.5f;
    [SerializeField] public float jumpSpeed = 6.0f;
    [SerializeField] public float gravity = 20.0f;
    [SerializeField] public Camera playerCamera;
    [SerializeField] public Camera topDownCamera;
    [SerializeField] public float lookSpeed = 2.0f;
    [SerializeField] public float lookXLimit = 45.0f;
    [SerializeField] private float SPEED_PLAYER_PERK_MULTIPLICATOR = 1.25f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    private bool canMove = true;
    private bool wasMoving = false;

    private const int MAX_HEIGHT_CAMERA = 65;
    private const int MIN_HEIGHT_CAMERA = 5;
    private const int INCREMENT_HEIGHT_CAMERA = 10;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (soundManager != null)
            soundManagerScript = soundManager.GetComponent<SoundManager>();
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera.depth = -1;
        topDownCamera.rect = new Rect(0f, 0.7f, 0.2f, 0.3f);
        topDownCamera.depth = 0;
        playerCamera.rect = new Rect(0f, 0f, 1f, 1f);
    }

    void Update()
    {
        if (!gameManagerScript.IsPauseGame() && !gameManagerScript.IsGameOver())
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (topDownCamera.depth == -1)
                {
                    playerCamera.depth = -1;
                    topDownCamera.depth = 0;
                    playerCamera.rect = new Rect(0f, 0f, 1f, 1f);
                    topDownCamera.rect = new Rect(0f, 0.7f, 0.2f, 0.3f);
                }
                else
                {
                    topDownCamera.depth = -1;
                    playerCamera.depth = 0;
                    topDownCamera.rect = new Rect(0f, 0f, 1f, 1f);
                    playerCamera.rect = new Rect(0f, 0.7f, 0.2f, 0.3f);
                }
                soundManagerScript.PlayPing();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                float newY = topDownCamera.transform.position.y - INCREMENT_HEIGHT_CAMERA;
                if (newY > MIN_HEIGHT_CAMERA)
                    topDownCamera.transform.position = new Vector3(topDownCamera.transform.position.x, newY, topDownCamera.transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                float newY = topDownCamera.transform.position.y + INCREMENT_HEIGHT_CAMERA;
                if (newY < MAX_HEIGHT_CAMERA)
                    topDownCamera.transform.position = new Vector3(topDownCamera.transform.position.x, newY, topDownCamera.transform.position.z);
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (!wasMoving && moveDirection.magnitude > 0.1f)
            {
                soundManagerScript.StartFootstep();
                wasMoving = true;
            }
            else if (wasMoving && moveDirection.magnitude <= 0.1f)
            {
                soundManagerScript.StopFootstep();
                wasMoving = false;
            }

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

                float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * lookSpeed;
                transform.localEulerAngles = new Vector3(0, rotationY, 0);

                Vector3 cameraPos = topDownCamera.transform.position;
                Vector3 newCameraPosition = new Vector3(transform.position.x, cameraPos.y, transform.position.z);
                topDownCamera.transform.position = newCameraPosition;
            }
        }
    }

    public void SpeedBoostPerkActivate()
    {
        walkingSpeed = walkingSpeed * SPEED_PLAYER_PERK_MULTIPLICATOR;
        runningSpeed = runningSpeed * SPEED_PLAYER_PERK_MULTIPLICATOR;
    }
}