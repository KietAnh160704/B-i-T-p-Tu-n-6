using Lab_1._1.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1._1.BUS
{
    public class StudentService
    {
       
        public List<Student> GetAll()
        {
            Model1 context = new Model1();
            return context.Students.ToList(); 
        }
        public void AddStudent(Student student)
        {
            Model1 context = new Model1();
            context.Students.Add(student);
            context.SaveChanges(); 
        }


    }
}