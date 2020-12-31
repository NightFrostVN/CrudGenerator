# CrudGenerator
Tự động tạo thủ tục CRUD theo mô hình ORM

## Usage
### 1. Generate code
- Copy 3 project __CrudCoreSystem__, __CrudGenerator__ và __DataAccess__ vào project cần sử dụng.

![alt text](https://raw.githubusercontent.com/NightFrostVN/CrudGenerator/master/Screenshot/Screenshot_1.jpg)

- Import 3 project đã copy vào project mới, set __CrudGenerator__ làm __Startup Project__.

![alt text](https://raw.githubusercontent.com/NightFrostVN/CrudGenerator/master/Screenshot/Screenshot_2.jpg)

- Chạy project, điền __Connection string__ của DB sử dụng sau đó nhấn __Connect__. Đoạn code SQL tạo các thủ tục CRUD sẽ được hiển thị tại textbox phía dưới.

![alt text](https://raw.githubusercontent.com/NightFrostVN/CrudGenerator/master/Screenshot/Screenshot_3.jpg)

- Copy đoạn code SQL phía trên sau đó chạy trong SSMS để tạo các thủ tục CRUD cần thiết.

![alt text](https://raw.githubusercontent.com/NightFrostVN/CrudGenerator/master/Screenshot/Screenshot_4.jpg)

### 2. Import
- Mở project __DataAccess__ trong __Solution Explorer__, nhấn __Show All Files__, include các thư mục: __DatabaseConfig__, __DataManipulation__, __DataModel__, __DataRepository__.

![alt text](https://raw.githubusercontent.com/NightFrostVN/CrudGenerator/master/Screenshot/Screenshot_5.jpg)

### 3. Use
- Add reference hai project __DataAccess__ và __CrudCoreSystem__ vào project cần sử dụng, sau đó sử dụng các hàm của class __DataRepository__ để thao tác dữ liệu.

![alt text](https://raw.githubusercontent.com/NightFrostVN/CrudGenerator/master/Screenshot/Screenshot_6.jpg)

#### 3.1. Create
```c#
HoSoThauDataRepository repo = new HoSoThauDataRepository();
//Tạo dữ liệu bảng HoSoThau
//Không truyền tham số HoSoID do cột HoSoID là ID tự tăng
HoSoThau model = new HoSoThau
{
    TenGoiThau = "Test CRUD",
    BPQLThauID = 1,
    LoaiHinhThauID = 1,
    ThoiGianMo = DateTime.Now,
    ThoiGianDong = DateTime.Now,
    IsActive = 1,
    Status = 1,
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
```
#### 3.2. Read
```c#
HoSoThauDataRepository repo = new HoSoThauDataRepository();
//Lấy toàn bộ dữ liệu bảng HoSoThau
List<HoSoThau> list = repo.Read(new HoSoThau());
//Lấy list bản ghi bảng HoSoThau có HoSoID = 1 và TenGoiThau like N'test'
List<HoSoThau> list1 = repo.Read(new HoSoThau { HoSoID = 6, TenGoiThau = "test" });
```
#### 3.3. Update
- Với những bảng không có cột ID tự tăng sẽ __không có hàm Update__.
```c#
HoSoThauDataRepository repo = new HoSoThauDataRepository();
//Lấy bản ghi cần update, vd cần lấy bản ghi có HoSoID = 6
HoSoThau model = repo.Read(new HoSoThau { HoSoID = 6})[0];
//Sửa nội dung
model.TenGoiThau = "Update title";
model.ModifiedBy = "Admin";
model.ModifiedDate = DateTime.Now;
repo.Update(model);
if (repo.ReturnCode == 0) //Thao tác thành công
    MessageBox.Show("Cập nhật dữ liệu thành công");
else //Có lỗi
    MessageBox.Show("Không thể truy vấn dữ liệu. Lỗi: " + repo.ReturnMess);
```
#### 3.4. Delete
- Với những bảng không có cột ID tự tăng sẽ __không có hàm Delete__.
- Với những bảng có trường __Status__ thì sẽ update __Status = -1__, còn không sẽ __xóa bản ghi__ của bảng đó.
```c#
//Lấy bản ghi cần delete, vd cần lấy bản ghi có HoSoID = 6
HoSoThau model = repo.Read(new HoSoThau { HoSoID = 6})[0];
repo.Delete(model);
if (repo.ReturnCode == 0) //Thao tác thành công
    MessageBox.Show("Xóa dữ liệu thành công");
else //Có lỗi
    MessageBox.Show("Không thể truy vấn dữ liệu. Lỗi: " + repo.ReturnMess);
```
#### 3.5. Custom query
- Khai báo thêm hàm trong class __DataRepository__ của bảng. Sử dụng hàm ``ExecuteProcedure(string procedureName, List<SqlParameter> listParam, bool isReturnDataTable)`` để truy vấn dữ liệu.
```c#
public class HoSoThauRepository : HoSoThauDataManipulation
{
    //Chạy thủ tục sp_HoSoThau_AutoUpdateStatus
    public void AutoUpdateStatus()
    {
        ExecuteProcedure("sp_HoSoThau_AutoUpdateStatus", null, false);
    }

    //Chạy thủ tục sp_HoSoThau_SaveLog với param
    public void SaveLog(int hoSoId)
    {
        List<SqlParameter> listParam = new List<SqlParameter>();
        listParam.Add(new SqlParameter("@HoSoID", Convert.ToInt32(hoSoId)));
        SqlParameter returnCode = new SqlParameter("@ReturnCode", SqlDbType.Int);
        returnCode.Direction = ParameterDirection.Output;
        listParam.Add(returnCode);
        SqlParameter returnMess = new SqlParameter("@ReturnMess", SqlDbType.NVarChar, 500);
        returnMess.Direction = ParameterDirection.Output;
        listParam.Add(returnMess);
        ExecuteProcedure("sp_HoSoThau_SaveLog", listParam, false);
    }

    //Chạy thủ tục sp_HoSoThau_GetLichSuGiaHan, trả về kết quả query
    public DataTable GetLichSuGiaHan(int hoSoId)
    {
        List<SqlParameter> listParam = new List<SqlParameter>();
        listParam.Add(new SqlParameter("@HoSoID", Convert.ToInt32(hoSoId)));

        ExecuteProcedure("sp_HoSoThau_GetLichSuGiaHan", listParam, true);
        return returnDataTable;
    }
}
```
```c#
//Sử dụng tại view
HoSoThauRepository repo = new HoSoThauRepository();
repo.AutoUpdateStatus();
if (repo.ReturnCode == 0) //Thao tác thành công
    MessageBox.Show("Truy vấn dữ liệu thành công");
else //Có lỗi
    MessageBox.Show("Không thể truy vấn dữ liệu. Lỗi: " + repo.ReturnMess);
    
DataTable dtLichSuGiaHan = repo.GetLichSuGiaHan(Convert.ToInt32(Request.Params["id"]));
```

## Known issues

## To-do list
- Tìm kiếm dữ liệu theo kiểu từ ngày - đến ngày.
- Thêm tính năng sử dụng trên nhiều DB.
- Update tài liệu đặc tả.
- Update chức năng.
