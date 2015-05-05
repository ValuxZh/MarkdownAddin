using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using MarkdownAddin.Properties;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MarkdownAddin
{
    partial class FormRegionMarkdown
    {
        protected const string TMPL_CONTENT = "#TMPL_CONTENT#";
        protected const string TMPL_STYLE = "#TMPL_STYLE#";

        protected string templateContent = string.Empty;


        #region Form Region Factory

        [Microsoft.Office.Tools.Outlook.FormRegionMessageClass(Microsoft.Office.Tools.Outlook.FormRegionMessageClassAttribute.Note)]
        [Microsoft.Office.Tools.Outlook.FormRegionName("MarkdownAddin.FormRegionMarkdown")]
        public partial class FormRegionMarkdownFactory
        {
            // Occurs before the form region is initialized.
            // To prevent the form region from appearing, set e.Cancel to true.
            // Use e.OutlookItem to get a reference to the current Outlook item.
            private void FormRegionMarkdownFactory_FormRegionInitializing(object sender, Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs e)
            {

            }
        }

        #endregion

        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void FormRegionMarkdown_FormRegionShowing(object sender, System.EventArgs e)
        {
            templateContent = Resources.Markdown.Replace(TMPL_STYLE, Resources.Github);

            tscbxEditorFont.Items.Clear();
            var fonts = new InstalledFontCollection();
            foreach (var font in fonts.Families)
                tscbxEditorFont.Items.Add(font.Name);

            tscbxEditorSize.SelectedItem = "11";
            tscbxEditorFont.SelectedItem = "微软雅黑";
            

            tscbxEditorFont.SelectedIndexChanged += tscbxEditorFont_SelectedIndexChanged;
            tscbxEditorSize.SelectedIndexChanged += tscbxEditorFont_SelectedIndexChanged;

        }

        // Occurs when the form region is closed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void FormRegionMarkdown_FormRegionClosed(object sender, System.EventArgs e)
        {
        }

        private void rtbMkdEditor_TextChanged(object sender, EventArgs e)
        {
            var html = CommonMark.CommonMarkConverter.Convert(rtbMkdEditor.Text);
            wbMkdPreview.DocumentText = templateContent.Replace(TMPL_CONTENT, html);

            var mail = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as Outlook.MailItem;
            mail.HTMLBody = wbMkdPreview.DocumentText;

        }

        private void tsbtnPreview_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;

        }

        private void tscbxEditorFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            float size = 11.0F;
            if (tscbxEditorSize.SelectedItem != null)
                float.TryParse(tscbxEditorSize.SelectedItem.ToString(), out size);
            rtbMkdEditor.Font = new System.Drawing.Font(tscbxEditorFont.SelectedItem.ToString(), size);
        }

        private void tsbtnEditorImg_Click(object sender, EventArgs e)
        {
            var dialog = new ImgWnd();
            dialog.Show();
        }
    }
}
