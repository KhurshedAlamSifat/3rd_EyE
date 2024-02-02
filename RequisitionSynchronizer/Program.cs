using RequisitionSynchronizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequisitionSynchronizer
{
    class Program
    {
        public static  ThirdEyeDBEntities thirdEyeDB = new ThirdEyeDBEntities();
        static void Main(string[] args)
        {
            var list = thirdEyeDB.Vehicles.Take(10).ToList();
            foreach (var item in list)
            {
                Console.WriteLine(item.RegistrationNumber);
            }
            Console.ReadKey();
        }
    }
}
