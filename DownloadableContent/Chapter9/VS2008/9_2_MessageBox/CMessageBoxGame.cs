using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MessageBox
{
    class CMessageBoxGame : GameEngineCh9.CGameEngineGDIBase
    {

        public CMessageBoxGame(Form gameForm)
            : base(gameForm)
        {
            Settings.LoadSettings();
        }

    }
}
