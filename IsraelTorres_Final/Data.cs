using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Name: Israel Torres
 * Class: Visual Frameworks - Online (MDV1830-O)
 * Term: C201905 01
 * Code Exercise: Synthesis
 * Number: Final
 */

namespace IsraelTorres_Final
{
    public class Data
    {
        private int _dvdId;
        private string _title;
        private string _genre;
        private decimal _year;
        private decimal _price;

        public int DVDID
        {
            get { return _dvdId; }
            set { _dvdId = value; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; }
        }
        public decimal Year
        {
            get { return _year; }
            set { _year = value; }
        }
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public override string ToString()
        {
            string item = "Title: " +Title;
            return item;
        }
    }
}
