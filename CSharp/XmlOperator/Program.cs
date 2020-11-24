using System;
using System.IO;
using System.Xml;

namespace XmlOperator
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement books = null;
            if (File.Exists("Books.xml"))
            {
                doc.Load("Books.xml");
                books = doc.DocumentElement;
            }
            else
            {
                //创建描述信息，必须
                XmlDeclaration des = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(des);
                //创建根节点，必须
                books = doc.CreateElement("Books");
                doc.AppendChild(books);
            }

            //写
            Write(doc, books);

            //读
            Read(books);

            //保存
            doc.Save("Books");

        }

        public static void Write(XmlDocument doc, XmlElement root)
        {
            //添加节点
            XmlElement book1 = doc.CreateElement("Book");
            root.AppendChild(book1);

            XmlElement name = doc.CreateElement("Name");
            name.InnerText = "C#从入门到放弃";
            book1.AppendChild(name);

            XmlElement price = doc.CreateElement("Price");
            price.InnerText = "58RMB";
            book1.AppendChild(price);

            XmlElement author = doc.CreateElement("Author");
            author.InnerText = "张三";
            book1.AppendChild(author);



            //添加属性
            XmlElement book2 = doc.CreateElement("Book");
            book2.SetAttribute("Name", "C++从入门到放弃");
            book2.SetAttribute("Price", "79RMB");
            book2.SetAttribute("Author", "李四");
            root.AppendChild(book2);
        }
        public static void Read(XmlElement root)
        {
            XmlNodeList xmlNodes = root.ChildNodes;
            foreach(XmlNode node in xmlNodes)
            {
                Console.WriteLine(node.InnerText);
                //属性读取
                //Console.WriteLine(node.Attributes["Name"].Value);
            }

        }

        public static void Delete(XmlElement root)
        {
            XmlNode no = root.SelectSingleNode("/Book/Price");
            root.RemoveChild(no);
        }
    }
}
