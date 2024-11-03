using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class CharacterManager : MonoBehaviour
{
    private string characterName;
    public bool facingRight = true;

    // Expression Management Variables
    public bool hasExpressions;
    public CharacterExpressions expressionSet;
    private SpriteRenderer leftbrow;
    private SpriteRenderer rightbrow;
    private SpriteRenderer nosemouth;
    private SpriteRenderer eyes;
    private bool hasBrows = true, hasNoseMouth = true;

    private Animator anim;
    private Rigidbody2D rb;

    void Awake() {
        characterName = gameObject.name;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Expression management logic
        string expressionPath = $"Constants/Expressions/{characterName}Expressions";
        expressionSet = Resources.Load<CharacterExpressions>(expressionPath);

        if (expressionSet == null) {
            hasExpressions = false;
        } else {
            hasExpressions = true;

            if (!(characterName.Equals("Catherine") || characterName.Equals("Oliver"))) {
                hasBrows = false;
            }
            if (characterName.Equals("MobMC")) {
                hasNoseMouth = false;
            }

            if (hasBrows) {
                leftbrow = transform.Find("body/head/leftbrow").gameObject.GetComponent<SpriteRenderer>();
                rightbrow = transform.Find("body/head/rightbrow").gameObject.GetComponent<SpriteRenderer>();
            }
            if (hasNoseMouth) {
                nosemouth = transform.Find("body/head/nosemouth").gameObject.GetComponent<SpriteRenderer>();
            }
            eyes = transform.Find("body/head/eyes").gameObject.GetComponent<SpriteRenderer>();
        }
    }

    [YarnCommand("changeExpression")]
    public void ChangeExpression(string expression) {
        Debug.Log($"Making {characterName} {expression}!");
        if (!hasExpressions) {
            Debug.Log("This character doesn't have an expression set yet.");
            return;
        }
        if (!expressionSet.expressionLookup.ContainsKey(expression)) {
            Debug.Log("Expression type invalid.");
            return;
        }

        Sprite newEyes = expressionSet.expressionLookup[expression].eyes;
        eyes.sprite = newEyes;
        if (hasBrows) {
            Sprite newBrow = expressionSet.expressionLookup[expression].eyebrows;
            leftbrow.sprite = newBrow;
            rightbrow.sprite = newBrow;
        }
        if (hasNoseMouth) {
            Sprite newNosemouth = expressionSet.expressionLookup[expression].nosemouth;
            nosemouth.sprite = newNosemouth;
        }
    }

    // Characters face right by default
    // TODO: add flipping commands to Yarn scripts once all scenes are fully set up
    [YarnCommand("flipCharacter")]
    public void FlipCharacter(bool faceRight) {
        if (!faceRight && facingRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            facingRight = false;
        } else if (faceRight && !facingRight) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            facingRight = true;
        }
    }

    public IEnumerator MoveCharacter(GameObject location) {
        Debug.Log($"Moving {characterName} to {location}");
        Vector3 newPos = location.transform.position;
        Vector2 positionOffset = new Vector2(9f, Physics2D.gravity.y * rb.gravityScale);

        Vector2 negative = new Vector2(-1f, 1f);
        Vector2 positive = new Vector2(1f, 1f);

        if (anim != null) {
            anim.SetBool("isWalking", true);

            if (transform.position.x > newPos.x) {
                FlipCharacter(false);
                while (transform.position.x > newPos.x) {
                    rb.MovePosition(rb.position + positionOffset * negative * Time.fixedDeltaTime);
                    yield return 0;
                }
                anim.SetBool("isWalking", false);

            } else if (transform.position.x < newPos.x) {
                FlipCharacter(true);
                while (transform.position.x < newPos.x) {
                    rb.MovePosition(rb.position + positionOffset * positive * Time.fixedDeltaTime);
                    yield return 0;
                }
                anim.SetBool("isWalking", false);
            }
        }
    }

    [YarnCommand("animateCharacter")]
    public void AnimateCharacter(string animName, int layer = 0) {
        anim.Play(animName, layer);
    }

    public void ChangeAnimationSpeed(string speedMultiplierName, float speedMultiplier)
    {
        anim.SetFloat(speedMultiplierName, speedMultiplier);
    }

    [YarnCommand("teleportCharacter")]
    public void TeleportCharacter(GameObject location) {
        Debug.Log($"Teleporting {characterName} to {location}");
        transform.position = location.transform.position;
    }

    public void StopWalking() {
        if (anim != null) {
            anim.SetBool("isWalking", false);
        }
    }
}
