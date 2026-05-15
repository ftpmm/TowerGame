namespace lzengine
{
    public static class EventDefine
    {
        #region Framework Events (1000-1999)

        public const int Engine_Init = 1000;
        public const int Engine_Update = 1001;
        public const int Game_Init = 1010;
        public const int Game_Start = 1011;
        public const int Game_Pause = 1012;
        public const int Game_Resume = 1013;
        public const int Game_Quit = 1014;
        
        #endregion

        #region Scene Events (2000-2999)

        public const int Scene_LoadStart = 2000;
        public const int Scene_LoadProgress = 2001;
        public const int Scene_LoadComplete = 2002;
        public const int Scene_Unload = 2003;

        #endregion

        #region UI Events (3000-3999)

        public const int UI_Show = 3000;
        public const int UI_Hide = 3001;
        public const int UI_Click = 3002;
        public const int UI_OpenPanel = 3003;
        public const int UI_ClosePanel = 3004;

        #endregion

        #region Actor Events (4000-4999)

        public const int Actor_Create = 4000;
        public const int Actor_Destroy = 4001;
        public const int Actor_Damage = 4002;
        public const int Actor_Death = 4003;
        public const int Actor_Spawn = 4004;

        #endregion

        #region Input Events (5000-5999)

        public const int Input_KeyDown = 5000;
        public const int Input_KeyUp = 5001;
        public const int Input_ButtonDown = 5002;
        public const int Input_ButtonUp = 5003;
        public const int Input_Touch = 5004;

        #endregion

        #region Gameplay Events (6000-6999)

        public const int Level_Start = 6000;
        public const int Level_Complete = 6001;
        public const int Level_Fail = 6002;
        public const int Enemy_Spawn = 6010;
        public const int Enemy_Die = 6011;
        public const int Tower_Place = 6020;
        public const int Tower_Upgrade = 6021;
        public const int Tower_Sell = 6022;
        public const int Resource_Change = 6030;
        public const int Wave_Start = 6040;
        public const int Wave_Complete = 6041;

        #endregion
    }
}
