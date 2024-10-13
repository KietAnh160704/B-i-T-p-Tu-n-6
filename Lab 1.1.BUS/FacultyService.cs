using Lab_1._1.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1._1.BUS
{
    public class FacultyService
    {
        public List<Faculty> GetAll()
        {
            Model1 context = new Model1();
            return context.Faculties.ToList();
        }
    }
}
