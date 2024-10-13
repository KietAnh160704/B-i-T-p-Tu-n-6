using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Lab_1._1.BUS;
using Lab_1._1.DAL.Entities;

namespace Lab_1._1.GUI
{
    public partial class Form1 : Form
    { 
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        Model1 context = new Model1();
        public Form1()
        {
            InitializeComponent();
            studentService = new StudentService(); 
            facultyService = new FacultyService();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            try

            {
                setGridViewStyle(dgvSinhVien);
                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();
                FillFalcultyCombobox(listFacultys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillFalcultyCombobox(List<Faculty> listFaculty)
        {
            listFaculty.Insert(0, new Faculty());
            this.cmbFaculty.DataSource = listFaculty;
            this.cmbFaculty.DisplayMember = "FacultyName" ;
            this.cmbFaculty.ValueMember = "FacultyID" ;
        }
        private void BindGrid(List<Student> listStudent)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = item.StudentID;
                dgvSinhVien.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dgvSinhVien.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvSinhVien.Rows[index].Cells[3].Value = item.AverageScore + "" ;
                if (item.MajorID != null)
                    dgvSinhVien.Rows[index].Cells[4].Value = item.Major.Name + "" ;
                
            }
        }
        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (!int.TryParse(txtMSSV.Text.Trim(), out int studentID))
                {
                    MessageBox.Show("Vui lòng nhập định dạng số hợp lệ cho Mã sinh viên.");
                    txtMSSV.Focus();
                    return;
                }

                
                string fullName = txtHoten.Text.Trim();
                if (string.IsNullOrEmpty(fullName))
                {
                    MessageBox.Show("Tên sinh viên không được để trống.");
                    txtHoten.Focus();
                    return;
                }

                
                if (cmbFaculty.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn Khoa.");
                    cmbFaculty.Focus();
                    return;
                }
                int facultyID = (int)cmbFaculty.SelectedValue;

                
                if (!decimal.TryParse(txtDTB.Text.Trim(), out decimal averageScore) || averageScore < 0)
                {
                    MessageBox.Show("Điểm trung bình phải là số dương.");
                    txtDTB.Focus();
                    return;
                }

                
                int? majorID = null; 

                using (var context = new Model1())
                {
                    
                    var existingStudent = context.Students.FirstOrDefault(s => s.StudentID == studentID);
                    if (existingStudent != null)
                    {
                        
                        existingStudent.FullName = fullName;
                        existingStudent.FacultyID = facultyID;
                        existingStudent.AverageScore = averageScore;
                        existingStudent.MajorID = majorID; 

                        MessageBox.Show("Cập nhật thông tin sinh viên thành công!");
                    }
                    else
                    {
                        
                        var student = new Student
                        {
                            StudentID = studentID,
                            FullName = fullName,
                            FacultyID = facultyID,
                            AverageScore = averageScore,
                            MajorID = majorID, 
                            Avatar = "test.jpg"
                        };

                        context.Students.Add(student);
                        MessageBox.Show("Thêm sinh viên thành công!");
                    }

                    context.SaveChanges();

                    
                    BindGrid(context.Students.ToList());

                    
                    ClearInputFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm/sửa sinh viên: " + ex.Message);
            }
        }
        private int? GetFacultyIdByName(string facultyName)
        {
            
            using (var context = new Model1())
            {
                return context.Faculties.FirstOrDefault(f => f.FacultyName == facultyName)?.FacultyID;
            }
        }
        private void ClearInputFields()
        {
            txtMSSV.Clear();
            txtHoten.Clear();
            txtDTB.Clear();
            cmbFaculty.SelectedIndex = -1; 
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrEmpty(txtMSSV.Text))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để xóa.");
                    return;
                }

                if (int.TryParse(txtMSSV.Text.Trim(), out int studentID))
                {
                    using (var context = new Model1()) 
                    {
                        
                        var studentToDelete = context.Students.FirstOrDefault(s => s.StudentID == studentID);
                        if (studentToDelete != null)
                        {
                            
                            context.Students.Remove(studentToDelete);
                            context.SaveChanges();

                            MessageBox.Show("Xóa sinh viên thành công!");

                            
                            var listStudents = studentService.GetAll();
                            BindGrid(listStudents);

                            
                            txtMSSV.Clear();
                            txtHoten.Clear();
                            txtDTB.Clear();
                            cmbFaculty.SelectedIndex = -1;
                        }
                        else
                        {
                            MessageBox.Show("Sinh viên không tồn tại.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mã sinh viên không hợp lệ.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message);
            }
        }
        private void dgvSinhVien_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dgvSinhVien.Rows[e.RowIndex];

                // Lấy thông tin từ dòng đã chọn và hiển thị lên các TextBox
                txtMSSV.Text = selectedRow.Cells[0].Value.ToString();
                txtHoten.Text = selectedRow.Cells[1].Value.ToString();
                cmbFaculty.SelectedValue = GetFacultyIdByName(selectedRow.Cells[2].Value.ToString());
                txtDTB.Text = selectedRow.Cells[3].Value.ToString();
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = studentService.GetAll();

            if (checkBox1.Checked)
            {
                // Lọc chỉ hiển thị sinh viên chưa có chuyên ngành
                listStudents = listStudents.Where(s => s.MajorID == null).ToList();
            }

            BindGrid(listStudents);
        }
    }   
}
