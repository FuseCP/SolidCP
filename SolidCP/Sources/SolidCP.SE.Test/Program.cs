using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.SE;

namespace SolidCP.SE.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AddDomain test01.expertservices.co");
            SE.AddDomain("test01.expertservices.co", "fjiwoejfow", "test@test01.expertservices.co", null);
            Console.ReadLine();

            Console.WriteLine("AddDomain test01.expertservices.co");
            SE.AddDomain("test01.expertservices.co", "fjiwoejfow", "test@test01.expertservices.co", null);
            Console.ReadLine();


            Console.WriteLine("AddEmail test@test01.expertservices.co Aa123123~");
            SE.AddEmail("test", "test01.expertservices.co", "Aa123123~");
            Console.ReadLine();

            Console.WriteLine("AddEmail test@test01.expertservices.co Aa123123~");
            SE.AddEmail("test", "test01.expertservices.co", "Aa123123~");
            Console.ReadLine();

            Console.WriteLine("SetEmailPassword test@test01.expertservices.co Bb123123~");
            SE.SetEmailPassword("test@test01.expertservices.co", "Bb123123~");

            Console.WriteLine("DeleteEmail test@test01.expertservices.co");
            SE.DeleteEmail("test@test01.expertservices.co");
            Console.ReadLine();

            Console.WriteLine("DeleteEmail test@test01.expertservices.co");
            SE.DeleteEmail("test@test01.expertservices.co");
            Console.ReadLine();

            Console.WriteLine("DeleteDomain test01.expertservices.co");
            SE.DeleteDomain("test01.expertservices.co");
            Console.ReadLine();

            Console.WriteLine("DeleteDomain test01.expertservices.co");
            SE.DeleteDomain("test01.expertservices.co");
            Console.ReadLine();
        }
    }
}
