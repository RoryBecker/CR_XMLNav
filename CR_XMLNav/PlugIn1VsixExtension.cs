using System.ComponentModel.Composition;
using DevExpress.CodeRush.Common;

namespace CR_XMLNav
{
    [Export(typeof(IVsixPluginExtension))]
    public class CR_XMLNavExtension : IVsixPluginExtension { }
}