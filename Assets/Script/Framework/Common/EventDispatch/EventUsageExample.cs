using UnityEngine;

namespace lzengine
{
    public class EventUsageExample
    {
        public class ExampleGameManager : MonoBehaviour
        {
            private void Start()
            {
                EventBus.Instance.SetDebugLog(true);
                
                EventDefine.Game_Init.Register(OnGameInit);
                EventDefine.Actor_Damage.Register<DamageEventData>(OnActorDamage);
                EventDefine.Resource_Change.Register<ResourceChangeData>(OnResourceChange, EventPriority.High);
            }

            private void OnDestroy()
            {
                EventDefine.Game_Init.Unregister(OnGameInit);
                EventDefine.Actor_Damage.Unregister<DamageEventData>(OnActorDamage);
                EventDefine.Resource_Change.Unregister<ResourceChangeData>(OnResourceChange);
            }

            private void Update()
            {
                EventBus.Instance.Update(Time.deltaTime);
            }

            private void OnGameInit()
            {
                LZDebug.Log("Game initialized!");
                EventDefine.Level_Start.Emit();
                EventDefine.Level_Start.EmitDelayed(1f);
            }

            private void OnActorDamage(DamageEventData data)
            {
                LZDebug.Log($"Actor {data.Value1} took {data.Value2} damage");
            }

            private void OnResourceChange(ResourceChangeData data)
            {
                LZDebug.Log($"{data.Value1} changed from {data.Value2} to {data.Value3}");
            }
        }

        public class ExampleEnemy : BaseActor
        {
            private int _hp = 100;

            public void TakeDamage(int damage)
            {
                _hp -= damage;
                EventDefine.Actor_Damage.Emit(new DamageEventData(this, damage));
                
                if (_hp <= 0)
                {
                    Die();
                }
            }

            private void Die()
            {
                EventDefine.Actor_Death.Emit(new ActorEventData(this));
                Destroy();
            }
        }

        public class ExampleUI : MonoBehaviour
        {
            private void Awake()
            {
                EventDefine.UI_OpenPanel.Register<StringEventData>(OnOpenPanel);
            }

            private void OnDestroy()
            {
                EventDefine.UI_OpenPanel.Unregister<StringEventData>(OnOpenPanel);
            }

            private void OnOpenPanel(StringEventData data)
            {
                LZDebug.Log($"Opening panel: {data.Value}");
            }

            public void ShowDamagePopup(BaseActor actor, int damage)
            {
                var pos = actor.Position + Vector3.up * 2f;
                EventDefine.UI_Click.Emit(new Vector3EventData(pos));
            }
        }
    }
}
