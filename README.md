# EmployeeManagement
API được viết bằng `.NET6`, có `truy vấn SQLserver` và áp dụng kiến thức `Lập trình hướng đối tượng` 

1 dự án ví dụ về API, sử dụng middleware để nhận các yêu cầu (request) và trả về phản hồi (respone) sử dụng ADO kết nối SQL, thực thi thêm, xóa, sửa (CRUD) 

Thay đổi thông tin chuỗi kết nối (connectionString) ở: Models/DbContext.cs - dòng 11
  để phù hợp với CSDL muốn kết nối

Mẫu thiết kế CSDL:

-- CREATE database Manager 

use Manager 

CREATE TABLE Employee 

( 

NvID INT IDENTITY(1,1) PRIMARY KEY, 

FirstName NVARCHAR(255) NULL, 

LastName NVARCHAR(255) NULL, 

) 

INSERT into Employee (FirstName, LastName) VALUES (N'Sở', N'Khanh');  
INSERT into Employee (FirstName, LastName) VALUES(N'Thúy', N'Kiều');  
INSERT into Employee (FirstName, LastName) VALUES(N'kim', N'Trọng'); 

Khi chạy `dotnet watch run` truy cập vào đường dẫn `https://localhost:7216/swagger/` để kiểm tra các API có hoạt động hay không 

Trên đây là 1 ví dụ nhỏ về WebAPI, ngoài ra em cũng có biết về MVC và Single-page, xin a/chị đánh giá và cho em 1 cơ hội việc làm ạ. Em cảm ơn rất nhiều!
