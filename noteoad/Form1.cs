using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace noteoad
{
    public partial class Form1 : Form
    {
        private string currentFilePath = string.Empty;
        private bool isDirty = false; // آیا محتوا تغییر کرده؟
        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isDirty)
            {
                var result = MessageBox.Show("Do you want to save changes?", "save changes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            txtEditor.Clear();
            currentFilePath = string.Empty;
            isDirty = false;
            UpdateTitle();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isDirty)
            {
                var result = MessageBox.Show("Do you want to save changes?", "save changes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    if (isDirty) return; // اگر ذخیره نشد (مثلاً cancel شد)، لغو کن
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                ofd.Title = "opening file";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        txtEditor.Text = File.ReadAllText(ofd.FileName);
                        currentFilePath = ofd.FileName;
                        isDirty = false;
                        UpdateTitle();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($" error opening file:\n{ex.Message}", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileAs();
            }
            else
            {
                try
                {
                    File.WriteAllText(currentFilePath, txtEditor.Text);
                    isDirty = false;
                    UpdateTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"error saving file:\n{ex.Message}", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SaveFileAs()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                sfd.Title = " saving file";
                sfd.FileName = "Untitled.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(sfd.FileName, txtEditor.Text);
                        currentFilePath = sfd.FileName;
                        isDirty = false;
                        UpdateTitle();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.SelectAll();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEditor.WordWrap = wordWrapToolStripMenuItem.Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Simple Notepad\n version 1.0\nMade withC# و Windows Forms", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtEditor_TextChanged(object sender, EventArgs e)
        {
            isDirty = true;
            UpdateTitle();
        }
        private void UpdateTitle()
        {
            string docName = string.IsNullOrEmpty(currentFilePath) ? "Untitled" : Path.GetFileName(currentFilePath);
            string star = isDirty ? "*" : "";
            this.Text = $"Simple Notepad - {docName}{star}";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isDirty)
            {
                var result = MessageBox.Show("Do you want to save changes?", "exit",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    if (isDirty) // اگر ذخیره نشد (مثلاً در حین SaveAs کنسل شد)
                        e.Cancel = true;
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.N:
                    newToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.O:
                    openToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.S:
                    saveToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.Shift | Keys.S:
                    saveAsToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.X:
                    cutToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.C:
                    copyToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.V:
                    pasteToolStripMenuItem.PerformClick();
                    return true;
                case Keys.Control | Keys.A:
                    selectAllToolStripMenuItem.PerformClick();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
    
