using Lab_1._1.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1._1.BUS
{
    internal class MajorService
    {
        public List<Major> GetAllByFaculty(int facultyID)
        {
            Model1 context = new Model1();
            return context.Majors.Where(p => p.FacultyID == facultyID).ToList();
        }
    }
}
