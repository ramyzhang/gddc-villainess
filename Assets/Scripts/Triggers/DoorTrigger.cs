using UnityEngine;

public class DoorTrigger : GameTrigger
{
    public GameObject teleportTarget;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        triggerType = TriggerType.Door;
    }

    /**
    TODO: Make option to choose between trigger methods (collision, onclick, etc.) so it's a sexier gameplay experience
    **/
    public override void Interact() {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterManager>().TeleportCharacter(teleportTarget);
    }
}