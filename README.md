# Cách Tính CronSchedule trong Quartz.NET

Trong **Quartz.NET**, `CronSchedule` dùng cú pháp của **cron expressions** để định nghĩa lịch chạy công việc (job). Cron expressions gồm 7 trường (hoặc 6 nếu không dùng "year") và được viết theo định dạng:

```
<second> <minute> <hour> <day-of-month> <month> <day-of-week> <year>
```

## Ý Nghĩa của Các Trường

| Trường         | Giá trị hợp lệ                   | Mô tả                                                                 |
|-----------------|----------------------------------|----------------------------------------------------------------------|
| **Second**      | 0-59                            | Giây chạy job                                                        |
| **Minute**      | 0-59                            | Phút chạy job                                                        |
| **Hour**        | 0-23                            | Giờ chạy job                                                         |
| **Day of Month**| 1-31                            | Ngày trong tháng chạy job                                            |
| **Month**       | 1-12 hoặc JAN-DEC               | Tháng chạy job                                                       |
| **Day of Week** | 1-7 hoặc SUN-SAT                | Ngày trong tuần chạy job (1 = Chủ nhật)                              |
| **Year** (Tùy chọn) | Trống hoặc giá trị cụ thể (1970-2099) | Năm chạy job (ít dùng, có thể bỏ qua)                                |

## Các Ký Hiệu Đặc Biệt

| Ký hiệu | Ý nghĩa                                                                 |
|---------|-------------------------------------------------------------------------|
| `*`     | Đại diện cho mọi giá trị hợp lệ (VD: `*` trong cột giây: chạy mọi giây).|
| `?`     | Không xác định giá trị (dùng trong `day-of-month` hoặc `day-of-week`).  |
| `,`     | Liệt kê các giá trị (VD: `1,3,5` nghĩa là chạy vào giây 1, 3, 5).       |
| `-`     | Chỉ khoảng giá trị (VD: `10-20` nghĩa là từ giây 10 đến 20).            |
| `/`     | Chạy cách một khoảng (VD: `0/5` nghĩa là chạy mỗi 5 giây, bắt đầu từ 0).|
| `L`     | "Last" (VD: `L` trong `day-of-month` nghĩa là ngày cuối tháng).         |
| `W`     | Ngày làm việc gần nhất (VD: `15W` nghĩa là ngày làm việc gần nhất quanh ngày 15).|
| `#`     | Thứ tự trong tuần (VD: `2#3` nghĩa là thứ Hai tuần thứ 3 trong tháng).  |

---

## Ví Dụ CronSchedule

1. **Chạy mỗi 5 giây**:
   ```
   0/5 * * * * ? 
   ```
   - `0/5`: Bắt đầu từ giây 0, chạy mỗi 5 giây.
   - `*`: Mọi phút, giờ, ngày, tháng, năm.

2. **Chạy mỗi ngày lúc 10:30 sáng**:
   ```
   0 30 10 * * ?
   ```
   - `30`: Phút thứ 30.
   - `10`: Giờ thứ 10.
   - `*`: Mọi ngày, mọi tháng.
   - `?`: Không quan tâm ngày trong tuần.

3. **Chạy vào ngày 1 và ngày 15 hàng tháng lúc 12:00 trưa**:
   ```
   0 0 12 1,15 * ?
   ```
   - `1,15`: Ngày 1 và ngày 15.
   - `12`: Giờ thứ 12 (trưa).

4. **Chạy mỗi thứ Hai lúc 8:00 tối**:
   ```
   0 0 20 ? * 2
   ```
   - `20`: Giờ 20 (8 giờ tối).
   - `2`: Thứ Hai (theo ISO: 1 là Chủ nhật, 2 là thứ Hai).

5. **Chạy mỗi phút trong khoảng từ 9:00 sáng đến 9:59 sáng**:
   ```
   0 * 9 * * ?
   ```
   - `*`: Mọi phút.
   - `9`: Giờ thứ 9.

---
