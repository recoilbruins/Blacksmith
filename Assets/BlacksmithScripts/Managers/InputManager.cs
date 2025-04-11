using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlackSmithInput
{
    [DefaultExecutionOrder(-1000)]
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        [SerializeField] private HandActionController handInputActions;

        [Header("Movement")]
        public Vector2 movementInput;
        public float horizontal;
        public float vertical;
        public float moveAmount;

        [Header("Button Presses")]
        public bool isSprintPressed;
        public bool isJumpPressed;
        public bool isDodgePressed;
        public bool isInteractPressed;
        public bool isEscapePressed;
        public bool isPrimaryButtonPressed;
        public bool isSecondaryButtonPressed;
        public bool isSwapWeaponPressed;

        [HideInInspector] public PlayerControls playerControls { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            DontDestroyOnLoad(instance);
            InitControls();
        }

        private void InitControls()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                //Movement
                playerControls.PlayerMovement.Movement.performed += playerControls => movementInput = playerControls.ReadValue<Vector2>();

                //Sprint
                playerControls.PlayerActions.Sprint.performed += playerControls => isSprintPressed = true;
                playerControls.PlayerActions.Sprint.canceled += playerControls => isSprintPressed = false;

                //Jump
                playerControls.PlayerActions.Jump.performed += playerControls => isJumpPressed = true;
                playerControls.PlayerActions.Jump.canceled += playerControls => isJumpPressed = false;

                //Dodge
                playerControls.PlayerActions.Dodge.performed += playerControls => isDodgePressed = true;
                //.PlayerActions.Dodge.canceled += playerControls => isDodgePressed = false;

                //Interact
                playerControls.PlayerActions.Interact.performed += playerControls => isInteractPressed = true;
                //playerControls.PlayerActions.Interact.canceled += playerControls => isInteractPressed = false;

                //Left Hand Action
                //playerControls.PlayerActions.PrimaryHand.started += playerControls => PrimaryHandInputPressed();
                playerControls.PlayerActions.PrimaryHand.started += playerControls => isPrimaryButtonPressed = true;
                //playerControls.PlayerActions.PrimaryHand.canceled += playerControls => isPrimaryButtonPressed = false;
                //playerControls.PlayerActions.PrimaryHand.canceled += playerControls => PrimaryHandInputReleased();
                // Off Hand Action
                //playerControls.PlayerActions.SecondaryHand.started += playerControls => SecondaryHandInputPressed();
                playerControls.PlayerActions.SecondaryHand.started += playerControls => isSecondaryButtonPressed = true;
                //playerControls.PlayerActions.SecondaryHand.canceled += playerControls => SecondaryHandInputReleased();
                playerControls.PlayerActions.SecondaryHand.canceled += playerControls => isSecondaryButtonPressed = false;


                playerControls.PlayerUI.Escape.performed += playerControls => isEscapePressed = true;

                playerControls.PlayerActions.SwapWeapon.performed += playerControls => isSwapWeaponPressed = true;
            }
        }

        private void Start()
        {
            LockCursor();
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        public void MovementInput()
        {
            vertical = movementInput.y;
            horizontal = movementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            isEscapePressed = false;
        }

        public void UpdateEscapePressed()
        {
            if(isEscapePressed)
            {
                UnlockCursor();
            }
        }

        /*private void SecondaryHandInputPressed()
        {
            handInputActions.SecondaryHandPress();
        }

        private void PrimaryHandInputPressed()
        {
            handInputActions.PrimaryHandPress();
        } 
        private void SecondaryHandInputReleased()
        {
            handInputActions.SecondaryHandRelease();
        }

        private void PrimaryHandInputReleased()
        {
            handInputActions.PrimaryHandRelease();
        }*/
    }
}

