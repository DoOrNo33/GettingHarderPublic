using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MakeForest : MonoBehaviour
{
    [SerializeField] protected int width;   // 47, 47
    [SerializeField] protected int height;

    [SerializeField] protected string seed;
    [SerializeField] protected bool useRandomSeed;

    [Range(0, 100)]
    [SerializeField] protected int randomeFillPercent;
    [SerializeField] protected int smoothNum;
    [SerializeField] protected int interval;
    [SerializeField] protected int treeRand;
    [SerializeField] protected int startArea;
    [SerializeField] protected float CheckRadius;

    protected int[,] map;
    protected const int Road = 0;
    protected const int Wall = 1;

    public GameObject GameObject;

    public List<Vector3> objPositions;

    protected virtual void Awake()
    {

    }


    public void GenerateMap()
    {
        map = new int[width, height];
        MapRandomFill();

        for (int i = 0; i < smoothNum; i++)
        {
            SmoothMap();
        }

        SetObject();
    }

    public void LoadMap()
    {
        foreach (Vector3 pos in objPositions)
        {
            Instantiate(GameObject, pos, Quaternion.identity, EnvironmentManager.Instance.transform);
        }
    }

    protected virtual void SetObject()
    {

    }

    protected bool CheckObject(Vector3 pos)
    {
        Collider[] col = Physics.OverlapSphere(pos, CheckRadius);

        if (col.Length < 2) return true;
        else
        {
            foreach (var obj in col)        // Floor 오브젝트 제외하고 검사
            {
                if (obj.gameObject.layer == LayerMask.NameToLayer("Floor"))
                {
                    // 아무것도 안함
                }
                else
                {
                    //Debug.Log("겹침");
                    return false;
                }
            }
        }
        return true;
    }

    protected void MapRandomFill()
    {
        if (useRandomSeed)     // 랜덤 시드 사용
        {
            seed = Time.time.ToString();
        }

        System.Random rand = new System.Random(seed.GetHashCode()); //  시드값을 해시코드로 변환

        for (int x = 0; x < width; x += interval)
        {
            for (int y = 0; y < height; y += interval)
            {
                //if (x == 0 || x == width - interval || y == 0 || y == height - interval)
                //{
                //    map[x, y] = Wall;  // 맵의 가장자리는 벽으로 채움
                //}
                //else
                //{
                    map[x, y] = rand.Next(0, 100) < randomeFillPercent ? Wall : Road; // 랜덤으로 벽을 생성
            //}
             }
        }
    }
    protected void SmoothMap()
    {
        for (int x = 0; x < width; x += interval)
        {
            for (int y = 0; y < height; y += interval)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > 4)
                {
                    map[x, y] = Wall;  // 벽이 5개 이상이면 벽으로 채움
                }
                else if (neighbourWallTiles < 4)
                {
                    map[x, y] = Road;  // 벽이 3개 이하면 길로 채움
                }

                if (x < (width * 0.5 + startArea - interval) && x > (width * 0.5 - startArea) && y < (height * 0.5 + startArea - interval) && y > (height * 0.5 - startArea))
                {
                    map[x, y] = Road;  // 시작지점은 길로 채움
                }
            }
        }
    }

    protected int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;

        for (int neighbourX = gridX - interval; neighbourX <= gridX + interval; neighbourX += interval)
        {
            for (int neighbourY = gridY - interval; neighbourY <= gridY + interval; neighbourY += interval)     // 현재 좌표를 기준으로 주변 8칸 검사
            {
                if ((neighbourX >= 0) && (neighbourX < width) && (neighbourY >= 0) && neighbourY < height) // 맵을 초과하지 않는 조건문으로 검사
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY]; // 현재 좌표가 아니면 벽의 갯수를 증가
                    }
                }
                else wallCount++; // 맵을 초과하면 벽의 갯수를 증가
            }
        }
        return wallCount;
    }
}
