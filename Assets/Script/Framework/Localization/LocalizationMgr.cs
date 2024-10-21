using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lzengine
{
    public class LocalizationMgr : Singleton<LocalizationMgr>
    {
        private ELocalization mCurLocalization = ELocalization.Zhs;
        public ELocalization CurLocalization
        {
            get
            {
                return mCurLocalization;
            }
        }

        public void Init()
        {
            switch(Application.systemLanguage)
            {
                case SystemLanguage.ChineseSimplified:
                    mCurLocalization = ELocalization.Zhs;
                    break;
                case SystemLanguage.ChineseTraditional:
                    mCurLocalization = ELocalization.Zht;
                    break;
                case SystemLanguage.English:
                    mCurLocalization = ELocalization.English;
                    break;
                default:
                    mCurLocalization = ELocalization.Zhs;
                    break;
            }
        }

        public void SetLocalization(ELocalization local)
        {
            mCurLocalization = local;
        }
    }
}
