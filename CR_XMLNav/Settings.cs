using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;

namespace CR_XMLNav
{
    public class Settings
    {
        public Settings()
        {
            AttributeNames = "id|ref";
        }
        public string AttributeNames { get; set; }
    }
}
