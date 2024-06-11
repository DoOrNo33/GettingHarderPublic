using SurvivalEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField]
    private ItemSO[] items;
    [SerializeField]
    private ItemSO[] buildings;

    public UserData data;

    string key = "key";  // 암호화보안키

    // 시간
    public Action<float> timeSet;
    public float dailyCycleTime;

    private void Update()       // TODO: 세이브 버튼UI에 할당
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnSaveData();
        } 

        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnLoadData();
        }
    }

    // 버튼 할당
    public void OnSaveData()
    {
        DataSerialize();

        var json = JsonUtility.ToJson(data);

        json = AESWithJava.Con.Program.Encrypt(json, key);  // 암호화

        File.WriteAllText(Application.persistentDataPath + "/UserData.json", json);
        Debug.Log(Application.persistentDataPath + "/UserData.json" + "에 저장되었습니다.");

    }

    public void OnLoadData()
    {
        var jsonData = File.ReadAllText(Application.persistentDataPath + "/UserData.json");

        jsonData = AESWithJava.Con.Program.Decrypt(jsonData, key);  // 복호화

        data = JsonUtility.FromJson<UserData>(jsonData);

        EnvironmentManager.Instance.ClearEnvironment();  // 환경 초기화

        DataDeserialize();
    }

    private void DataSerialize()
    {
        data.curHealth = CharacterManager.Instance.Player.condition.curHealth();
        data.curHunger = CharacterManager.Instance.Player.condition.curHunger();
        data.curStamina = CharacterManager.Instance.Player.condition.curStamina();
        data.curThirst = CharacterManager.Instance.Player.condition.curThirst();

        data.curPos = CharacterManager.Instance.Player.transform.position;
        data.curRot = CharacterManager.Instance.Player.transform.rotation;

        data.waterPondPos = EnvironmentManager.Instance.WaterPond.objPositions;
        data.treeLeafPos = EnvironmentManager.Instance.TreeLeaf.objPositions;
        data.treeApplePos = EnvironmentManager.Instance.TreeApple.objPositions;
        data.rockPos = EnvironmentManager.Instance.Rock.objPositions;
        data.ironPos = EnvironmentManager.Instance.Iron.objPositions;
        data.goldPos = EnvironmentManager.Instance.Gold.objPositions;
        data.rabbitPos = EnvironmentManager.Instance.Rabbit.objPositions;
        data.deerPos = EnvironmentManager.Instance.Deer.objPositions;
        data.bisonPos = EnvironmentManager.Instance.Bison.objPositions;
        data.wolfPos = EnvironmentManager.Instance.Wolf.objPositions;
        data.woodPos = EnvironmentManager.Instance.Wood.objPositions;
        data.stonePos = EnvironmentManager.Instance.Stone.objPositions;
        data.carrotPos = EnvironmentManager.Instance.Carrot.objPositions;
        data.grassPos = EnvironmentManager.Instance.Grass.objPositions;

        foreach (var slot in CharacterManager.Instance.inventory.slots)
        {
            if (slot.item != null)
            {
                SaveInventory saveData = new SaveInventory();
                saveData.itemID = slot.item.itemID;
                saveData.quantity = slot.quantity;
                saveData.equipped = slot.equipped;
                data.inventories.Add(saveData);
            }
        }

        data.SaveTime = dailyCycleTime;
    }

    private void DataDeserialize()
    {
        CharacterManager.Instance.Player.condition.setHealth(data.curHealth);
        CharacterManager.Instance.Player.condition.setHunger(data.curHunger);
        CharacterManager.Instance.Player.condition.setStamina(data.curStamina);
        CharacterManager.Instance.Player.condition.setThirst(data.curThirst);

        CharacterManager.Instance.Player.transform.position = data.curPos;
        CharacterManager.Instance.Player.transform.rotation = data.curRot;

        foreach (var building in data.buildings)
        {
            ItemSO itemSO = CheckBuildingSO(building.itemID);
            if (itemSO != null)
            {
                Instantiate(itemSO.itemPrefab, building.buildingPos, building.buildingRot);
            }
        }

        EnvironmentManager.Instance.WaterPond.objPositions = data.waterPondPos;
        EnvironmentManager.Instance.TreeLeaf.objPositions = data.treeLeafPos;
        EnvironmentManager.Instance.TreeApple.objPositions = data.treeApplePos;
        EnvironmentManager.Instance.Rock.objPositions = data.rockPos;
        EnvironmentManager.Instance.Iron.objPositions = data.ironPos;
        EnvironmentManager.Instance.Gold.objPositions = data.goldPos;
        EnvironmentManager.Instance.Rabbit.objPositions = data.rabbitPos;
        EnvironmentManager.Instance.Deer.objPositions = data.deerPos;
        EnvironmentManager.Instance.Bison.objPositions = data.bisonPos;
        EnvironmentManager.Instance.Wolf.objPositions = data.wolfPos;
        EnvironmentManager.Instance.Wood.objPositions = data.woodPos;
        EnvironmentManager.Instance.Stone.objPositions = data.stonePos;
        EnvironmentManager.Instance.Carrot.objPositions = data.carrotPos;
        EnvironmentManager.Instance.Grass.objPositions = data.grassPos;

        EnvironmentManager.Instance.LoadForest();

        foreach (var slot in data.inventories)
        {
            ItemSO itemSO = CheckSO(slot.itemID);
            if (itemSO != null)
            {
                CharacterManager.Instance.inventory.AddItem(itemSO, slot.quantity, slot.equipped);
            }
        }

        timeSet?.Invoke(data.SaveTime);

        GameManager.Instance.gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        CharacterManager.Instance.Player.GetComponent<Animator>().SetTrigger("Load");

    }

    private ItemSO CheckSO(int itemID)
    {
        foreach (ItemSO itemSOs in items)
        {
            if (itemSOs.itemID == itemID)
            {
                return itemSOs;
            }
        }

        return null;
    }
    private ItemSO CheckBuildingSO(int itemID)
    {
        foreach (ItemSO buildingSOs in buildings)
        {
            if (buildingSOs.itemID == itemID)
            {
                return buildingSOs;
            }
        }

        return null;
    }

    public void UpdateTime(float time)
    {
        dailyCycleTime = time;
    }

}



