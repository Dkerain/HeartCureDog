using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using NodeCanvas.Framework;
using Newtonsoft.Json;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string saveFilePath;
    private SaveGameCollection currentSaves;

    // 章节标题映射
    private Dictionary<int, string> chapterTitles = new Dictionary<int, string>()
    {
        {0, "第零章：章节测试"},
        {1, "第一章：新的开始"},
        {2, "第二章：冒险启程"},
        {3, "第三章：黑暗森林"},
        // 添加更多章节...
    };
    private Dictionary<int, string> chapterScenes = new Dictionary<int, string>()
    {
        {0, "LivingroomScene"},
        {1, "LivingroomScene"},
        {2, "LivingroomScene"},
        {3, "LivingroomScene"},
        // 添加更多章节场景...
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSaveSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSaveSystem()
    {
        // 获取 AppData/Local 文件夹路径
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string gameFolder = Path.Combine(appDataPath, "HeartCureDogs");
        saveFilePath = Path.Combine(gameFolder, "saves.json");

        // 确保目录存在
        Directory.CreateDirectory(gameFolder);

        LoadAllSaves();
    }

    public void SaveGame(int slotIndex)
    {
        Debug.Log($"开始保存到存档位{slotIndex}");
        if (currentSaves == null)
        {
            Debug.LogError("currentsSaves为null!");
            return;
        }

        // 获取当前章节
        Blackboard globalBlackboard = GlobalBlackboard.Find("Global");
        if (globalBlackboard == null)
        {
            Debug.LogError("全局黑板未找到！");
            return;
        }

        int currentChapter = globalBlackboard.GetVariableValue<int>("Chapter");
        Debug.Log($"当前章节：{currentChapter}");

        // 创建存档数据
        SaveData saveData = new SaveData
        {
            Chapter = currentChapter,
            Title = GetChapterTitle(currentChapter),
            SaveTime = DateTime.Now,
            SceneName = GetChapterScene(currentChapter) // 保存场景名
        };
        Debug.Log($"创建存档数据：Chapter={saveData.Chapter}, Scene={saveData.SceneName}");

        // 确保有足够的存档位
        while (currentSaves.SaveSlots.Count <= slotIndex)
        {
            currentSaves.SaveSlots.Add(new SaveData());
        }

        // 保存到指定位置
        currentSaves.SaveSlots[slotIndex] = saveData;

        // 保存到文件
        SaveToFile();

        Debug.Log($"游戏已保存到存档位 {slotIndex + 1}");
    }

    public void LoadGame(int slotIndex)
    {
        if (currentSaves == null || currentSaves.SaveSlots.Count <= slotIndex) return;

        SaveData saveData = currentSaves.SaveSlots[slotIndex];

        // 检查是否是空存档
        if (saveData.Chapter == 0)
        {
            Debug.Log("该存档位为空");
            return;
        }

        // 加载到全局黑板
        Blackboard globalBlackboard = GlobalBlackboard.Find("Global");
        if (globalBlackboard != null)
        {
            globalBlackboard.SetVariableValue("Chapter", saveData.Chapter);
            Debug.Log($"已加载存档：第{saveData.Chapter}章 - {saveData.Title}");
        }

        // 这里可以添加场景加载等其他逻辑
        // 保存要加载的场景信息（供场景加载器使用）
        if (!string.IsNullOrEmpty(saveData.SceneName))
        {
            PlayerPrefs.SetString("LoadTargetScene", saveData.SceneName);
            PlayerPrefs.Save();
            Debug.Log($"设置目标场景: {saveData.SceneName}");
        }
    }
    // 获取存档对应的场景名
    public string GetSaveSceneName(int slotIndex)
    {
        if (currentSaves == null || currentSaves.SaveSlots.Count <= slotIndex)
            return string.Empty;

        SaveData saveData = currentSaves.SaveSlots[slotIndex];
        return GetChapterScene(saveData.Chapter);
    }



    public string GetSaveDisplayText(int slotIndex)
    {
        if (currentSaves == null)
        { 
            Debug.LogError("currentSaves为null");
            return "存档系统错误";
        }
            
        if(currentSaves.SaveSlots.Count <= slotIndex)
        {
            Debug.LogError($"存档位{slotIndex}超出范围!总数：{currentSaves.SaveSlots.Count}");
            return "存档位错误";
        }
            
        SaveData saveData = currentSaves.SaveSlots[slotIndex];

        if (saveData.Chapter == 0)
        {
            Debug.Log($"存档位{slotIndex}为空存档");
            return "空存档位";
        }
        string title = GetChapterTitle(saveData.Chapter);
        string displayText = $"第{saveData.Chapter}章: {saveData.Title}\n{saveData.SaveTime:yyyy-MM-dd HH:mm}";
        Debug.Log($"存档位 {slotIndex} 显示文本: {displayText}");
        return displayText;
        //return $"第{saveData.Chapter}章: {saveData.Title}\n{saveData.SaveTime:yyyy-MM-dd HH:mm}";
    }

    public bool IsSaveSlotEmpty(int slotIndex)
    {
        if (currentSaves == null || currentSaves.SaveSlots.Count <= slotIndex)
            return true;

        return currentSaves.SaveSlots[slotIndex].Chapter == 0;
    }

    private string GetChapterTitle(int chapter)
    {
        return chapterTitles.ContainsKey(chapter) ? chapterTitles[chapter] : $"第{chapter}章";
    }
    private string GetChapterScene(int chapter)
    {
        return chapterScenes.ContainsKey(chapter) ? chapterScenes[chapter] : "MainScene";
    }
    private void SaveToFile()
    {
        try
        {
            string json = JsonConvert.SerializeObject(currentSaves, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"存档已写入文件：{saveFilePath}");
            if(File.Exists(saveFilePath))
            {
                string fileContent=File.ReadAllText(saveFilePath);
                Debug.Log($"文件内容：{fileContent}");
            }
            else 
            {
                Debug.Log("存档文件不存在");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"保存失败: {e.Message}");
        }
    }

    private void LoadAllSaves()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                Debug.Log($"从文件中读取存档{json}");
                currentSaves = JsonConvert.DeserializeObject<SaveGameCollection>(json);
                Debug.Log($"成功加载存档，共{currentSaves.SaveSlots.Count}个存档位");
                // 打印所有存档位信息
                for (int i = 0; i < currentSaves.SaveSlots.Count; i++)
                {
                    SaveData save = currentSaves.SaveSlots[i];
                    Debug.Log($"存档位 {i}: Chapter={save.Chapter}, Title={save.Title}");
                }
            }
            else
            {
                Debug.Log("存档文件不存在，创建新存档");
                currentSaves = new SaveGameCollection();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"读取存档失败: {e.Message}");
            currentSaves = new SaveGameCollection();
        }
    }
}
