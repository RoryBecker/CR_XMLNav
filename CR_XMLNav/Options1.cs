using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;

namespace CR_XMLNav
{
    [UserLevel(UserLevel.Advanced)]
    public partial class Options1 : OptionsPage
    {
        // DXCore-generated code...
        #region Initialize
        protected override void Initialize()
        {
            base.Initialize();

            //
            // TODO: Add your initialization code here.
            //
        }
        #endregion

        #region GetCategory
        public static string GetCategory()
        {
            return @"Editor\Navigation";
        }
        #endregion
        #region GetPageName
        public static string GetPageName()
        {
            return @"XML Nav";
        }
        #endregion
        public static Settings LoadSettings(DecoupledStorage storage)
        {
            Settings settings = new Settings();
            settings.AttributeNames = storage.ReadString("XMLNav", "AttributeNames","id|ref");
            return settings;
        }

        public static void SaveSettings(DecoupledStorage storage, Settings settings)
        {
            storage.WriteString("XMLNav", "AttributeNames", settings.AttributeNames);
        }

        private void Options1_RestoreDefaults(object sender, OptionsPageEventArgs ea)
        {
            var Settings = new Settings();
            txtAttributeNames.Text = Settings.AttributeNames;
        }

        private void Options1_PreparePage(object sender, OptionsPageStorageEventArgs ea)
        {
            var Settings = LoadSettings(ea.Storage);
            txtAttributeNames.Text = Settings.AttributeNames;
        }

        private void Options1_CommitChanges(object sender, CommitChangesEventArgs ea)
        {
            var settings = new Settings();
            settings.AttributeNames = txtAttributeNames.Text;
            SaveSettings(ea.Storage, settings);

        }



    }
}