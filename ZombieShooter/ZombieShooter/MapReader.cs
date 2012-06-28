using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ZombieShooter
{
    public class MapReader
    {
        public bool ReadMap(string path)
        {
            try
            {
                StreamReader reader = new StreamReader(path);

                string xmlString = reader.ReadToEnd();
                string[] xmlObjArray = splitObjects(xmlString);
                createAllObjects(xmlObjArray);
                setMap(xmlString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string[] splitObjects(string xmlString)
        {
            List<string> stringArray = new List<string>();

            //remove shit
            xmlString = xmlString.Substring(xmlString.IndexOf("<Objects>") + 1, (xmlString.IndexOf("</Objects>") + 1) - (xmlString.IndexOf("<Objects>") + 1)).Replace("\t", "").Replace("\n", "").Replace("<Objects>", "").Replace("</Objects>", "");
            
            string keySplit = "<obj ";
            
            int pos = xmlString.IndexOf(keySplit);
            while (pos > -1)
            {
                int start = pos, end = xmlString.IndexOf("/>", pos + 1), len = (end + 1) - start;

                stringArray.Add(xmlString.Substring(start, len));

                pos = xmlString.IndexOf(keySplit, pos + 1);
            }
            
            return stringArray.ToArray();
        }
        public void setMap(string xmlString)
        {
            //remove shit
            xmlString = xmlString.Substring(xmlString.IndexOf("<Map>") + 1, (xmlString.IndexOf("</Map>") + 1) - (xmlString.IndexOf("<Map>") + 1)).Replace("\t", "").Replace("\n", "").Replace("<Map>", "").Replace("</Map>", "");

            int width = Convert.ToInt32(getAttribute(xmlString, "width")), height = Convert.ToInt32(getAttribute(xmlString, "height"));

            Game.stage.Width = width;
            Game.stage.Height = height;
        }

        public void createAllObjects(string[] objects)
        {
            foreach (string s in objects)
            {
                CreateObj(s);
            }
        }

        public void CreateObj(string s)
        {
            string _class = getAttribute(s, "class");
            int _x = Convert.ToInt32(getAttribute(s, "x")), _y = Convert.ToInt32(getAttribute(s, "y")), _size;
            switch (_class.ToLower())
            {
                case "wall":
                    _size = Convert.ToInt32(getAttribute(s, "size"));
                    ObjList.objList.Add(new Wall(new Vector2(_x, _y), _size));
                    break;
                case "player":
                    ObjList.objList.Add(new Player(new Vector2(_x, _y)));
                    break;
                case "spawner":
                    ObjList.objList.Add(new EnemySpawner(new Vector2(_x, _y)));
                    break;
                case "barricade":
                    ObjList.objList.Add(new Barricade(new Vector2(_x, _y)));
                    break;
            }
        }

        public string getAttribute(string objString, string attString)
        {
            int pos = objString.IndexOf(attString + "=\"");
            if (pos != -1)
            {
                int start = pos + (attString + "=\"").Length, end = objString.IndexOf("\"", start + 1), len = end - start;
                return objString.Substring(start, len);
            }
            else
            {
                return "";
            }
        }
    }
}
