using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ExcelSheetReader
{
    public partial class Form1 : Form
    {
        //Hold the value of the files path
        private string csvFilePath = String.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        List<Item> items = new List<Item>();

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check if the openDialog is == OK
            bool isFileOpen = dialogOpenCSV.ShowDialog() == DialogResult.OK ? true : false;
            if (!isFileOpen)
            {
                Parser.WriteErr("Error on loading file");
            }
            else
            {   
                try
                {
                    lblCurrentFile.Text = dialogOpenCSV.FileName;
                    csvFilePath = dialogOpenCSV.FileName;
                    items = Parser.ParseCSV(csvFilePath);
                    var bindingList = new BindingList<Item>(items);
                    var source = new BindingSource(bindingList, null);
                    gridViewItems.DataSource = source;
                }
                catch (Exception err)
                {
                    Parser.WriteErr(err.Message);
                }
            }
        }
        //Add a new item to the list and rebind the datasource
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(csvFilePath))
                {
                    //Create a new item
                    Item item = new Item(txtBoxName.Text, txtBoxDescription.Text);
                    //Pass it to the database append function
                    Parser.AppendEntry(csvFilePath, item);
                    //Add to the list items
                    items.Add(item);
                    var bindingList = new BindingList<Item>(items);
                    var source = new BindingSource(bindingList, null);
                    gridViewItems.DataSource = source;
                    //Clear Inputs
                    txtBoxID.Text = items.Count.ToString();
                    txtBoxName.Text = String.Empty;
                    txtBoxDescription.Text = String.Empty;
                }
            }
            catch (Exception err)
            {
                Parser.WriteErr(err.Message);
            }
            
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isFilePathValid = dialogSaveFile.ShowDialog() == DialogResult.OK ? true : false;
            if (!isFilePathValid)
            {
                Parser.WriteErr("Unable to save here");
            }
            else
            {
                try
                {
                    Parser.SaveFile(csvFilePath, items);
                }
                catch (Exception err)
                {
                    Parser.WriteErr(err.Message);
                }
            }
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Close the current file with the toolbar
            Parser.CloseFile(csvFilePath);
            csvFilePath = String.Empty;
            //Clear the items list and rebind the list
            items.Clear();
            lblCurrentFile.Text = String.Empty;
            var bindingList = new BindingList<Item>(items);
            var source = new BindingSource(bindingList, null);
            gridViewItems.DataSource = source;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Ensure the file closes on any form closing event
            Parser.CloseFile(csvFilePath);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //No need to close an open file, ass the FormClosing Event handles this
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If a file is already open, close it
            if (File.Exists(csvFilePath))
            {
                Parser.CloseFile(csvFilePath);
            }
            //Reset the csv file path to the new file rebind datasource
            csvFilePath = Parser.NewFile();
            items = Parser.ParseCSV(csvFilePath);
            var bindingList = new BindingList<Item>(items);
            var source = new BindingSource(bindingList, null);
            gridViewItems.DataSource = source;
        }
    }
}
