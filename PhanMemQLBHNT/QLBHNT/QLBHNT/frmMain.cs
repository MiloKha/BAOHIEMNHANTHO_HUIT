using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace QLBHNT
{
    public partial class frmMain : Form
    {  
            private IMongoCollection<BsonDocument> collection;
            private MongoClient client;
            private IMongoDatabase database;

            private frmQLHopDong frmXemHopDong;
        public frmMain()
        {
            
            InitializeComponent();
            IMongoCollection<BsonDocument> dbCollection = GetMongoDBCollection();
            frmXemHopDong = new frmQLHopDong(dbCollection);
        }
        private IMongoCollection<BsonDocument> GetMongoDBCollection()
        {
            string connectionString = "mongodb://localhost:27017";
            string databaseName = "QLBH";
            string collectionName = "HopDong";

            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(databaseName);
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);

            return collection;
        }
        private void quảnLýKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDanhSachKhachHang fr1 = new frmDanhSachKhachHang();
            fr1.TopLevel = false;
            fr1.AutoScroll = true;

            fr1.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr1);
            fr1.Show();
        }

        private void quảnLýHợpĐồngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            frmDanhSachKhachHang fr1 = new frmDanhSachKhachHang();
            fr1.TopLevel = false;
            fr1.AutoScroll = true;

            fr1.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr1);
            fr1.Show();
        }

        private void tạoHợpĐồngMớiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void xemHợpĐồngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(frmXemHopDong);
        }
        private void ShowFormInPanel(Form form)
        {
            panel1.Controls.Clear();
            form.TopLevel = false;
            form.AutoScroll = true;
            form.Dock = DockStyle.Fill;
            panel1.Controls.Add(form);
            form.Show();
        }
        private void hồSơBồiThườngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHoSoBoiThuong fr = new frmHoSoBoiThuong();
            fr.TopLevel = false;
            fr.AutoScroll = true;
            fr.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr);
            fr.Show();
        }

        private void đóngBảoHiểmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongKeHopDongDuocKy fr = new frmThongKeHopDongDuocKy();
            fr.TopLevel = false;
            fr.AutoScroll = true;
            fr.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr);
            fr.Show();
        }

        private void bồiThườngBảoHiểmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongKeHSBoiThuong fr = new frmThongKeHSBoiThuong();
            fr.TopLevel = false;
            fr.AutoScroll = true;
            fr.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr);
            fr.Show();
        }

        private void hồSơBồiThườngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmThongKeSoTienDongBaoHiem fr = new frmThongKeSoTienDongBaoHiem();
            fr.TopLevel = false;
            fr.AutoScroll = true;
            fr.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr);
            fr.Show();
        }

        private void sốTiềnBồiThườngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongKeTienBoiThuong fr = new frmThongKeTienBoiThuong();
            fr.TopLevel = false;
            fr.AutoScroll = true;
            fr.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr);
            fr.Show();
        }

        private void kháchHàngThamGiaBHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongKeKHThamGiaBH fr = new frmThongKeKHThamGiaBH();
            fr.TopLevel = false;
            fr.AutoScroll = true;
            fr.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr);
            fr.Show();
        }

        private void tạoHợpĐồngMớiToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmTaoHopDong fr = new frmTaoHopDong();
            fr.TopLevel = false;
            fr.AutoScroll = true;
            fr.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr);
            fr.Show();
        }

        private void quảnLýTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLChuongTrinh fr = new QLChuongTrinh();
            fr.TopLevel = false;
            fr.AutoScroll = true;
            fr.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr);
            fr.Show();
        }

        private void phânQuyềnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongTinCaNhan fr = new ThongTinCaNhan();
            fr.TopLevel = false;
            fr.AutoScroll = true;
            fr.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(fr);
            fr.Show();
        }

    }
}
 