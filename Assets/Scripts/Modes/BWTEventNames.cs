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
    }
}
