using System.IO;
using System.Text;
using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Windows.Forms;
using RtfHelper.Formatter;

namespace Kbg.NppPluginNET
{
    class Main
    {
        public static string Name = "RtfHelper";
        private static RtfFormatter rtfFormatter = new RtfFormatter();

        public static void OnNotification(ScNotification notification)
        {
        }

        internal static void CommandMenuInit()
        {
            string iniFilePath = null;

            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, Name + ".ini");

            SetCommands();
        }

        private static void SetCommands()
        {
            PluginBase.SetCommand(0, "Format RTF", FormatRtf, new ShortcutKey(true, true, true, Keys.V));
        }

        internal static void SetToolBarIcon()
        {
        }

        internal static void PluginCleanUp()
        {
        }

        private static void FormatRtf()
        {
            IntPtr currentScint = PluginBase.GetCurrentScintilla();
            ScintillaGateway scintillaGateway = new ScintillaGateway(currentScint);

            string allText = scintillaGateway.GetAllText();

            if (!IsTextRtf(allText))
            {
                return;
            }

            string newText = rtfFormatter.GetFormattedText(allText);

            if (allText != newText)
            {
                scintillaGateway.SetText(newText);
            }
        }

        private static bool IsTextRtf(string allText)
        {
            return allText.StartsWith(@"{\rtf");
        }
    }
}