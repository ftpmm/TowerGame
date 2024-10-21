using System.Collections;
using System;

[Serializable]
public class BaseParamEnemy
{
    /// <summary>
    /// 血量
    /// </summary>
    public int hp;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// 移动速度加成百分比
    /// </summary>
    public float moveSpeedPercent;
    /// <summary>
    /// 基础攻击力
    /// </summary>
    public float atk;
    /// <summary>
    /// 攻击加成百分比
    /// </summary>
    public float atkPercent;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float atkRange;
    /// <summary>
    /// 攻击速度
    /// </summary>
    public float atkSpeed;
    /// <summary>
    /// 暴击触发百分比
    /// </summary>
    public float criPercent;
    /// <summary>
    /// 暴击伤害百分比
    /// </summary>
    public float criDmgPercent;
}