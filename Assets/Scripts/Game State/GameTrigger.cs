using UnityEngine;

public class GameTrigger : MonoBehaviour
{
    [HideInInspector]
    public TriggerType triggerType;
    private bool inRange = false;

    /**
    TODO: Make option to choose between trigger methods (collision, onclick, etc.) so it's a sexier gameplay experience
    **/
    void OnMouseDown() {
        Debug.Log("I was clicked on!");
        if (!inRange) return;

        Interact();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("I'm in range!");
        inRange = true;
    }

    void OnTriggerExit2D(Collider2D collision) {
        Debug.Log("I'm out of range!");
        inRange = false;
    }

    // Override this in subclasses
    public virtual void Interact() { }
}