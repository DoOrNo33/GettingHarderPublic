public class CharacterManager : Singleton<CharacterManager>
{
    private Player player;

    public UIInventory inventory;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
