using UnityEditor;
using UnityEngine;

public class TowerBase:MonoBehaviour
{
    //========== 防御塔基类
    
    /// <summary>
    /// x轴占格子数
    /// </summary>
    public int sizeX;
    /// <summary>
    /// y轴占格子数
    /// </summary>
    public int sizeY;

    /// <summary>
    /// 所在的格子x
    /// </summary>
    public int posX;
    /// <summary>
    /// 所在的格子y
    /// </summary>
    public int posY;
    /// <summary>
    /// 防御塔参数
    /// </summary>
    public BaseParamTower param;
}