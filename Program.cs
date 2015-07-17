using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace ReflectionTest3
{
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person();
            p.Save();
        }
    }

    public class Person
    {
        private int age = -1;
        private string name = String.Empty;

        public void Load()
        {
            if (File.Exists("settings.dat"))
            {
                Type type = this.GetType();

                string propertyName, value;
                string[] temp;
                char[] splitChars = new char[] { '|' };
                PropertyInfo propertyInfo;

                string[] settings = File.ReadAllLines("settings.dat");
                foreach (string s in settings)
                {
                    temp = s.Split(splitChars);
                    if (temp.Length == 2)
                    {
                        propertyName = temp[0];
                        value = temp[1];
                        propertyInfo = type.GetProperty(propertyName);
                        if (propertyInfo != null)
                        {
                            this.SetProperty(propertyInfo, value);
                        }
                    }
                }
            }
        }

        public void Save()
        {
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();
            TextWriter tw = new StreamWriter("settings.dat");
            foreach (PropertyInfo propertyInfo in properties)
            {
                tw.WriteLine(propertyInfo.Name + "|" + propertyInfo.GetValue(this, null));
            }
            tw.Close();
        }

        public void SetProperty(PropertyInfo propertyInfo, object value)
        {
            switch (propertyInfo.PropertyType.Name)
            {
                case "Int32":
                    propertyInfo.SetValue(this, Convert.ToInt32(value), null);
                    break;

                case "String":
                    propertyInfo.SetValue(this, value.ToString(), null);
                    break;
            }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
