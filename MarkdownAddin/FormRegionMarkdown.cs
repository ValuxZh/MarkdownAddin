using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MarkdownAddin
{
    partial class FormRegionMarkdown
    {
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

        private string tmpl = string.Empty;
        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void FormRegionMarkdown_FormRegionShowing(object sender, System.EventArgs e)
        {
            var reader = new StreamReader(@"D:\Workspace\MarkdownAddin\MarkdownAddin\markdown\markdown.html");
            tmpl = reader.ReadToEnd();
            reader.Close();

            reader = new StreamReader(@"D:\Workspace\MarkdownAddin\MarkdownAddin\markdown\bootstrap.min.css");
            tmpl = tmpl.Replace("#TMPL_STYLE_MARKDOWN#", reader.ReadToEnd());
            reader.Close();
            reader = new StreamReader(@"D:\Workspace\MarkdownAddin\MarkdownAddin\markdown\styles\monokai_sublime.css");
            tmpl = tmpl.Replace("#TMPL_STYLE_HIGHLIGHE#", reader.ReadToEnd());
            reader.Close();


            wbMkdBrowser.Navigate("about:blank");
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
            wbMkdBrowser.DocumentText = tmpl.Replace("#TMPL_CONTENT#", html);


            var mail = Globals.ThisAddIn.Application.ActiveInspector().CurrentItem as Outlook.MailItem;
            mail.HTMLBody = wbMkdBrowser.DocumentText;
        }
    }
}
