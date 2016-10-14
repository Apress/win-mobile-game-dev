using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace HighScores
{
    class CHighScoresGame : GameEngineCh9.CGameEngineGDIBase
    {

        public CHighScoresGame(Form gameForm)
            : base(gameForm)
        {
            Settings.LoadSettings();
        }

    }
}
