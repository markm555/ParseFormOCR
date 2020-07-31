using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

/*
07302020 - Mark Moore

This code takes input in the form of JSON as provided by Azure Computer vison OCR and scrapes the text from a specific portion of the form in a
virutual box defined by an if statement.  The text is then written to the console.  It would be easy to turn this into a function
and return the text as a string.

*/



namespace ParseJson1
{
    class Program
    {
        static void Main(string[] args)
        {
            string box;                                                                              //used to hold the string of the bounding box
            string text;                                                                             //used to hold the string of the text

            var json = System.IO.File.ReadAllText(@"c:\users\markm.moorecasa\documents\test.json");  //Input file location and name

            var objects = JArray.Parse(json);                                                        //parse the JSON
            int x;                                                                                   //flag to indicate if the next pass is text from within the box

            foreach (JObject root in objects)                                                        //loop through the rows in file
            {
                x = 0;                                                                               //set flag to 0

                foreach (KeyValuePair<String, JToken> app in root)                                   //loop through key value pairs of each row
                {
                    box = (string)app.Key;                                                           //pull the key
                    text = (string)app.Value;                                                        //pull the value
                    if (x == 1)                                                                      //if this is text we want write it to the console
                    {
                        Console.WriteLine(text);
                    }

                    if (text.Contains(','))                                                          //check to see if text contains a comma for bounding box info
                    {
                        string[] part = text.Split(',');                                             //create an string array with bounding box values
                        try                                                                          //it is possible to have text that contains a comma that is not part of a bounding box.  if so ignore it.
                        {
                            if (Int32.TryParse(part[0], out int MarginX)) { };                           //convert first Value to int X
                            if (Int32.TryParse(part[1], out int MarginY)) { };                           //convert second Value to int Y
                            if (Int32.TryParse(part[2], out int width)) { };                             //convert third Value to int width
                            if (Int32.TryParse(part[3], out int height)) { };                            //convert fourth Value to int Hight

                            if (MarginX > 120 && MarginX < 550 && MarginY < 175)                     //draw a virtual box containing text we want
                            {
                                x = 1;                                                               //set flag for next pass through.  This pass is the bounding box
                            }                                                                        //the next pass will contain the text we are looking for
                        }
                        catch { }
                    }
                }
            }
        }
    }
}

