using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace QLBHNT
{
    public partial class frmThongKeHopDongDuocKy : Form
    {
        private IMongoCollection<BsonDocument> hopDongCollection;
        private MongoClient client;
        private IMongoDatabase database;
        public frmThongKeHopDongDuocKy()
        {
            InitializeComponent();

            string connectionString = "mongodb://localhost:27017"; // Thay đổi tên địa chỉ và cổng MongoDB tại đây
            client = new MongoClient(connectionString);
            database = client.GetDatabase("QLBH");
            hopDongCollection = database.GetCollection<BsonDocument>("HopDong"); // Thay tên collection mong muốn

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void frmThongKeHopDongDuocKy_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void btnthongke_Click(object sender, EventArgs e)
        {
            //Thống kế hồ sơ được bồi thường
            loadHopDongDaKy();
            loadHoSoBoiThuong();
        }
        public void loadHopDongDaKy()
        {
            DateTime startTime = dateTimePicker1.Value;
            DateTime endTime = dateTimePicker2.Value;

            var filter = Builders<BsonDocument>.Filter.And(
                 Builders<BsonDocument>.Filter.Gte("Ngaybatdau", startTime),
                 Builders<BsonDocument>.Filter.Lte("Ngaybatdau", endTime)
            );

            var documents = hopDongCollection.Find(filter).ToList();

            var groupedData = documents
                .GroupBy(d => {
                    var ngaybatdau = ((DateTime)d["Ngaybatdau"]).ToUniversalTime().ToLocalTime();
                    return new DateTime(ngaybatdau.Year, ngaybatdau.Month, 1);
                })
                .Select(g => new { MonthYear = new DateTime(g.Key.Year, g.Key.Month, 1), Count = g.Count() })
                .ToList();

            // Vẽ biểu đồ
            chart1.Series[0].Points.Clear();
            foreach (var dataPoint in groupedData)
            {
                string label = dataPoint.MonthYear.ToString("MM/yyyy", CultureInfo.InvariantCulture);
                chart1.Series[0].Points.AddXY(label, dataPoint.Count);
            }
        }
        public void loadHoSoBoiThuong()
        {
            // Tạo filter từ ngày bắt đầu và ngày kết thúc
            var filter = Builders<BsonDocument>.Filter.And(
                 Builders<BsonDocument>.Filter.Gte("Hosoboithuong.Ngayyeucau", dateTimePicker1.Value),
                 Builders<BsonDocument>.Filter.Lte("Hosoboithuong.Ngayyeucau", dateTimePicker2.Value)
             );
            var documentss = hopDongCollection.Find(filter).ToList();

            // Xử lý dữ liệu
            var groupedData = new Dictionary<DateTime, int>();

            foreach (var document in documentss)
            {
                if (document.TryGetValue("Hosoboithuong", out var hosoboithuong) && hosoboithuong is BsonDocument)
                {
                    var ngayyeucau = hosoboithuong.AsBsonDocument.GetValue("Ngayyeucau").ToUniversalTime().Date;

                    if (groupedData.ContainsKey(ngayyeucau))
                    {
                        groupedData[ngayyeucau] += 1;
                    }
                    else
                    {
                        groupedData[ngayyeucau] = 1;
                    }
                }
            }

            // Vẽ biểu đồ
            chart2.Series[0].Points.Clear();
            foreach (var dataPoint in groupedData)
            {
                string label = dataPoint.Key.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                chart2.Series[0].Points.AddXY(label, dataPoint.Value);
            }
        }

    }
}
