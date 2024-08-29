using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBHNT
{
    public partial class frmThongKeTienBoiThuong : Form
    {
        public frmThongKeTienBoiThuong()
        {
            InitializeComponent();
            comboBox1.Items.Add("Lọc số tiền nhiều nhất");
            comboBox1.Items.Add("Lọc số tiền ít nhất");
            comboBox1.Items.Add("Sắp xếp theo khách hàng");
        }
    }
}
