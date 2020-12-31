# CrudGenerator
Tự động tạo thủ tục CRUD theo mô hình ORM

## Usage
### 1. Generate code
- Copy 3 project __CrudCoreSystem__, __CrudGenerator__ và __DataAccess__ vào project cần sử dụng.
- Import 3 project đã copy vào project mới, set __CrudGenerator__ làm Startup Project.
- Chạy project, điền __Connection string__ của DB sử dụng sau đó nhấn __Connect__. Đoạn code SQL tạo các thủ tục CRUD sẽ được hiển thị tại textbox phía dưới.
- Copy đoạn code SQL phía trên sau đó chạy trong SSMS để tạo các thủ tục CRUD cần thiết.

### 2. Import class
- Mở project __DataAccess__ trong __Solution Explorer__, nhấn __Show All Files__, include các thư mục: __DatabaseConfig__, __DataManipulation__, __DataModel__, __DataRepository__.

### 3. Test
- Add reference hai project __DataAccess__ và __CrudCoreSystem__ vào project test, sau đó sử dụng các phương thức của class __DataRepository__ để thao tác dữ liệu.
#### 3.1. Create
```c#
HoSoThauDataRepository repo = new HoSoThauDataRepository();
//Tạo dữ liệu bảng HoSoThau
//Không truyền tham số HoSoID do cột HoSoID là ID tự tăng
HoSoThau model = new HoSoThau
{
    TenGoiThau = "Test CRUD",
    CreatedBy = "Admin",
    CreatedDate = DateTime.Now
};
repo.Create(model);
if (repo.ReturnCode == 0) //Thao tác thành công
{
    int returnId = repo.ReturnData; //Lấy về ID của bản ghi vừa thêm mới
    MessageBox.Show("Thêm mới dữ liệu thành công");
}
else //Có lỗi
    MessageBox.Show("Không thể truy vấn dữ liệu. Lỗi: " + repo.ReturnMess);
