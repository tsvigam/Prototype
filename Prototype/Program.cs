using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

namespace Prototype
{
    class Program
    {
        static void Main(string[] args)
        {
            ITree t1 = new BirchTree(1, "yellow", "serezki");
            t1.GetName();
            ITree t2 = t1.DeepClone();
            t2.Name = "Birch 2";
            t2.Plod.flower = "changed color";
            t2.GetName();
            t1.GetName();
            ITree t3 = new AppleTree(1, "white", "apples");
            t3.GetName();
            ITree t4 = t3.Clone() as ITree;
            t4.Plod.plod = "other apples";
            t4.GetName();// t4.GetPlod();
        }
    }

    [Serializable]
    public class Plod
    {
        public string flower;
        public string plod;
    }

    public interface ITree
    {
        public Plod Plod { get; set; }
        public string Name { get; set; }
        ITree Clone();
        ITree DeepClone();
        void GetName();
    }

    [Serializable]
    public class BirchTree : ITree
    {
        public Plod Plod { get; set; }
        public string Name { get; set; } = "BirchTree";

        public ITree Clone()
        {
            char[] num = this.Name.Where(x => Char.IsDigit(x)).ToArray();
            int id = (int.Parse(num)) + 1;
            // return new BirchTree(id,this.Plod.flower, this.Plod.plod); //ссылочное копирование не сработало
            return this.MemberwiseClone() as ITree; //ссылочное копирование сработало
        }

        public ITree DeepClone()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;

                return (ITree)formatter.Deserialize(ms);
            }
        }

        public BirchTree(int id, string f, string p)
        {
            this.Name = "BirchTree-" + id;
            this.Plod = new Plod { flower = f, plod = p };
        }

        public BirchTree(string name, Plod plod)
        {
            this.Name = name;
            this.Plod = plod;
        }

        public void GetName()
        {
            Console.WriteLine(this.Name + " with " + Plod.plod + ", " + Plod.flower);
        }
    }
    [Serializable]
    class AppleTree : ITree
    {
        public string Name { get; set; } = "AplleTree";
        public Plod Plod { get; set; }

        public ITree Clone()
        {
            char[] num = this.Name.Where(x => Char.IsDigit(x)).ToArray();
            int id = (int.Parse(num)) + 1;
            return new AppleTree(id, this.Plod.flower, this.Plod.plod);
        }

        public ITree DeepClone()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                return (ITree)formatter.Deserialize(ms);
            }
        }
        //    ITree other = (ITree)this.MemberwiseClone();
        //    char[] num = this.Name.Where(x => Char.IsDigit(x)).ToArray();
        //    int id = (int.Parse(num)) + 1;
        //    other.Name = this.Name + id;
        //    other.Plod.flower = String.Copy(this.Plod.flower);
        //    other.Plod.plod = String.Copy(this.Plod.plod);
        //    return other;

        public AppleTree(int id, string f, string p)
        {
            this.Name = "AppleTree-" + id;
            this.Plod = new Plod { flower = f, plod = p };
        }

        public void GetName()
        {
            Console.WriteLine(this.Name + " with " + Plod.plod + ", " + Plod.flower);
        }

        public void GetPlod()
        {
            Console.WriteLine(Plod.plod + "from - " + this.Name);
        }
    }
}
