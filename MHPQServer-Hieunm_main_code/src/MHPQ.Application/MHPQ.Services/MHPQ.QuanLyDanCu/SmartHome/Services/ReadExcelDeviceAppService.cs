//using Abp.Application.Services;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using OfficeOpenXml;

//namespace MHPQ.Services
//{

//    public interface IReadExcelDeviceAppService : IApplicationService
//    {

//    }

//    public class ReadExcelDeviceAppService : MHPQAppServiceBase, IReadExcelDeviceAppService
//    {

//        public ReadExcelDeviceAppService()
//        {

//        }


//        public IEnumerable<ExcelData> GetDataFromExcelFile(string fileName, string sheetName)
//        {

//            try
//            {

//                //string HoTenCV = chuyenvien.Surname + " " + chuyenvien.Name;
//                var tempExcelPath = Path.Combine(GetUrlFileDefaut(), fileName);

//                var result = new List<ExcelData>();

//                var a = File.Exists(tempExcelPath);
//                int startRow = CheckExcelType(fileName, ref tempExcelPath);
//                using (ExcelPackage package = new ExcelPackage(new FileInfo(tempExcelPath)))
//                {
//                    // Khởi tạo Lấy Sheet đầu tiện trong file Excel để truy vấn, truyền vào name của Sheet để lấy ra sheet cần, nếu name = null thì lấy sheet đầu tiên
//                    ExcelWorksheet workSheet = package.Workbook.Worksheets[sheetName] ?? package.Workbook.Worksheets[1];
//                    //GetHeader
//                    var mapColumn = GetHeaderExcel(workSheet, startRow);
//                    // Đọc tất cả các row
//                    for (var rowNumber = startRow + 1; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
//                    {
//                        var birtday = CellToString(workSheet, workSheet.Cells[rowNumber, mapColumn.birthday.Value]);
//                        DateTime? birthDate = new DateTime();
//                        if (birtday != null)
//                        {
//                            birthDate = DateTime.ParseExact(birtday, "dd-MM-yyyy", null);
//                        }
//                        else birthDate = null;

//                        var value = new ExcelData()
//                        {
//                            StudentID = CellToString(workSheet, workSheet.Cells[rowNumber, mapColumn.StudentID.Value])
//                        };
//                        result.Add(value);

//                    }
//                }
//                return result;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }


//        public async Task<string> CreateUserFromExcel(string fileName, string sheetName)
//        {

//            try
//            {

//                //string HoTenCV = chuyenvien.Surname + " " + chuyenvien.Name;
//                var tempExcelPath = Path.Combine(GetUrlFileDefaut(), fileName);

//                var a = File.Exists(tempExcelPath);
//                int startRow = CheckExcelType(fileName, ref tempExcelPath);
//                using (ExcelPackage package = new ExcelPackage(new FileInfo(tempExcelPath)))
//                {
//                    // Khởi tạo Lấy Sheet đầu tiện trong file Excel để truy vấn, truyền vào name của Sheet để lấy ra sheet cần, nếu name = null thì lấy sheet đầu tiên
//                    ExcelWorksheet workSheet = package.Workbook.Worksheets[sheetName] ?? package.Workbook.Worksheets[1];
//                    //GetHeader
//                    var mapColumn = GetHeaderExcel(workSheet, startRow);
//                    // Đọc tất cả các row
//                    for (var rowNumber = startRow + 1; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
//                    {
//                        // Lấy 1 row trong excel để truy vấn

//                        var StudentID = CellToString(workSheet, workSheet.Cells[rowNumber, mapColumn.StudentID.Value]);
//                        var StudentName = CellToString(workSheet, workSheet.Cells[rowNumber, mapColumn.StudentName.Value]);
//                        var FullnameTeacher = CellToString(workSheet, workSheet.Cells[rowNumber, mapColumn.GVHD.Value]);
//                        var TeacherName = RemoveVietnameseTone(FullnameTeacher);
//                        var MaLopHoc = CellToString(workSheet, workSheet.Cells[rowNumber, mapColumn.classid.Value]);
//                        var MaHocPhan = CellToString(workSheet, workSheet.Cells[rowNumber, mapColumn.courseid.Value]);
//                        var ClassName = CellToString(workSheet, workSheet.Cells[rowNumber, mapColumn.name.Value]);

