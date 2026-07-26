using Multimorphic.P3App.Modes;

namespace Francotronics.BWT.Modes
{
    /// <summary>
    /// Named events used in M2M and M2G events.
    /// </summary>
    public static class BWTEventNames
    {
        // Framework events
        public const string EnableFlippers = "Evt_EnableFlippers";
        public const string EnableBumpers =  "Evt_EnableBumpers";
        public const string BallStarted =  "Evt_BallStarted";
        public const string BallEnded =  "Evt_BallEnded";
        public const string ChangeGameState =  "Evt_ChangeGameStates";
        public const string EnableBallSearch =  "Evt_EnableBallSearch";
        public const string SideTargetHit = "Evt_SideTargetHit";
        public const string LeftSlingHit = "Evt_LeftSlingHit";
        public const string RightSlingHit = "Evt_RightSlingHit";

        // App events
        public const string BWTHomeSetup = "Evt_BWTHomeSetup";
        public const string InitialLaunch = "Evt_InitialLaunch";
        public const string BallDrained = "Evt_BallDrained";
        public const string SpawnBird = "Evt_SpawnBird";
        public const string SpawnBirdFromIndex = "Evt_SpawnBirdFromIndex";
        public const string BirdHit = "Evt_BirdHit";
        public const string MainTimerExpired = "Evt_MainTimerExpired";
        public const string InlaneRightLower =  "Evt_InlaneRightLower";
        public const string InlaneLeftLower =  "Evt_InlaneLeftLower";
        public const string Shake =  "Evt_Shake";
        public const string AnimatePhotos = "Evt_AnimatePhotos";
        public const string ShowBirdBook = "Evt_ShowBirdBook";
        public const string PhotoAnimationComplete = "Evt_PhotoAnimationComplete";
    }
}
