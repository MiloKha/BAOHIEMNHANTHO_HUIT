using MongoDB.Bson;
using MongoDB.Driver;
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
    public partial class frmTaoHopDong : Form
    {
        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> collection;
        public frmTaoHopDong()
        {
            InitializeComponent();
            // Chuỗi kết nối tới cơ sở dữ liệu MongoDB
            string connectionString = "mongodb://localhost:27017";

            // Tạo MongoClient và kết nối tới cơ sở dữ liệu
            client = new MongoClient(connectionString);
            database = client.GetDatabase("QLBH");
            collection = database.GetCollection<BsonDocument>("HopDong");

            // Add dữ liệu combobox giới tính
            this.cbo_gioitinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_gioitinh.FormattingEnabled = true;
            this.cbo_gioitinh.Items.AddRange(new string[] { "Nam", "Nữ" });

            //Add dữ liệu cho combobox tên bảo hiểm
            this.cbo_tenBH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_tenBH.FormattingEnabled = true;
            cbo_tenBH.Items.AddRange(new string[]
                {
                    "Bảo hiểm trọn đời",
                    "Bảo hiểm sinh kỳ",
                    "Bảo hiểm tử kỳ",
                    "Bảo hiểm hỗn hợp",
                    "Bảo hiểm trả tiền định kỳ",
                    "Bảo hiểm liên kết đầu tư",
                    "Bảo hiểm hưu trí"
                });

            this.cbo_stBH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_stBH.FormattingEnabled = true;
            cbo_stBH.Items.Add("5,000,000");
            cbo_stBH.Items.Add("10,000,000");

            this.cbo_stGD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_stGD.FormattingEnabled = true;
            cbo_stGD.Items.Add("5,000,000");
            cbo_stGD.Items.Add("10,000,000");

            // Add dữ liệu cho combobox tên giao dịch
            this.cbo_tenGD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_tenBH.FormattingEnabled = true;
            cbo_tenGD.Items.AddRange(new string[]
                {
                    "Đóng định kỳ",
                    "Tạm ứng"
                });
        }

        private BindingSource GetHopDongList()
        {
            var hopDongList = collection.Find(new BsonDocument()).ToList();
            return new BindingSource { DataSource = hopDongList };
        }

        // Lấy danh sách hợp đồng từ cơ sở dữ liệu
        private BindingSource GetHopDongListData()
        {
            var hopDongList = collection.Find(new BsonDocument()).ToList();
            return new BindingSource { DataSource = hopDongList };
        }

        private void ClearTextBoxes()
        {

            txt_ten.Text = "";
            txt_diachi.Text = "";
            txt_sdt.Text = "";
            cbo_gioitinh.SelectedItem = null;
            txt_nghenghiep.Text = "";
            cbo_tenBH.SelectedItem = null;
            cbo_tenGD.SelectedItem = null;
            cbo_stGD.SelectedItem = null;
            cbo_stBH.SelectedItem = null;
            txt_quanhe.Text = "";
            txt_nguoith.Text = "";
        }

        private void txt_ten_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbo_tenBH_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_themHD_Click(object sender, EventArgs e)
        {
            validateDate = true; // Kích hoạt kiểm tra ngày
            DateTime ngayBatDau = dtp_ngayBD.Value;
            DateTime ngayKetThuc = dtp_ngayKT.Value;

            // Kiểm tra số ngày chênh lệch giữa ngày bắt đầu và ngày kết thúc
            TimeSpan KtraNgay = ngayKetThuc.Subtract(ngayBatDau);
            if (Math.Abs(KtraNgay.Days) < 5 * 365) // Kiểm tra nếu số ngày chênh lệch nhỏ hơn 5 năm (365 ngày x 5)
            {
                MessageBox.Show("Ngày bắt đầu và ngày kết thúc phải cách nhau ít nhất 5 năm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var hopDong = new BsonDocument
                {
                    { "Giaodich", new BsonDocument
                        {
                            { "Magd", Guid.NewGuid().ToString() },
                            { "Ngaydg", dtp_ngayGD.Value },
                            { "Sotiendg", cbo_stGD.SelectedItem.ToString()},
                            { "GiaoDich", new BsonDocument
                                {
                                    { "MaDG", Guid.NewGuid().ToString() },
                                    {"TenDG", cbo_tenGD.SelectedItem.ToString()}
                                }
                            }
                        }
                    },
                    { "Hosoboithuong", new BsonDocument
                        {
                            { "Mahs", Guid.NewGuid().ToString() },
                            { "Ngayyeucau", BsonNull.Value },
                            { "Motahoso", string.Empty },
                            { "Tinhtrang", true }
                        }
                    },
                    { "Khachhang", new BsonDocument
                        {
                            { "Makh", Guid.NewGuid().ToString() },
                            { "Tenkh", txt_ten.Text },
                            { "Diachi", txt_diachi.Text },
                            { "SDT", txt_sdt.Text },
                            { "Gioitinh", cbo_gioitinh.SelectedItem.ToString() },
                            { "Nghenghiep", txt_nghenghiep.Text }
                        }
                    },
                    { "Ngaybatdau", dtp_ngayBD.Value },
                    { "Ngayketthuc", dtp_ngayKT.Value },
                    { "Nguoithuhuong", new BsonDocument
                        {
                            { "Manth", Guid.NewGuid().ToString() },
                            { "Hoten",  txt_nguoith.Text},
                            { "QuanheKH", txt_quanhe.Text},
                            { "Ngaysinh",  dtp_ngaysinhnth.Value }
                        }
                    },
                    { "Sotienbaohiem", cbo_stBH.SelectedItem.ToString()},
                    { "BaoHiem", new BsonDocument
                        {
                            { "id_baohiem", Guid.NewGuid().ToString() },
                            { "Tenbaohiem", cbo_tenBH.SelectedItem.ToString() }
                        }
                    },
                };

            // Thêm hợp đồng vào cơ sở dữ liệu
            collection.InsertOne(hopDong);

            // Xóa dữ liệu trong TextBox
            ClearTextBoxes();
            validateDate = false; // Tắt kiểm tra ngày
        }
        private void frmTaoHopDong_Load(object sender, EventArgs e)
        {
            txt_ten.KeyPress += txt_ten_KeyPress;
            txt_nghenghiep.KeyPress += txt_ten_KeyPress;
            txt_nguoith.KeyPress += txt_ten_KeyPress;
            txt_quanhe.KeyPress += txt_ten_KeyPress;
            txt_sdt.KeyPress += txt_sdt_KeyPress;
            txt_sdt.TextChanged += txt_sdt_TextChanged;
            dtp_ngayBD.ValueChanged += dtp_ngayBD_ValueChanged;
            dtp_ngayKT.ValueChanged += dtp_ngayKT_ValueChanged;
        }
        private void txt_ten_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu ký tự không phải là chữ cái hoặc khoảng trắng
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar))
            {
                e.Handled = true; // Ngăn không cho ký tự nhập vào
            }
        }

        private void txt_sdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu ký tự không phải là số và không phải là phím Backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Ngăn không cho ký tự nhập vào
            }
        }

        private void txt_sdt_TextChanged(object sender, EventArgs e)
        {
            string text = txt_sdt.Text;

            // Kiểm tra nếu có ký tự không phải số
            if (text.Any(c => !char.IsDigit(c)))
            {
                // Xóa ký tự không hợp lệ và cập nhật giá trị của textbox
                txt_sdt.Text = new string(text.Where(char.IsDigit).ToArray());
            }

            if (txt_sdt.Text.Length > 10)
            {
                txt_sdt.Text = txt_sdt.Text.Substring(0, 10); // Giới hạn độ dài của số điện thoại là 10 ký tự
            }
        }
        private bool validateDate = false;
        private void dtp_ngayBD_ValueChanged(object sender, EventArgs e)
        {
            if (!validateDate)
            {
                return;
            }

            DateTime ngayBatDau = dtp_ngayBD.Value;
            DateTime ngayKetThuc = dtp_ngayKT.Value;

            // Kiểm tra số ngày chênh lệch giữa ngày bắt đầu và ngày kết thúc
            TimeSpan KtraNgay = ngayKetThuc.Subtract(ngayBatDau);
            if (Math.Abs(KtraNgay.Days) < 5 * 365) // Kiểm tra nếu số ngày chênh lệch nhỏ hơn 5 năm (365 ngày x 5)
            {
                MessageBox.Show("Ngày bắt đầu và ngày kết thúc phải cách nhau ít nhất 5 năm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtp_ngayKT_ValueChanged(object sender, EventArgs e)
        {
            if (!validateDate)
            {
                return;
            }

            DateTime ngayBatDau = dtp_ngayBD.Value;
            DateTime ngayKetThuc = dtp_ngayKT.Value;

            // Kiểm tra số ngày chênh lệch giữa ngày bắt đầu và ngày kết thúc
            TimeSpan KtraNgay = ngayKetThuc.Subtract(ngayBatDau);
            if (Math.Abs(KtraNgay.Days) < 5 * 365) // Kiểm tra nếu số ngày chênh lệch nhỏ hơn 5 năm (365 ngày x 5)
            {
                MessageBox.Show("Ngày bắt đầu và ngày kết thúc phải cách nhau ít nhất 5 năm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
