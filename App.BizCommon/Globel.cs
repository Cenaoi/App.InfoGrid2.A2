using System;
using System.Collections.Generic;
using System.Text;
using EC5.SystemBoard;
using HWQ.Entity.Decipher.LightDecipher;

namespace App.BizCommon
{
    public class Globel:EcApplication
    {

        void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            DbDecipher decipher = this.Context.Items["DbDecipher"] as DbDecipher;

            if (decipher != null)
            {
                decipher.Dispose();
            }
        }


    }
}
