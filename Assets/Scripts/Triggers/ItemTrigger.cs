public class ItemTrigger : GameTrigger
{
    public Item item;	// Item to put in the inventory if picked up

    // Start is called before the first frame update
    void Start()
    {
        triggerType = TriggerType.Item;
    }

    /**
    TODO: Make option to choose between trigger methods (collision, onclick, etc.) so it's a sexier gameplay experience
    **/
    public override void Interact() {
        Inventory.instance.AddItem(item);	// Add to inventory
        Destroy(gameObject);	// Destroy item from scene  
    }
}