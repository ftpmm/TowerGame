using lzengine;
using UnityEngine;

public class LevelNormal : LevelBase
{
    public override void DoStart()
    {

    }

    public override void DoUpdate(float deltaTime)
    {

    }

    public override void InitLevel(int levelId)
    {
        base.InitLevel(levelId);
        //初始化关卡
        if(mMap == null)
        {
            LZDebug.LogError("Map is null, InitLevel failed!!!");
            return;
        }

        var targetActor = GameObject.FindAnyObjectByType<TargetActor>();
        if (targetActor == null)
        {
            LZDebug.LogError("Target is null, InitLevel failed!!!");
            return; ;
        }
        Vector3 pos = targetActor.transform.position;
        Vector2Int targetCellPos = GetLevelCellByPos(pos);
        Vector2 targetCenterPos = GetLevelMapCenterPos(targetCellPos);
        targetActor.transform.position = targetCenterPos ;

        mMap.GenerateMap(new Vector2Int(sizeX, sizeY), targetCellPos);
        mMapPreview.CloneFrom(mMap);
    }
}