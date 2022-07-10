using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float direction;
    private bool flipped;
    private Animator anim;

    // Awake is called after all objects are initialized. Called in random order.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Relinquish player control if dialogue is running
        if (FindObjectOfType<DialogueRunner>().IsDialogueRunning == true)
        {
            return;
        }

        //Move character
        direction = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(direction * 5f, rb.velocity.y);

        //Changing from idle animation to walk animation
        if (direction == 0) {
            anim.SetBool("isWalking", false);
        } else {
            anim.SetBool("isWalking", true);
        }

        //Sprite flipping mechanism
        if ((direction > 0 && flipped) || (direction < 0 && !flipped))
        {
            transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);
            flipped = !flipped;
        }
    }
}
