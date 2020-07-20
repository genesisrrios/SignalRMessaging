using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Helpers
{
    public class ColorsList
    {
        public Dictionary<string,string> Colors { get; set; } = new Dictionary<string,string>();

        public ColorsList()
        {
            Colors.Add("Brown","#77dd77");
            Colors.Add("Baby Blue","#89cff0");
            Colors.Add("Purple","#b39eb5");
            Colors.Add("Red","#ff6961");
            Colors.Add("Pink","#ff9899");
            Colors.Add("Dim Gray","#3B3638");
        }
    }
}
