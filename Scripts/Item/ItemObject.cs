using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemSO data;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interaction interaction = other.GetComponent<Interaction>();
            if (interaction != null)
            {
                interaction.OnInteractInput();
            }
        }
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemSO = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        if (CharacterManager.Instance.Player.itemSO == null )
        {
            Destroy(gameObject);
        }
    }
}
