using System.Collections;
using UnityEngine;

namespace lzengine
{
    public class MainState : BaseState
    {
        public MainState(FSMObject owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            //AssetsManager.Instance.LoadSceneAsync("scene/Game", UnityEngine.SceneManagement.LoadSceneMode.Single, (ret) => {
            //    if(ret)
            //    {
            //        UIManager.Instance.CloseUI<UILogin>();
            //        UIManager.Instance.OpenUIASync<UIMain>((ui) => { });

            //        var actorSys = GameSystemMgr.GetSystem<ActorSystem>();
            //        var player = actorSys.CreateActor<Player>("prefab/player/LilWiz");
            //        player.Position = (new Vector3(0, 4, 0));
            //    }
            //});

            AssetsManager.Instance.LoadSceneAsync("scene/NormalLevel", UnityEngine.SceneManagement.LoadSceneMode.Single, (ret) =>
            {
                if (ret)
                {
                    GameLevelManager.Instance.StartLevel(1);

                    var actorSys = GameSystemMgr.GetSystem<ActorSystem>();
                    var enemy = actorSys.CreateActor<Enemy>("prefab/monster/slime");
                    enemy.Position = (new Vector3(0.4f, 3.22f, 0));

                    UIManager.Instance.OpenUIASync<UINormalLevel>((ui) => {
                        UIManager.Instance.CloseUI<UILogin>();
                    });

                    
                    //var actorSys = GameSystemMgr.GetSystem<ActorSystem>();
                    //var player = actorSys.CreateActor<Player>("prefab/player/LilWiz");
                    //player.Position = (new Vector3(0, 4, 0));
                }
            });
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            return base.OnExecute(deltaTime);
        }

        public override void OnExit()
        {

        }
    }
}