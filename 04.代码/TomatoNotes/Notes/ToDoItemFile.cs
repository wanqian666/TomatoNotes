using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace Notes
{
    class ToDoItemFile
    {
        private string _path = "D://Note.xml";
       
        public Item[] GetXml()
        {
            if (!File.Exists(_path))//若文件夹不存在则新建文件夹   
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);
                //创建一个根节点（一级）
                XmlElement root = doc.CreateElement("Notes");
                doc.AppendChild(root);

                doc.Save(@_path);
                Console.Write(doc.OuterXml);
            }  
            //读取Xml文件数据
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(_path);
            XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("Notes").ChildNodes;
            //dataGridView1.Rows.Clear();
            int count = xmlNodeList.Count;
            Item[] items = new Item[count];
            int i = 0;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                Item item = new Item();
                XmlElement xmlElement = (XmlElement)xmlNode;
                item._isAlarm = Convert.ToBoolean(xmlElement.ChildNodes.Item(0).InnerText);
                item._detail = xmlElement.ChildNodes.Item(1).InnerText;
                item._endTime = Convert.ToDateTime(xmlElement.ChildNodes.Item(2).InnerText);
                item._isFinish = Convert.ToBoolean(xmlElement.ChildNodes.Item(3).InnerText);
                item._id = Convert.ToInt32(xmlElement.ChildNodes.Item(4).InnerText);
                items[i] = item; 
                i++;
            } 
            return items;
        }

        public void WriteXml(Item newitem)
        {
            ////添加元素 things
            XmlDocument myDoc = new XmlDocument();
            myDoc.Load(_path);
            XmlElement ele = myDoc.CreateElement("alarm");
            XmlText text = myDoc.CreateTextNode(Convert.ToString(newitem._isAlarm));
            XmlElement ele1 = myDoc.CreateElement("things");
            XmlText text1 = myDoc.CreateTextNode(newitem._detail);
            XmlElement ele2 = myDoc.CreateElement("time");
            XmlText text2 = myDoc.CreateTextNode(Convert.ToString(newitem._endTime));
            XmlElement ele3 = myDoc.CreateElement("finish");
            XmlText text3 = myDoc.CreateTextNode(Convert.ToString(newitem._isFinish));
            XmlElement ele4 = myDoc.CreateElement("id");
            XmlText text4 = myDoc.CreateTextNode(Convert.ToString(newitem._id));
            //添加节点
            XmlNode newElem = myDoc.CreateNode("element", "Note", "");

            //在节点添加元素
            newElem.AppendChild(ele);
            newElem.LastChild.AppendChild(text);
            newElem.AppendChild(ele1);
            newElem.LastChild.AppendChild(text1);
            newElem.AppendChild(ele2);
            newElem.LastChild.AppendChild(text2);
            newElem.AppendChild(ele3);
            newElem.LastChild.AppendChild(text3);
            newElem.AppendChild(ele4);
            newElem.LastChild.AppendChild(text4);

            XmlElement root = myDoc.DocumentElement;
            root.AppendChild(newElem);

            //保存
            myDoc.Save(_path);
        }

        public void UpdateXml(Item[] _items)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(_path);
            XmlNode xmlElement_Notes = xmlDocument.SelectSingleNode("Notes");
            xmlElement_Notes.RemoveAll();
            foreach (Item it in _items)
            {
                XmlElement xmlElement_Note = xmlDocument.CreateElement("Note");
                XmlElement xmlElement_alarm = xmlDocument.CreateElement("alarm");
                xmlElement_alarm.InnerText = Convert.ToString(it._isAlarm);
                xmlElement_Note.AppendChild(xmlElement_alarm);
                //添加detail
                XmlElement xmlElement_detail = xmlDocument.CreateElement("detail");
                xmlElement_detail.InnerText = it._detail;
                xmlElement_Note.AppendChild(xmlElement_detail);
                //
                XmlElement xmlElement_endtime = xmlDocument.CreateElement("endtime");
                xmlElement_endtime.InnerText = Convert.ToString(it._endTime);
                xmlElement_Note.AppendChild(xmlElement_endtime);
                //
                XmlElement xmlElement_finish = xmlDocument.CreateElement("finish");
                xmlElement_finish.InnerText = Convert.ToString(it._isFinish);
                xmlElement_Note.AppendChild(xmlElement_finish);
                //
                XmlElement xmlElement_id = xmlDocument.CreateElement("id");
                xmlElement_id.InnerText = Convert.ToString(it._id);
                xmlElement_Note.AppendChild(xmlElement_id);
                xmlElement_Notes.AppendChild(xmlElement_Note);
            }
            
            
            xmlDocument.Save(_path);
        }
    }
}
