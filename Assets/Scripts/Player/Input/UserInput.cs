using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{
    
    public static UserInput instance;

    [HideInInspector] public PlayerInput controls;
    [HideInInspector] public Vector2 moveInput;
    private void Awake()
    {
        controls = new PlayerInput();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //keep this object when loading new scenes
        }
        else 
        {
            Destroy(gameObject); //destroy duplicate
            return;
        }

        controls = new PlayerInput();

        controls.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); //Grabbing Inputs input on move
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        if (controls != null) 
        {
            controls.Disable();
        }
    }
        
}
