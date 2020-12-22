using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelSheetReader
{
    class Item
    {
        private static int _id = 0;

        public Item()
        {
            _id = Id;
            Name = "";
            Desc = "";
            IncrementID();
        }
        public Item(string name, string desc)
        {
            _id = Id;
            Name = name;
            Desc = desc;
            IncrementID();
        }

        //Getters/Setters

        //CHANGE ME LATER, I MUST COLLECT THE LAST KNOWN ID AND INCREMEMNT 1
        public int Id
        {
            get { return _id; }
        }
        public string Name { get; set; }
        public string Desc { get; set; }

        //Methods
        private static void IncrementID()
        {
            _id += 1;
        }
    }
}
