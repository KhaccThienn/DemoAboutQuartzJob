namespace DemoAboutQuartzJob.Infrastructures.Models
{
    /// <summary>
    /// Lớp chứa thông tin cấu hình cho một công việc được lên lịch trong Quartz.NET.
    /// </summary>
    public class ScheduledJobInfo
    {
        /// <summary>
        /// Chuỗi các biểu thức cron dùng để xác định lịch trình chạy của job.
        /// Mỗi phần tử trong mảng đại diện cho một trigger riêng biệt.
        /// </summary>
        public string[] CronSchedules { get; set; }
        /// <summary>
        /// Mô tả công việc được lên lịch, giúp dễ dàng nhận diện job khi kiểm tra hoặc debug.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Hành động cấu hình job, được thực thi khi job được đăng ký trong Quartz.
        /// Có thể sử dụng để thêm dữ liệu vào JobDataMap hoặc thay đổi các thiết lập khác.
        /// Ví dụ: thêm dữ liệu vào JobDataMap:
        /// <code>
        /// JobConfigurator = job => job.UsingJobData("Key1", "Value1")
        /// </code>
        /// </summary>
        public Action<IJobConfigurator> JobConfigurator { get; set; }
        /// <summary>
        /// Cờ chỉ định job có nên chạy ngay lập tức khi scheduler khởi động hay không.
        /// Nếu giá trị là true, một trigger phụ sẽ được tạo với `StartNow()` để job được kích hoạt ngay.
        /// Mặc định là false (chỉ chạy theo lịch trình cron).
        /// </summary>
        public bool IsStartNow { get; set; }
    }
}
