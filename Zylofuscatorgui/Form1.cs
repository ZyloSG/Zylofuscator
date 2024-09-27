using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Zylofuscatorgui.Protection;
using Zylofuscatorgui.Protection.Crasher;
using Zylofuscatorgui.Protection.CtrlFlow;
using Zylofuscatorgui.Protection.Int;
using Zylofuscatorgui.Protection.Other;
using Zylofuscatorgui.Protection.Proxy;
using Zylofuscatorgui.Protection.Renamer;
using Zylofuscatorgui.Protection.String;

namespace Zylofuscatorgui
{
    public partial class Form1 : Form
    {
        public static MethodDef Init;
        public static MethodDef Init2;
        private readonly List<Action> _func = new List<Action>();
        private string _directoryName = string.Empty;
        private ModuleDefMD Md { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gunaAnimateWindow1.Start();
            AssemblyTab.Visible = true;
            ProtectionsTab.Visible = false;
            renameOptionsTab.Visible = false;
        }

        private void gunaSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            // Nothing
        }



        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AssemblyTab.Visible = true;
            ProtectionsTab.Visible = false;
            renameOptionsTab.Visible = false;
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AssemblyTab.Visible = false;
            ProtectionsTab.Visible = true;
            renameOptionsTab.Visible = false;
        }

        private void ProtectionsTab_Paint(object sender, PaintEventArgs e)
        {
            // Nothing
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Md = ModuleDefMD.Load(inputFile.Text);
            foreach (var func in _func) func();
            var text2 = Path.GetDirectoryName(inputFile.Text);
            if (text2 != null && !text2.EndsWith("\\"))
                text2 += "\\";
            var path =
                $"{text2}{Path.GetFileNameWithoutExtension(inputFile.Text)}_zylofuscated{Path.GetExtension(inputFile.Text)}";

            var opts = new ModuleWriterOptions(Md)
            {
                Logger = DummyLogger.NoThrowInstance
            };
            RunProtection();
            Md.Write(path, opts);
        }

        private void proxySwitch_CheckedChanged(object sender, EventArgs e)
        {
            // Nothing
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // Nothing  
        }

        private void RunProtection()
        {
            {
                if (classSwitch.Checked) RenamerPhase.ExecuteClassRenaming(Md);
                if (fieldsSwitch.Checked) RenamerPhase.ExecuteFieldRenaming(Md);
                if (methodsSwitch.Checked) RenamerPhase.ExecuteMethodRenaming(Md);
                if (cflowSwitch.Checked) ControlFlowObfuscation.Execute(Md);
                if (mutationSwitch.Checked) IntV2.Execute(Md);
                if (constantsSwitch.Checked) StringEncryption.Execute(Md);
                if (proxySwitch.Checked) ProxyInt.Execute(Md); ProxyString.Execute(Md);
                if (hidemethodsSwitch.Checked) hideMethods.Execute(Md);
                if (dnspyCrasherSwitch.Checked) CrasherPhase.Execute(Md);
                Watermark.Execute(Md);
                //   ProxyMeth.Execute(Md);
            }

        }

        private void showRenameOptions_Click(object sender, EventArgs e)
        {
            AssemblyTab.Visible = false;
            ProtectionsTab.Visible = false;
            renameOptionsTab.Visible = true;
        }

        private void gunaLabel7_Click(object sender, EventArgs e)
        {
            // Nothing
        }

        private void gunaLabel21_Click(object sender, EventArgs e)
        {
            // Nothing
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenamerPhase phase = new RenamerPhase();
            if (guna2ComboBox1.SelectedIndex == -1) return;
            else if (guna2ComboBox1.SelectedIndex == 0) phase.selectedRenameMode = RenamerPhase.RenameMode.Ascii;
            else if (guna2ComboBox1.SelectedIndex == 1) phase.selectedRenameMode = RenamerPhase.RenameMode.Chinese;
            else if (guna2ComboBox1.SelectedIndex == 2) phase.selectedRenameMode = RenamerPhase.RenameMode.Spam;

        }
    }
}