using lzengine;
using System.Collections;
using UnityEngine;

public class EnemyAnimeMono : MonoBehaviour
{
    public BaseParamEnemy param;

    public AnimationCurve idleCurve;
    private float idleAnimeTime = 0;
    public float idleAnimeSpeed = 0.3f;

    public AnimationCurve walkCurve;
    private float walkAnimeTime = 0;
    public float walkAnimeSpeed = 0.2f;
    public float walkAngle = 10.0f;

    private Transform walkTrans;

    private void Start()
    {
        walkTrans = transform.Find("sp");
    }

    public void Update()
    {
        float deltaTime = Time.deltaTime;
        UpdateIdleAnime(deltaTime);
        UpdateWalkAnime(deltaTime);
    }

    private void UpdateIdleAnime(float deltaTime)
    {
        idleAnimeTime += deltaTime * idleAnimeSpeed;
        if (idleAnimeTime >= 1)
        {
            idleAnimeSpeed *= -1;
        }
        else if (idleAnimeTime <= 0)
        {
            idleAnimeSpeed *= -1;
        }
        Vector3 newScale = transform.localScale;
        newScale.y = idleCurve.Evaluate(idleAnimeTime);
        transform.localScale = newScale;
    }

    private void UpdateWalkAnime(float deltaTime)
    {
        walkAnimeTime += deltaTime * walkAnimeSpeed;
        //if (walkAnimeTime >= 1)
        //{
        //    walkAnimeSpeed *= -1;
        //}
        //else if (walkAnimeTime <= 0)
        //{
        //    walkAnimeSpeed *= -1;
        //}
        Vector3 newScale = Vector3.one;
        newScale.z = Mathf.Sin(walkAnimeTime) * walkAngle;
        walkTrans.localEulerAngles = newScale;
    }
}