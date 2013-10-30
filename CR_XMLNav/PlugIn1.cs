using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.PlugInCore;
using DevExpress.CodeRush.StructuralParser;

namespace CR_XMLNav
{
    public partial class PlugIn1 : StandardPlugIn
    {
        private List<string> _Attributes;
        // DXCore-generated code...
        #region InitializePlugIn
        public override void InitializePlugIn()
        {
            base.InitializePlugIn();
            registerXMLRefSearch();
            //
            // TODO: Add your initialization code here.
            //
        }
        #endregion
        #region FinalizePlugIn
        public override void FinalizePlugIn()
        {
            //
            // TODO: Add your finalization code here.
            //

            base.FinalizePlugIn();
        }
        #endregion


        private void registerXMLRefSearch()
        {
            var XMLRefSearch = new DevExpress.CodeRush.Core.SearchProvider();
            ((System.ComponentModel.ISupportInitialize)(XMLRefSearch)).BeginInit();
            XMLRefSearch.Description = "XMLRefSearch";
            XMLRefSearch.ProviderName = "XMLRefSearch"; // Needs to be Unique
            XMLRefSearch.Register = true;
            XMLRefSearch.UseForNavigation = true;
            XMLRefSearch.CheckAvailability += XMLRefSearch_CheckAvailability;
            XMLRefSearch.SearchReferences += XMLRefSearch_SearchReferences;
            ((System.ComponentModel.ISupportInitialize)(XMLRefSearch)).EndInit();
        }
        private void XMLRefSearch_CheckAvailability(object sender, CheckSearchAvailabilityEventArgs ea)
        {
            // Get Active LanguageElement
            var Element = CodeRush.Documents.ActiveTextDocument.GetNodeAt(CodeRush.Caret.Line, CodeRush.Caret.Offset);
            if (Element == null)
                return;
            if (!(Element is XmlAttribute))
                return;
            // Allow search to start if Attribute

            string AttributeName = ((XmlAttribute)Element).Name;

            List<List<string>> AttributeCollections = new List<List<string>>();
            AttributeCollections.Add("id|ref".Split('|').ToList());
            AttributeCollections.Add("id1|ref1".Split('|').ToList());
            _Attributes = null;
            foreach (List<string> attributes in AttributeCollections)
            {
                if (attributes.Contains(AttributeName))
                {
                    _Attributes = attributes;
                }
            }
            ea.Available = _Attributes != null;
        }
        private void XMLRefSearch_SearchReferences(Object Sender, DevExpress.CodeRush.Core.SearchEventArgs ea)
        {
            // Store Value of initial XmlAttribute
            TextDocument activeDoc = CodeRush.Documents.ActiveTextDocument;
            string StartValue = ((XmlAttribute)activeDoc.GetNodeAt(CodeRush.Caret.Line, CodeRush.Caret.Offset)).Value;

            // Iterate LanguageElements in solution
            SolutionElement activeSolution = CodeRush.Source.ActiveSolution;
            foreach (ProjectElement project in activeSolution.AllProjects)
            {
                foreach (SourceFile sourceFile in project.AllFiles)
                {
                    SourceFile activeFile = CodeRush.Source.ActiveSourceFile;
                    ElementEnumerable Enumerator = new ElementEnumerable(sourceFile, new XMLAttributeFilter(StartValue, _Attributes), true);
                    foreach (XmlAttribute CurrentAttribute in Enumerator)
                    {
                        ea.AddRange(new FileSourceRange(CurrentAttribute.FileNode, CurrentAttribute.ValueRange));
                    }
                }
            }
        }
    }
    public class XMLAttributeFilter : IElementFilter
    {
        private readonly string _startValue;
        private readonly List<string> _attributes;
        public XMLAttributeFilter(string StartValue, List<string> Attributes)
        {
            _attributes = Attributes;
            _startValue = StartValue;
        }
        public bool Apply(IElement element)
        {
            // Skip if null 
            if (element == null)
                return false;

            // Skip if not XmlAttribute
            if (!(element is XmlAttribute))
                return false;

            XmlAttribute CurrentAttribute = (XmlAttribute)element;

            // Skip attribute if it doesn't have the correct name.
            if (!(_attributes.Contains(CurrentAttribute.Name)))
                return false;

            // Skip the attribute if it doesn't have the same value as start point.
            if (CurrentAttribute.Value != _startValue)
                return false;

            return true;
        }
        public bool SkipChildren(IElement element)
        {
            return false;
        }
    }
}