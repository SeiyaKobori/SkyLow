using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public class Level
    {
        public int level = -1;
        public int generateStepSpan = 0;
        public float gravityPower = 0;
        public float jammerSpan = 0; //分間何個現れるか
        public float itemSpan = 0; // 分間何個現れるか
    }

    private void Start()
    {
        CheckLevelList();
    }

    private void CheckLevelList()
    {
        List<int> levels = new List<int>();
        foreach(var l in levelList)
        {
            if(levels.Contains(l.level))
            {
                Debug.LogWarning("Level情報が重複している箇所が存在します。:" + l.level);
                return;
            }
            levels.Add(l.level);
        }
    }

    public int GetGenerateStepSpan(int level)
    {
        foreach(var l in levelList)
        {
            if (l.level != level)
                continue;
            return l.generateStepSpan;
        }

        Debug.LogWarning("指定したレベルがありません : " + level);
        return -1;
    }

    public float GetGravityPower(int level)
    {
        foreach (var l in levelList)
        {
            if (l.level != level)
                continue;
            return l.gravityPower;
        }

        Debug.LogWarning("指定したレベルがありません : " + level);
        return -1;
    }

    public float GetjammerSpan(int level)
    {
        foreach (var l in levelList)
        {
            if (l.level != level)
                continue;
            return l.jammerSpan;
        }

        Debug.LogWarning("指定したレベルがありません : " + level);
        return -1;
    }

    public float GetItemSpan(int level)
    {
        foreach (var l in levelList)
        {
            if (l.level != level)
                continue;
            return l.itemSpan;
        }

        Debug.LogWarning("指定したレベルがありません : " + level);
        return -1;
    }

    private List<Level> levelList = new List<Level>()
    {
        new Level()
        {
            level = 1,
            generateStepSpan = 40,
            gravityPower = 50,
            jammerSpan = 30,
            itemSpan = 30,
        },

        new Level()
        {
            level = 2,
            generateStepSpan = 50,
            gravityPower = 51,
            jammerSpan = 31,
            itemSpan = 30,
        },

        new Level()
        {
            level = 3,
            generateStepSpan = 60,
            gravityPower = 52,
            jammerSpan = 32,
            itemSpan = 31,
        },

        new Level()
        {
            level = 4,
            generateStepSpan = 70,
            gravityPower = 52,
            jammerSpan = 33,
            itemSpan = 31,
        },

        new Level()
        {
            level = 5,
            generateStepSpan = 80,
            gravityPower = 53,
            jammerSpan = 34,
            itemSpan = 32,
        },

        new Level()
        {
            level = 6,
            generateStepSpan = 90,
            gravityPower = 53,
            jammerSpan = 35,
            itemSpan = 32,
        },

        new Level()
        {
            level = 7,
            generateStepSpan = 100,
            gravityPower = 54,
            jammerSpan = 36,
            itemSpan = 33,
        },

        new Level()
        {
            level = 8,
            generateStepSpan = 110,
            gravityPower = 54,
            jammerSpan = 37,
            itemSpan = 33,
        },

        new Level()
        {
            level = 9,
            generateStepSpan = 120,
            gravityPower = 55,
            jammerSpan = 36,
            itemSpan = 34,
        },

        new Level()
        {
            level = 10,
            generateStepSpan = 120,
            gravityPower = 55,
            jammerSpan = 37,
            itemSpan = 34,
        },

        new Level()
        {
            level = 11,
            generateStepSpan = 120,
            gravityPower = 56,
            jammerSpan = 36,
            itemSpan = 35,
        },

        new Level()
        {
            level = 12,
            generateStepSpan = 120,
            gravityPower = 56,
            jammerSpan = 37,
            itemSpan = 35,
        },

        new Level()
        {
            level = 13,
            generateStepSpan = 120,
            gravityPower = 57,
            jammerSpan = 38,
            itemSpan = 36,
        },

        new Level()
        {
            level = 14,
            generateStepSpan = 120,
            gravityPower = 57,
            jammerSpan = 39,
            itemSpan = 36,
        },

        new Level()
        {
            level = 15,
            generateStepSpan = 120,
            gravityPower = 58,
            jammerSpan = 40,
            itemSpan = 37,
        },

        new Level()
        {
            level = 16,
            generateStepSpan = 120,
            gravityPower = 58,
            jammerSpan = 41,
            itemSpan = 37,
        },

        new Level()
        {
            level = 17,
            generateStepSpan = 120,
            gravityPower = 59,
            jammerSpan = 42,
            itemSpan = 38,
        },

        new Level()
        {
            level = 18,
            generateStepSpan = 120,
            gravityPower = 59,
            jammerSpan = 43,
            itemSpan = 38,
        },

        new Level()
        {
            level = 19,
            generateStepSpan = 120,
            gravityPower = 60,
            jammerSpan = 44,
            itemSpan = 39,
        },

        new Level()
        {
            level = 20,
            generateStepSpan = 120,
            gravityPower = 60,
            jammerSpan = 45,
            itemSpan = 39,
        },

        new Level()
        {
            level = 21,
            generateStepSpan = 120,
            gravityPower = 61,
            jammerSpan = 46,
            itemSpan = 40,
        },

        new Level()
        {
            level = 22,
            generateStepSpan = 120,
            gravityPower = 61,
            jammerSpan = 47,
            itemSpan = 40,
        },

        new Level()
        {
            level = 23,
            generateStepSpan = 120,
            gravityPower = 62,
            jammerSpan = 48,
            itemSpan = 41,
        },

        new Level()
        {
            level = 24,
            generateStepSpan = 120,
            gravityPower = 62,
            jammerSpan = 49,
            itemSpan = 41,
        },

        new Level()
        {
            level = 25,
            generateStepSpan = 120,
            gravityPower = 63,
            jammerSpan = 50,
            itemSpan = 42,
        },

        new Level()
        {
            level = 26,
            generateStepSpan = 120,
            gravityPower = 63,
            jammerSpan = 51,
            itemSpan = 42,
        },
    };
}