//                        if (StudentID != null && StudentName != null)
//                        {

//                            var existingUser = await _userManager.FindByNameAsync(StudentID);
//                            if (existingUser == null)
//                            {
//                                ApplicationUser user = new ApplicationUser()
//                                {
//                                    FullName = StudentName,
//                                    UserName = StudentID

//                                };
//                                var result = await _userManager.CreateAsync(user, user.UserName + "Hust#");
//                                if (result.Succeeded)
//                                {
//                                    var role = new Role();
//                                    role.Name = "Student";
//                                    bool studentRoleExists = await _roleManager.RoleExistsAsync("Student");
//                                    if (!studentRoleExists)
//                                    {
//                                        var roleResult = await _roleManager.CreateAsync(role);
//                                    }
//                                    await _userManager.AddToRoleAsync(user, "Student");

//                                    var userprofile = new UserProfile();
//                                    userprofile.UserId = user.Id;
//                                    userprofile.FullName = user.FullName;
//                                    userprofile.RoleName = "Student";
//                                    _userProfileRepos.Add(userprofile);
//                                }

//                            }
//                        }



//                        if (FullnameTeacher != null)
//                        {
//                            TeacherName = TeacherName.ToUpper();

//                            var existingUser = await _userManager.FindByNameAsync(TeacherName);
//                            if (existingUser == null)
//                            {
//                                ApplicationUser user = new ApplicationUser()
//                                {
//                                    FullName = FullnameTeacher,
//                                    UserName = TeacherName

//                                };

//                                var result = await _userManager.CreateAsync(user, user.UserName.Replace(" ", "") + "Cntt2#");
//                                if (result.Succeeded)
//                                {
//                                    var role = new Role();
//                                    role.Name = "Teacher";
//                                    bool studentRoleExists = await _roleManager.RoleExistsAsync("Teacher");
//                                    if (!studentRoleExists)
//                                    {
//                                        var roleResult = await _roleManager.CreateAsync(role);
//                                    }
//                                    await _userManager.AddToRoleAsync(user, "Teacher");

//                                    var userprofile = new UserProfile();
//                                    userprofile.UserId = user.Id;
//                                    userprofile.FullName = user.FullName;
//                                    userprofile.RoleName = "Teacher";
//                                    _userProfileRepos.Add(userprofile);
//                                }

//                            }
//                        }
//                        var TeacherId = _userManager.FindByNameAsync(TeacherName);
//                    }

//                }

//                return null;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }
//        public string GetUrlFileDefaut()
//        {
//            string ATTP_FILE_PDF = ConfigurationManager.AppSettings["QLDA"];
//            if (string.IsNullOrEmpty(ATTP_FILE_PDF))
//            {
//                ATTP_FILE_PDF = @"C:\QLDA";
//            }

//            if (!Directory.Exists(ATTP_FILE_PDF))
//            {
//                Directory.CreateDirectory(ATTP_FILE_PDF);
//            }
//            return ATTP_FILE_PDF;

//        }

//        private string CellToString(ExcelWorksheet ws, ExcelRange er)
//        {
//            // lấy value của cell, nếu cell merger lấy giá trị cell đầu tiên
//            try
//            {
//                dynamic runTimeObject = new ExpandoObject();
//                runTimeObject.Name = 1;
//                runTimeObject.Value = "2";
//                string ret = null;
//                var sttaddress = er.GetMergedRangeAddress();
//                string[] splAddress = sttaddress.Split(':');
//                if (ws.Cells[splAddress[0]].Value != null)
//                {
//                    ret = ws.Cells[splAddress[0]].Value.ToString() ?? string.Empty;
//                    return ret.Trim();
//                }
//                return ret;
//            }
//            catch
//            {
//                return null;
//            }

