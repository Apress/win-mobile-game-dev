using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Settings
{
    class CSettingsGame : GameEngineCh9.CGameEngineGDIBase
    {

        public CSettingsGame(Form gameForm)
            : base(gameForm)
        {
            Settings.LoadSettings();
        }

    }
}
