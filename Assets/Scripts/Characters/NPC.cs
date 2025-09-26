using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, Iinteractable
{
    [SerializeField] private SpriteRenderer interactSprite;

    private Transform playerTransform;
    private const float INTERACT_DISTANCE = 1f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && WithininInteractDistance())
        {
            //interact with NPC
            Interact();
        }

        if (interactSprite.gameObject.activeSelf && !WithininInteractDistance()) 
        {
            //Turn oFf interact sprite
            interactSprite.gameObject.SetActive(false);
        }
        else if (!interactSprite.gameObject.activeSelf && WithininInteractDistance())
        {
            //Turn on interact sprite
            interactSprite.gameObject.SetActive(true);
        }
    }

    public abstract void Interact(); //override to other scripts
    
    private bool WithininInteractDistance()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) < INTERACT_DISTANCE) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
