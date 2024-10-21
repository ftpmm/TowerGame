using UnityEditor;
using UnityEngine;

public class TowerMono:MonoBehaviour
{
    //========== 防御塔基类
    /// <summary>
    /// 防御塔Id
    /// </summary>
    public int towerId;
    /// <summary>
    /// x轴占格子数
    /// </summary>
    public int sizeX;
    /// <summary>
    /// y轴占格子数
    /// </summary>
    public int sizeY;
    /// <summary>
    /// 防御塔参数
    /// </summary>
    public BaseParamTower param;
}