[System.Serializable]
public class UserData
{
    public float curHealth;
    public float curHunger;
    public float curStamina;
    public float curThirst;

    public Vector3 curPos;
    public Quaternion curRot;

    public List<Vector3> waterPondPos;
    public List<Vector3> treeLeafPos;
    public List<Vector3> treeApplePos;
    public List<Vector3> rockPos;
    public List<Vector3> ironPos;
    public List<Vector3> goldPos;
    public List<Vector3> rabbitPos;
    public List<Vector3> deerPos;
    public List<Vector3> bisonPos;
    public List<Vector3> wolfPos;
    public List<Vector3> woodPos;
    public List<Vector3> stonePos;
    public List<Vector3> carrotPos;
    public List<Vector3> grassPos;

    public InventoryItemSlot[] slots;
    public HandItemSlot[] handSlots;
    public EquipItemSlot[] equipSlot;

    public List<SaveInventory> inventories;

    public List<SaveBuilding> buildings;

    public float SaveTime;
}

[System.Serializable]
public struct SaveInventory
{
    public int itemID;
    public int quantity;
    public bool equipped;
}

[System.Serializable]
public struct SaveBuilding
{
    public int itemID;
    public Vector3 buildingPos;
    public Quaternion buildingRot;
}


// 암호화 복호화
namespace AESWithJava.Con

{

    class Program

    {

        static void Main(string[] args)

        {

            String originalText = "plain text";

            String key = "key";

            String en = Encrypt(originalText, key);

            String de = Decrypt(en, key);



            Console.WriteLine("Original Text is " + originalText);

            Console.WriteLine("Encrypted Text is " + en);

            Console.WriteLine("Decrypted Text is " + de);

        }



        public static string Decrypt(string textToDecrypt, string key)

        {

            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;



            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);

            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length)

            {

                len = keyBytes.Length;

            }

            Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;

            rijndaelCipher.IV = keyBytes;

            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);

        }



        public static string Encrypt(string textToEncrypt, string key)

        {

            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;



            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length)

            {

                len = keyBytes.Length;

            }

            Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;

            rijndaelCipher.IV = keyBytes;

            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));

        }



    }

}