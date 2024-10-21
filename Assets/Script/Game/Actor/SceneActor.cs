using System;
using System.Collections.Generic;
using UnityEngine;

namespace lzengine
{
    public class SceneActor:BaseActor
    {
        protected Vector3 _position;
        public virtual Vector3 Position 
        {
            get { return _position; }
            set { _position = value; }
        }

        protected Vector3 _forward = Vector3.one;
        public virtual Vector3 Forward 
        { 
            get { return _forward; } 
            set { _forward = value; }
        }

        public SceneActor():base()
        {
            AddState<AddSceneState>(90);
        }

        public void AddScene()
        {
            var sceneSys = GameSystemMgr.GetSystem<SceneSystem>();
            sceneSys.AddActor(this);
        }
    }
}