//        }

//        private int CheckExcelType(string fileName, ref string pathOut)
//        {
//            int startRow = 1;
//            if (fileName.ToLower().EndsWith(".xls"))
//            {
//                string pathNew = @"C:\QLDA";
//                AppFileHelper.DeleteFilesInFolderIfExists(pathNew, fileName + "x");
//                var xlsFile = Path.Combine(pathNew, fileName);
//                var xlsxFile = xlsFile + "x";

//                FileStream stream = File.Open(xlsFile, FileMode.Open, FileAccess.Read);
//                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
//                DataSet result = excelReader.AsDataSet();

//                using (ExcelPackage epackage = new ExcelPackage())
//                {
//                    ExcelWorksheet excel = epackage.Workbook.Worksheets.Add("ExcelTabName");
//                    DataTable dtbl = result.Tables[0];
//                    excel.Cells["A1"].LoadFromDataTable(dtbl, true);
//                    System.IO.FileInfo file = new System.IO.FileInfo(@"" + xlsxFile);
//                    epackage.SaveAs(file);
//                }
//                excelReader.Close();
//                startRow = 2;
//                pathOut = Path.Combine(pathNew, fileName + "x");
//            }
//            return startRow;
//        }

//        private MapColumn GetHeaderExcel(ExcelWorksheet workSheet, int startRow)
//        {
//            MapColumn ret = new MapColumn();
//            foreach (var firstRowCell in workSheet.Cells[startRow, 1, 1, workSheet.Dimension.End.Column])
//            {
//                switch (firstRowCell.Text)
//                {
//                    case "classid":
//                        ret.classid = firstRowCell.Start.Column;
//                        break;
//                    case "name":
//                        ret.name = firstRowCell.Start.Column;
//                        break;
//                    case "courseid":
//                        ret.courseid = firstRowCell.Start.Column;
//                        break;
//                    case "SectionType":
//                        ret.SectionType = firstRowCell.Start.Column;
//                        break;
//                    case "note":
//                        ret.note = firstRowCell.Start.Column;
//                        break;
//                    case "StudentID":
//                        ret.StudentID = firstRowCell.Start.Column;
//                        break;
//                    case "studentname":
//                        ret.StudentName = firstRowCell.Start.Column;
//                        break;
//                    case "Tên đề tài":
//                        ret.TopicName = firstRowCell.Start.Column;
//                        break;
//                    case "birthdate":
//                        ret.birthday = firstRowCell.Start.Column;
//                        break;
//                    case "groupname":
//                        ret.GroupName = firstRowCell.Start.Column;
//                        break;
//                    case "termid":
//                        ret.termid = firstRowCell.Start.Column;
//                        break;
//                    case "BM":
//                        ret.BM = firstRowCell.Start.Column;
//                        break;
//                    case "GVHD":
//                        ret.GVHD = firstRowCell.Start.Column;
//                        break;
//                    case "Trạng thái":
//                        ret.trangthai = firstRowCell.Start.Column;
//                        break;
//                    case "Lớp":
//                        ret.ClassName = firstRowCell.Start.Column;
//                        break;
//                    case "Thông tin":
//                        ret.ThongTin = firstRowCell.Start.Column;
//                        break;
//                    default:
//                        break;
//                }
//            }
//            return ret;
//        }


//        public string RemoveVietnameseTone(string text)
//        {
//            if (text != null)
//            {
//                string result = text.ToLower();
//                result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
//                result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
//                result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
//                result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
//                result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
//                result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
//                result = Regex.Replace(result, "đ", "d");
//                return result;
//            }
//            else return null;

//        }

//    }
//}
