using System;
using UnityEngine;

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    public Respawn[] respawns;

    public MakeForest WaterPond;
    public MakeForest TreeLeaf;
    public MakeForest TreeApple;
    public MakeForest Rock;
    public MakeForest Iron;
    public MakeForest Gold;
    public MakeForest Rabbit;
    public MakeForest Deer;
    public MakeForest Bison;
    public MakeForest Wolf;
    public MakeForest Wood;
    public MakeForest Stone;
    public MakeForest Carrot;
    public MakeForest Grass;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GameManager.Instance.OnSleep += SetEnvironmentActive;
    }

    // 임시
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetEnvironmentActive();
        }
    }

    public void GenerateForest()
    {
        WaterPond.GenerateMap();
        TreeLeaf.GenerateMap();
        TreeApple.GenerateMap();
        Rock.GenerateMap();
        Iron.GenerateMap();
        Gold.GenerateMap();
        Rabbit.GenerateMap();
        Deer.GenerateMap();
        Bison.GenerateMap();
        Wolf.GenerateMap();
        Wood.GenerateMap();
        Stone.GenerateMap();
        Carrot.GenerateMap();
        Grass.GenerateMap();

        CollectEnvironments();
    }

    public void LoadForest()
    {
        WaterPond.LoadMap();
        TreeLeaf.LoadMap();
        TreeApple.LoadMap();
        Rock.LoadMap();
        Iron.LoadMap();
        Gold.LoadMap();
        Rabbit.LoadMap();
        Deer.LoadMap();
        Bison.LoadMap();
        Wolf.LoadMap();
        Wood.LoadMap();
        Stone.LoadMap();
        Carrot.LoadMap();
        Grass.LoadMap();

        CollectEnvironments();
    }

    public void CollectEnvironments()
    {
        // 이 오브젝트의 자식 오브젝트 수집
        respawns = new Respawn[transform.childCount];

        //EnvironmentManager 오브젝트 아래 오브젝트의 Respawn 컴포넌트를 모두 수집
        for (int i = 0; i < transform.childCount; i++)
        {
            respawns[i] = transform.GetChild(i).GetComponent<Respawn>();
        }

    }

    public void SetEnvironmentActive()
    {
        foreach (Respawn respawn in respawns)
        {
            if (respawn != null)
            {
                respawn.OnRespawn();
            }
        }
    }

    public void ClearEnvironment()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}