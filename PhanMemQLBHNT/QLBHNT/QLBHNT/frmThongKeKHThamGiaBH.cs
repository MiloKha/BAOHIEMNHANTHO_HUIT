using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;


namespace QLBHNT
{
    public partial class frmThongKeKHThamGiaBH : Form
    {
        private IMongoCollection<BsonDocument> hopDongCollection;
        private IMongoCollection<BsonDocument> hosoboithuongCollection;
        private MongoClient client;
        private IMongoDatabase database;
        public frmThongKeKHThamGiaBH()
        {
            InitializeComponent();

            
        }

        private void btnthongke_Click(object sender, EventArgs e)
        {
            string connectionString = "mongodb://localhost:27017"; // Thay đổi tên địa chỉ và cổng MongoDB tại đây
            client = new MongoClient(connectionString);
            database = client.GetDatabase("QLBH");
            hopDongCollection = database.GetCollection<BsonDocument>("HopDong"); // Thay tên collection mong muốn
            DateTime startTime = dateTimePicker1.Value;
            DateTime endTime = dateTimePicker2.Value;

            // Lấy collection "your_collection_name"
           // var collection = database.GetCollection<BsonDocument>("HopDong");
            // Query dữ liệu từ MongoDB
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Gte("Ngaybatdau", startTime),
                Builders<BsonDocument>.Filter.Lte("Ngayketthuc", endTime)
            );

            // Lấy danh sách hợp đồng phù hợp
            var hopDongs = hopDongCollection.Find(filter).ToList();
            // Tạo dictionary để lưu trữ dữ liệu theo tháng
            Dictionary<string, decimal[]> dataByMonth = new Dictionary<string, decimal[]>();

            foreach (var hopDong in hopDongs)
            {
                var hsboithuong = hopDong["Hosoboithuong"].AsBsonDocument;
                string monthYear = hopDong["Ngaybatdau"].ToLocalTime().ToString("MM/yyyy");

                // Kiểm tra nếu monthYear đã có trong dictionary hay chưa
                if (dataByMonth.ContainsKey(monthYear))
                {
                    // Nếu có, cập nhật dữ liệu
                    dataByMonth[monthYear][0] += hopDong["Sotienbaohiem"].ToDecimal();
                    dataByMonth[monthYear][1] += hsboithuong["Tienboithuong"].ToDecimal();
                }
                else
                {
                    // Nếu chưa, thêm dữ liệu vào dictionary
                    decimal[] newData = new decimal[2];
                    newData[0] = hopDong["Sotienbaohiem"].ToDecimal();
                    newData[1] = hsboithuong["Tienboithuong"].ToDecimal();
                    dataByMonth.Add(monthYear, newData);
                }
            }

            // Tạo biểu đồ
            chart1.Series.Clear();
            chart1.Titles.Clear();

            Series baoHiemSeries = chart1.Series.Add("Số tiền bảo hiểm");
            baoHiemSeries.ChartType = SeriesChartType.Column;

            Series boiThuongSeries = chart1.Series.Add("Số tiền bồi thường");
            boiThuongSeries.ChartType = SeriesChartType.Column;

            foreach (var data in dataByMonth)
            {
                string monthYear = data.Key;
                decimal sotienbaohiem = data.Value[0];
                decimal tienboithuong = data.Value[1];

                baoHiemSeries.Points.AddXY(monthYear, sotienbaohiem);
                boiThuongSeries.Points.AddXY(monthYear, tienboithuong);
            }

            // Đổi tên trục x thành tháng/năm
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM/yyyy";

            // Hiển thị tiêu đề và hiển thị biểu đồ
            chart1.Titles.Add("Thống kê số tiền bảo hiểm và số tiền bồi thường"); 
            chart1.ChartAreas[0].RecalculateAxesScale();

            // Hiển thị biểu đồ trên form
            chart1.Visible = true;
        }

        private void frmThongKeKHThamGiaBH_Load(object sender, EventArgs e)
        {

        }
        
    }
}
