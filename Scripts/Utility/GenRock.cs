using UnityEngine;

public class GenRock : MakeForest
{
    protected override void SetObject()
    {
        for (int x = 0; x < width; x += interval)
        {
            for (int y = 0; y < height; y += interval)
            {
                if (map[x, y] == Wall)
                {
                    int randX = UnityEngine.Random.Range(-treeRand, treeRand + 1);
                    int randY = UnityEngine.Random.Range(-treeRand, treeRand + 1);
                    Vector3 pos = new Vector3(x - (width * 0.5f) + (interval * 0.5f) + randX, 0, y - (height * 0.5f) + (interval * 0.5f) + randY);

                    if (CheckObject(pos))
                    {
                        Instantiate(GameObject, pos, Quaternion.identity, EnvironmentManager.Instance.transform);  
                        objPositions.Add(pos);        
                    }
                }
            }
        }
    }
}


