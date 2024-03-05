using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEditor;

public static class SaveSystem
{
    public static void SavePlayer ( PlayerStats playerStats){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "playerData.bin");
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerStats);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(){
        string path = Path.Combine(Application.persistentDataPath, "playerData.bin");
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }else{
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveAbilities(List<Ability> abilitiesToSave, List<Ability> selectedAbilitiesToSave){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "abilitiesData.bin");
        FileStream stream = new FileStream(path, FileMode.Create);

        AbilitiesData data = new AbilitiesData(abilitiesToSave, selectedAbilitiesToSave);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public static AbilitiesData LoadAbilities(){
        string path = Path.Combine(Application.persistentDataPath, "abilitiesData.bin");
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AbilitiesData data = formatter.Deserialize(stream) as AbilitiesData;
            stream.Close();
            return data;
        }else{
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }


    public static void SaveInventory(Dictionary<ItemObject,int> inventory, Dictionary<string, string> equipedItems){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "inventoryData.bin");
        FileStream stream = new FileStream(path, FileMode.Create);

        InventoryData data = new InventoryData(inventory, equipedItems);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static InventoryData LoadInventory(){
        string path = Path.Combine(Application.persistentDataPath, "inventoryData.bin");
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            var data = formatter.Deserialize(stream) as InventoryData;
            stream.Close();
            if(data.equipedItems == null){
                data.equipedItems = new Dictionary<string, string>
                {
                    { "HeadSlot", null },
                    { "HandSlot", null}
                };
                
            }
            if(data.inventory == null){
                data.inventory = new Dictionary<string, int>();
            }
            return data;
        }else{
            Debug.LogError("Save file not found in " + path);
            var newEquipedItems = new Dictionary<string, string>
                {
                    { "HeadSlot", null },
                    { "HandSlot", null}
                };
            return new InventoryData(new Dictionary<ItemObject, int>(), newEquipedItems);
        }
    }
}
