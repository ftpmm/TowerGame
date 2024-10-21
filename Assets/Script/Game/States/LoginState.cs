using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class LoginState : BaseState
    {
        public LoginState(FSMObject owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            EventManager.Instance.AddEventListener(GameEventDefine.Enter_Game, OnEnterMain);

            TestEventManager.Instance.AddEventListener<TestStruct>(130, OnTestCB);
            TestEventManager.Instance.AddEventListener<TestClass>(140, OnTestCC);
            TestEventManager.Instance.AddEventListener<string>(150, OnTestString);
            UIManager.Instance.OpenUIASync<UILogin>((ui) => {
                EventManager.Instance.Dispatch<bool>(LZEngineEventDefine.Game_Init, true);
            });
        }

        private void OnEnterMain()
        {
            mRunState = EStateExecute.Finish;
        }

        public override EStateExecute OnExecute(float deltaTime)
        {
            return base.OnExecute(deltaTime);
        }

        public override void OnExit()
        {
            TestStruct t = new TestStruct() { a = "test", b = 10.0f, c = 0 };
            OnTestCB(t);
             TestEventManager.Instance.Dispatch<TestStruct>(130, new TestStruct() { a= "test222", b=10.0f, c = 0});
             TestEventManager.Instance.Dispatch<TestClass>(140, new TestClass());
            TestEventManager.Instance.Dispatch<string>(150, "tfafdafda");
            EventManager.Instance.RemoveEventListener(GameEventDefine.Enter_Game, OnEnterMain);
            mMachine.TransState<MainState>();
        }

        public void OnTestCB(TestStruct testStruct)
        {
            LZDebug.Log("fdafdafdfdTestStruct t=" + testStruct.a);
        }

        public void OnTestCC(TestClass testClass)
        {
            LZDebug.Log("fdafdafdTestClass t=" + testClass);
        }

        public void OnTestString(string testString)
        {
            LZDebug.Log("TestString Testttesfdstr = " + testString);
        }
    }

    public struct TestStruct
    {
        public string a;
        public float b;
        public int c;
    }

    public class TestClass
    {
        public TestStruct t;
        public string a;
        public delegate void TestDelegate();
    }
}
