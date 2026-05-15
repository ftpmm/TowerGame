namespace lzengine
{
    public static class GameEvent
    {
        #region Game State Events (2000-2099)

        public const int Game_Init = 2000;
        public const int Game_Start = 2001;
        public const int Game_Pause = 2002;
        public const int Game_Resume = 2003;
        public const int Game_Over = 2004;
        public const int Game_Quit = 2005;

        #endregion

        #region Level Events (2100-2199)

        public const int Level_Start = 2100;
        public const int Level_Complete = 2101;
        public const int Level_Failed = 2102;
        public const int Level_Pause = 2103;
        public const int Level_Resume = 2104;
        public const int Level_Restart = 2105;

        public const int Wave_Start = 2150;
        public const int Wave_Complete = 2151;
        public const int Wave_AllComplete = 2152;

        #endregion

        #region Actor Events (2200-2299)

        public const int Actor_Spawn = 2200;
        public const int Actor_Destroy = 2201;
        public const int Actor_Damage = 2202;
        public const int Actor_Death = 2203;
        public const int Actor_Attack = 2204;
        public const int Actor_Skill = 2205;

        #endregion

        #region Player Events (2300-2399)

        public const int Player_Move = 2300;
        public const int Player_Attack = 2301;
        public const int Player_Skill = 2302;
        public const int Player_Skill_Unlock = 2303;
        public const int Player_Select_Target = 2310;

        #endregion

        #region Enemy Events (2400-2499)

        public const int Enemy_Spawn = 2400;
        public const int Enemy_ReachGoal = 2401;
        public const int Enemy_Die = 2402;
        public const int Enemy_Spawn_Wave = 2410;

        #endregion

        #region Tower Events (2500-2599)

        public const int Tower_Place = 2500;
        public const int Tower_Sell = 2501;
        public const int Tower_Upgrade = 2502;
        public const int Tower_Attack = 2510;
        public const int Tower_Skill = 2511;

        #endregion

        #region Resource Events (2600-2699)

        public const int Gold_Change = 2600;
        public const int Diamond_Change = 2601;
        public const int Energy_Change = 2602;
        public const int Score_Change = 2603;
        public const int Level_Star_Change = 2604;

        #endregion

        #region UI Events (2700-2799)

        public const int UI_HUD_Update = 2700;
        public const int UI_Pause_Click = 2701;
        public const int UI_Setting_Click = 2702;
        public const int UI_Skill_Click = 2710;
        public const int UI_Inventory_Update = 2720;

        #endregion

        #region Input Events (2800-2899)

        public const int Input_Touch_Begin = 2800;
        public const int Input_Touch_Move = 2801;
        public const int Input_Touch_End = 2802;
        public const int Input_Click_Ground = 2830;
        public const int Input_Click_Enemy = 2831;
        public const int Input_Click_Tower = 2832;

        #endregion
    }
}
