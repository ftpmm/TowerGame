using UnityEngine;

namespace lzengine
{
    public class IntEventData : EventData<int> { public IntEventData(int v) : base(v) { } }
    public class FloatEventData : EventData<float> { public FloatEventData(float v) : base(v) { } }
    public class StringEventData : EventData<string> { public StringEventData(string v) : base(v) { } }
    public class BoolEventData : EventData<bool> { public BoolEventData(bool v) : base(v) { } }
    public class Vector2EventData : EventData<Vector2> { public Vector2EventData(Vector2 v) : base(v) { } }
    public class Vector3EventData : EventData<Vector3> { public Vector3EventData(Vector3 v) : base(v) { } }
    
    public class ActorEventData : EventData<BaseActor>
    {
        public ActorEventData(BaseActor actor) : base(actor) { }
    }
    
    public class DamageEventData : EventData<BaseActor, int>
    {
        public DamageEventData(BaseActor victim, int damage) : base(victim, damage) { }
    }
    
    public class ResourceChangeData : EventData<string, int, int>
    {
        public ResourceChangeData(string resourceType, int oldValue, int newValue) 
            : base(resourceType, oldValue, newValue) { }
    }
    
    public class SceneLoadProgressData : EventData<string, float>
    {
        public SceneLoadProgressData(string sceneName, float progress) 
            : base(sceneName, progress) { }
    }
    
    public class TowerPlaceData : EventData<int, Vector3, int>
    {
        public TowerPlaceData(int towerId, Vector3 position, int level) 
            : base(towerId, position, level) { }
    }
}
