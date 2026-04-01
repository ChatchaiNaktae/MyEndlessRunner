using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// What: Custom Unity Editor tool to view and reset player save data.
public class SaveDebugger : EditorWindow
{
    // สร้างเมนูใหม่ด้านบนของ Unity ชื่อ Tools -> Game Save Debugger
    [MenuItem("Tools/Game Save Debugger")]
    public static void ShowWindow()
    {
        GetWindow<SaveDebugger>("Save Debugger");
    }
    
    void OnGUI()
    {
        GUILayout.Label("Player Data (Read Only)", EditorStyles.boldLabel);
        
        // ดึงข้อมูลมาโชว์ในหน้าต่าง
        EditorGUILayout.IntField("Total Coins", PlayerPrefs.GetInt("TotalCoins", 0));
        EditorGUILayout.IntField("High Score", PlayerPrefs.GetInt("HighScore", 0));
        EditorGUILayout.IntField("Total Deaths", PlayerPrefs.GetInt("TotalDeaths", 0));
        EditorGUILayout.IntField("Selected Pet ID", PlayerPrefs.GetInt("SelectedPetIndex", 0));
        
        GUILayout.Space(10);
        GUILayout.Label("Pet Unlock Status", EditorStyles.boldLabel);
        
        // วนลูปเช็คสถานะสัตว์เลี้ยงสัก 5 ตัว
        for (int i = 0; i < 5; i++)
        {
            string key = "PetUnlocked_" + i;
            if (PlayerPrefs.HasKey(key))
            {
                bool isUnlocked = PlayerPrefs.GetInt(key, 0) == 1;
                EditorGUILayout.Toggle("Pet ID " + i + " Unlocked", isUnlocked);
            }
        }
        
        GUILayout.Space(20);
        
        // ปุ่มสำหรับลบเซฟทิ้งทั้งหมดเพื่อเริ่มทดสอบใหม่
        if (GUILayout.Button("Reset All Save Data (Delete PlayerPrefs)", GUILayout.Height(30)))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.LogWarning("🔥 All Save Data has been wiped! 🔥");
        }
    }
}