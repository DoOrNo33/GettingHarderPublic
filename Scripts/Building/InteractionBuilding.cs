using UnityEngine;


public class InteractionBuilding : MonoBehaviour, IConstructure
{
    [SerializeField]
    private Define.Constructure type;
    [SerializeField]
    private bool canInteraction;
    [SerializeField]
    private string player = "Player";
    [SerializeField]
    private GameObject keyInfo;
    [SerializeField]
    public GameObject InteractionInfo;
    // Start is called before the first frame update
    void Start()
    {
        keyInfo = UIManager.Instance.interactionTxt;
        InteractionInfo = UIManager.Instance.Panels[(int)type];
        ConstructData();
    }

    public void OnInteraction()
    {
        if (InteractionInfo.activeInHierarchy)
        {
            InteractionInfo.SetActive(false);
            keyInfo.SetActive(true);
        }
        else
        {
            InteractionInfo.SetActive(true);
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (canInteraction)
        {
           if (other.gameObject.CompareTag(player))
            {
                keyInfo.SetActive(true);
                CharacterManager.Instance.Player.controller.CallbackInteraction.AddListener(OnInteraction);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (canInteraction)
        {
            if (other.gameObject.CompareTag(player))
            {
                CharacterManager.Instance.Player.controller.CallbackInteraction.RemoveListener(OnInteraction);
                InteractionInfo.SetActive(false);
                keyInfo.SetActive(false);
            }
        }
    }

    public void ConstructData()
    {
        SaveBuilding saveBuilding = new SaveBuilding();

        switch (type)
        {
            case Define.Constructure.Craft:
                saveBuilding.itemID = (int)Define.SaveConID.Craft;
                break;
            case Define.Constructure.Sleep:
                saveBuilding.itemID = (int)Define.SaveConID.Sleep;
                break;
            case Define.Constructure.None:
                saveBuilding.itemID = (int)Define.SaveConID.None;
                break;
        }
        saveBuilding.buildingPos = transform.position;
        saveBuilding.buildingRot = transform.rotation;
        DataManager.Instance.data.buildings.Add(saveBuilding);
    }
}
