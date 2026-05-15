namespace lzengine
{
    public static class FrameworkEvent
    {
        #region Engine Events (1000-1099)

        public const int Engine_Init = 1000;
        public const int Engine_Update = 1001;
        public const int Engine_Destroy = 1002;

        #endregion

        #region HotUpdate Events (1100-1199)

        public const int HotUpdate_Start = 1100;
        public const int HotUpdate_Progress = 1101;
        public const int HotUpdate_Complete = 1102;
        public const int HotUpdate_Failed = 1103;

        public const int Bundle_Update_Start = 1150;
        public const int Bundle_Update_Progress = 1151;
        public const int Bundle_Update_Complete = 1152;
        public const int Bundle_Update_Failed = 1153;

        #endregion

        #region Resource Events (1200-1299)

        public const int Resource_LoadStart = 1200;
        public const int Resource_LoadProgress = 1201;
        public const int Resource_LoadComplete = 1202;
        public const int Resource_LoadFailed = 1203;

        #endregion

        #region Localization Events (1300-1399)

        public const int Localization_LoadStart = 1300;
        public const int Localization_LoadComplete = 1301;
        public const int Localization_Change = 1302;

        #endregion

        #region Scene Events (1400-1499)

        public const int Scene_LoadStart = 1400;
        public const int Scene_LoadProgress = 1401;
        public const int Scene_LoadComplete = 1402;
        public const int Scene_Unload = 1403;

        #endregion

        #region UI Events (1500-1599)

        public const int UI_PanelOpen = 1500;
        public const int UI_PanelClose = 1501;
        public const int UI_PanelDestroy = 1502;
        public const int UI_LoadingShow = 1510;
        public const int UI_LoadingHide = 1511;
        public const int UI_LoadingProgress = 1512;

        #endregion
    }
}
