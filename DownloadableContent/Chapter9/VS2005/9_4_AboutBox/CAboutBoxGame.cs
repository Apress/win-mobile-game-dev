using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AboutBox
{
    class CAboutBoxGame : GameEngineCh9.CGameEngineGDIBase
    {

        public CAboutBoxGame(Form gameForm)
            : base(gameForm)
        {
            Settings.LoadSettings();
        }

    }
}
