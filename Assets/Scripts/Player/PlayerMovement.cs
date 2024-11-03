using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float direction;
    public bool isFacingRight;
    private Animator anim;
    public CharacterManager cm;

    // Awake is called after all objects are initialized. Called in random order.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cm = GetComponent<CharacterManager>();
    }

    void Start()
    {
        isFacingRight = cm.facingRight;
    }

    // Update is called once per frame
    void Update()
    {
        //Relinquish player control if dialogue is running
        if (FindObjectOfType<DialogueRunner>().IsDialogueRunning == true)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            // anim.SetBool("isWalking", false);
            return;
        }

        //Move character
        direction = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(direction * 5f, rb.velocity.y);

        //Changing from idle animation to walk animation
        if (anim != null) {
            if (rb.velocity.x == 0) {
                anim.SetBool("isWalking", false);
            } else {
                anim.SetBool("isWalking", true);
            }
        }
        
        //Sprite flipping
        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
        {
            cm.FlipCharacter(!isFacingRight);
            isFacingRight = cm.facingRight;
        }
    }
}
