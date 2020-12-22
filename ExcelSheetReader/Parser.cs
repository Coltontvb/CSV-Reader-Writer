using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace ExcelSheetReader
{
    static class Parser
    {
        private static int rowCount = 0;
        private static StreamReader inputFile;
        public static void WriteErr(string errorMsg)
        {
            MessageBox.Show(errorMsg);
        }

        public static List<Item> ParseCSV(string filePath)
        {
            //Set an empty list of items
            List<Item> items = new List<Item>();
            //Set a variable to store each row
            string rowContents;
            //Delimiter for tokenizing
            char[] delimeter = { ',' }; 
            //Open the input file
            
            try
            {
                inputFile = File.OpenText(filePath);

                while (!inputFile.EndOfStream)
                {
                    //Store the row
                    rowContents = inputFile.ReadLine();
                    //Tokenize
                    string[] tokens = rowContents.Split(delimeter);
                    Item item = new Item(tokens[1], tokens[2]);
                    items.Add(item);
                    rowCount++;
                }
                //Reset the row count and return the list of items
                rowCount = 0;
                CloseFile(filePath);
                return items;
            }
            catch (Exception err)
            {
                WriteErr(err.Message);
                return items;
            }
            
        }

        public static void CloseFile(string filePath)
        {
            //If there is file open, close the file
            if (inputFile != null)
            {
                inputFile.Close();
            }
            
        }
        public static void SaveFile(string filePath, List<Item> items)
        {
            string rowContents;
            try
            {
                if (File.Exists(filePath))
                {
                    StreamWriter outputFile = File.CreateText(filePath);
                    foreach (Item item in items)
                    {
                        rowContents = item.Id + "," + item.Name + "," + item.Desc;
                        outputFile.WriteLine(rowContents);
                    }
                    outputFile.Close();
                }
            }
            catch (Exception err)
            {
                Parser.WriteErr(err.Message);
            } 
        }

        public static void AppendEntry(string filePath, Item item)
        {
            string rowContents;
            try
            {               
                if (File.Exists(filePath))
                {
                    StreamWriter outputFile = File.AppendText(filePath);
                    rowContents = $"{item.Id}, {item.Name}, {item.Desc}";
                    outputFile.WriteLine(rowContents);
                    outputFile.Close();
                }
            }
            catch (Exception err)
            {
                Parser.WriteErr(err.Message);
            }
            
        }

        public static string NewFile()
        {
            string csvFilePath;
            try
            {
                
                CommonOpenFileDialog dialogNewFile = new CommonOpenFileDialog();
                dialogNewFile.IsFolderPicker = true;
                dialogNewFile.InitialDirectory = "C:\\Users";
                if (dialogNewFile.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    csvFilePath = dialogNewFile.FileName + "\\newFile.csv";
                    StreamWriter newFile = File.CreateText(csvFilePath);
                    newFile.Close();
                    return csvFilePath;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception err)
            {
                Parser.WriteErr(err.Message);
                return null;
            }
            
        }
    }
}
