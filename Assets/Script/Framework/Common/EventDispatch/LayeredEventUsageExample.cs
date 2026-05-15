using UnityEngine;

namespace lzengine
{
    public class LayeredEventUsageExample
    {
        public class FrameworkEventExample : MonoBehaviour
        {
            private void Start()
            {
                LayeredEventBus.Instance.SetDebugLog(true);

                FrameworkEvent.Engine_Init.Register(OnEngineInit, EventLayer.Framework);
                FrameworkEvent.HotUpdate_Complete.Register(OnHotUpdateComplete, EventLayer.Framework);
                FrameworkEvent.Scene_LoadComplete.Register<SceneLoadProgressData>(OnSceneLoaded, EventLayer.Framework);
            }

            private void OnDestroy()
            {
                FrameworkEvent.Engine_Init.Unregister(OnEngineInit, EventLayer.Framework);
                FrameworkEvent.HotUpdate_Complete.Unregister(OnHotUpdateComplete, EventLayer.Framework);
                FrameworkEvent.Scene_LoadComplete.Unregister<SceneLoadProgressData>(OnSceneLoaded, EventLayer.Framework);
            }

            private void OnEngineInit()
            {
                LZDebug.Log("[Framework] Engine initialized");
                
                FrameworkEvent.HotUpdate_Start.Emit(EventLayer.Framework);
            }

            private void OnHotUpdateComplete()
            {
                LZDebug.Log("[Framework] Hot update complete, loading scene...");
                
                var sceneData = new SceneLoadProgressData("MainScene", 0f);
                FrameworkEvent.Scene_LoadStart.Emit(sceneData, EventLayer.Framework);
            }

            private void OnSceneLoaded(SceneLoadProgressData data)
            {
                LZDebug.Log($"[Framework] Scene {data.Value1} loaded!");
                
                GameEvent.Game_Start.Emit(EventLayer.Game);
            }
        }

        public class GameEventExample : MonoBehaviour
        {
            private void Start()
            {
                GameEvent.Game_Start.Register(OnGameStart, EventLayer.Game);
                GameEvent.Level_Start.Register(OnLevelStart, EventLayer.Game);
                GameEvent.Actor_Damage.Register<DamageEventData>(OnActorDamage, EventLayer.Game);
                GameEvent.Gold_Change.Register<IntEventData>(OnGoldChange, EventLayer.Game);
            }

            private void OnDestroy()
            {
                GameEvent.Game_Start.Unregister(OnGameStart, EventLayer.Game);
                GameEvent.Level_Start.Unregister(OnLevelStart, EventLayer.Game);
                GameEvent.Actor_Damage.Unregister<DamageEventData>(OnActorDamage, EventLayer.Game);
                GameEvent.Gold_Change.Unregister<IntEventData>(OnGoldChange, EventLayer.Game);
            }

            private void OnGameStart()
            {
                LZDebug.Log("[Game] Game started!");
                
                GameEvent.Level_Start.Emit(EventLayer.Game);
                GameEvent.Gold_Change.Emit(new IntEventData(100), EventLayer.Game);
            }

            private void OnLevelStart()
            {
                LZDebug.Log("[Game] Level started!");
            }

            private void OnActorDamage(DamageEventData data)
            {
                LZDebug.Log($"[Game] Actor damaged: {data.Value1.uuid}, Damage: {data.Value2}");
            }

            private void OnGoldChange(IntEventData data)
            {
                LZDebug.Log($"[Game] Gold changed: {data.Value}");
            }
        }

        public class CrossLayerEventExample : MonoBehaviour
        {
            private void Start()
            {
                FrameworkEvent.UI_PanelOpen.Register<StringEventData>(OnUIPanelOpen, EventLayer.Framework);
                
                GameEvent.UI_HUD_Update.Register(OnHUDUpdate, EventLayer.Game);
                
                GameEvent.Level_Start.Register(OnLevelStart, EventLayer.Game);
            }

            private void OnDestroy()
            {
                FrameworkEvent.UI_PanelOpen.Unregister<StringEventData>(OnUIPanelOpen, EventLayer.Framework);
                GameEvent.UI_HUD_Update.Unregister(OnHUDUpdate, EventLayer.Game);
                GameEvent.Level_Start.Unregister(OnLevelStart, EventLayer.Game);
            }

            private void OnUIPanelOpen(StringEventData data)
            {
                LZDebug.Log($"[Framework] UI Panel opened: {data.Value}");
            }

            private void OnHUDUpdate()
            {
                LZDebug.Log("[Game] HUD updated");
            }

            private void OnLevelStart()
            {
                LZDebug.Log("[Game] Level start - opening HUD");
                
                FrameworkEvent.UI_PanelOpen.Emit(new StringEventData("HUD"), EventLayer.Framework);
                
                GameEvent.UI_HUD_Update.Emit(EventLayer.Game);
            }
        }

        public class DelayEventExample : MonoBehaviour
        {
            private void Start()
            {
                GameEvent.Level_Complete.Register(OnLevelComplete, EventLayer.Game);
            }

            private void OnDestroy()
            {
                GameEvent.Level_Complete.Unregister(OnLevelComplete, EventLayer.Game);
            }

            private void OnLevelComplete()
            {
                LZDebug.Log("[Game] Level complete! Showing result after 2 seconds...");
                
                GameEvent.Level_Complete.EmitDelayed(2f, EventLayer.Game);
            }
        }
    }
}
