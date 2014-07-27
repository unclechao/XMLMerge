using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLUpdateMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            string oldXMLPath = @"G:\My Projects\Old.xml";
            string newXMLPath = @"G:\My Projects\New.xml";

            XMLMerge mergeTool = new XMLMerge();
            mergeTool.UpdateXMLFile(oldXMLPath, newXMLPath);
            Console.WriteLine("XML Merge finished! Press ant key to exit");
            Console.ReadKey();
        }
    }
}
