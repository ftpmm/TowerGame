using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager
{
    private static GameLevelManager _instance;

    public static GameLevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameLevelManager();
            }

            return _instance;
        }
    }

    private LevelBase curLevel;

    public void Init()
    {

    }

    public void StartLevel(int levelId)
    {
        var normalLevel = GameObject.FindAnyObjectByType<LevelNormal>();
        normalLevel.InitLevel(levelId);
        curLevel = normalLevel;
    }

    public Vector2Int TryGetNextMapPos(Vector3 pos, out Vector2 retPos)
    {
            return curLevel.TryGetNextMapPos(pos, out retPos);
    }

    public Vector2 GetWalkablePos(Vector2 pos)
    {
        return curLevel.GetWalkablePos(pos);
    }

    public bool IsWalkable(Vector3 pos)
    {
        return curLevel.IsWalkable(pos);
    }
}