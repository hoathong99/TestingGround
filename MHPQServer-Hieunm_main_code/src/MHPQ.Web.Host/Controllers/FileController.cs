using Abp;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using MHPQ.Chat;
using MHPQ.Common.DataResult;
using MHPQ.Configuration;
using MHPQ.Controllers;
using MHPQ.RoomChats;
using MHPQ.Web.Host.Chat;
using MHPQ.Web.Host.Common;
using MHPQ.Web.Host.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace MHPQ.Web.Host.Controllers
{
    public class FileController: MHPQControllerBase
    {
        private readonly IAbpSession _session;
        private static IHubContext<ChatHub> ChatHub;
        private readonly IRoomChatManager _roomChatManager;
        private readonly IChatMessageManager _chatMessageManager;
        private readonly IConfigurationRoot _appConfiguration;

        public FileController(
            IAbpSession session,
            IHubContext<ChatHub> chatHub,
            IRoomChatManager roomChatManager,
            IChatMessageManager chatMessageManager,
            IWebHostEnvironment env

        )
        {
            _session = session;
            ChatHub = chatHub;
            _roomChatManager = roomChatManager;
            _chatMessageManager = chatMessageManager;
            _appConfiguration = env.GetAppConfiguration();
        }


        [HttpGet]
        [Route("downfile/{filePath}")]
        public IActionResult DownloadFile(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }
                var FileType = "";
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, FileType, filePath);
            }
            catch (Exception ex)
            {
                return Ok("");
            }
        }

        [HttpPost]
        [Route("uploadimagepublic")]
        public async Task<string> UploadFile()
        {
            try
            {
                var request = await Request.ReadFormAsync();
                //Check input
               
                var ufile = request.Files[0];
                var profile = request["profile"].ToString();
                //var hocKy = request["hocKy"].ToString();
                if (ufile != null && ufile.Length > 0)
                {
                    //var userId = _session.UserId;
                    var fileName = Path.GetFileName(ufile.FileName);
                    var pathName = "User" + _session.UserId.ToString();
                    if(profile != null && profile != "")
                    {
                        pathName = pathName + @"/profileUser";
                    }
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", pathName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    var filePath = Path.Combine(folderPath, fileName);
                    AppFileHelper.DeleteFilesInFolderIfExists(folderPath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ufile.CopyToAsync(fileStream);
                    }
                    var result = _appConfiguration["App:ServerRootAddress"] + @"images/" + pathName + @"/" + fileName;
                    return result;
                }
                return null;
            }
            catch(Exception ex)
            {
                Logger.Error($"{DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy")} CheckFilePdfValid {ex.Message} {JsonConvert.SerializeObject(ex)}");
                return null;
            }
        }

        [HttpPost]
        [Route("uploadlistimagepublic")]
        public async Task<object> UploadListFile()
        {
            try
            {
                var request = await Request.ReadFormAsync();
                //Check input

                var ufiles = request.Files;

                
                if (ufiles != null && ufiles.Count > 0)
                {
                    var result = new List<string>();
                    foreach (var ufile in ufiles)
                    {
                        var fileName = Path.GetFileName(ufile.FileName);
                        var pathName = "User_" + _session.UserId.ToString();
                      
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", pathName);

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        var filePath = Path.Combine(folderPath, fileName);
                        AppFileHelper.DeleteFilesInFolderIfExists(folderPath, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ufile.CopyToAsync(fileStream);
                        }
                        var url = _appConfiguration["App:ServerRootAddress"] + @"images/" + pathName + @"/" + fileName;
                        result.Add(url);
                    }
                    return result;
                }



               
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"{DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy")} CheckFilePdfValid {ex.Message} {JsonConvert.SerializeObject(ex)}");
                return null;
            }
        }

        [HttpPost]
        [Route("uploadfile")]
        public async Task<object> UploadFilePublic()
        {
            try
            {
                var request = await Request.ReadFormAsync();
                var userId = _session.UserId;
                //Check input
                if (request.Files.Count <= 0 || request.Files[0] == null)
                {
                    return new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                var file = request.Files[0];
                var filePath = CreateDirectories(file);
                var result = new FileDataResult(file.ContentType, filePath);
                var data = DataResult.ResultSucces(result, "Upload thành công");
                return data;
            }
            catch (Exception ex)
            {
                var data = (ex.ToString(), "Có lỗi");
                return Ok(data);
            }
        }

        [HttpPost]
        [Route("UploadImageGroupChat")]
        public async Task<object> UploadImageGroupChat([FromForm] UploadFileRoomChatInput input)
        {
            try
            {
                var request = await Request.ReadFormAsync();
                var userId = _session.UserId;
                //Check input
                if (request.Files.Count <= 0 || request.Files[0] == null)
                {
                    return new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }
                var file = request.Files[0];
                var pathfile = CreateDirectories(file);
                var result = new FileDataResult(file.ContentType, pathfile);
                var data = DataResult.ResultSucces(result, "Upload thành công");
                var fileBytes = System.IO.File.ReadAllBytes(pathfile);
                //  var roomChatCode = _roomChatManager.GetRoomChat(input.RoomId);
                string base64String = Convert.ToBase64String(fileBytes);
                base64String = "data:image/jpeg;base64," + base64String;
                string htmlImage = string.Format("<img  class=\"post-image\" src=\"{0}\" />", base64String);
                return htmlImage;
            }
            catch (Exception ex)
            {
                var data = (ex.ToString(), "Có lỗi");
                return Ok(data);
            }
        }

        [HttpPost]
        [Route("UploadImageUserChat")]
        public async Task<object> UploadImageUserChat([FromForm] UploadImageUserChatInput input)
        {
            try
            {
                var request = await Request.ReadFormAsync();
                var userId = _session.UserId;
                var sender = _session.ToUserIdentifier();
                var receiver = new UserIdentifier(input.TenantId, Int32.Parse(input.UserId));
                //Check input
                if (input.File == null)
                {
                    return new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }
                var file = input.File;
                var pathfile = CreateDirectories(file);
                var result = new FileDataResult(file.ContentType, pathfile);
                var fileBytes = System.IO.File.ReadAllBytes(pathfile);
                string base64String = Convert.ToBase64String(fileBytes);
                base64String = "data:image/jpeg;base64," + base64String;
                string htmlImage = string.Format("<img class=\"post-image\" src=\"{0}\" />", base64String);

                _chatMessageManager.SendMessage(sender, receiver, htmlImage, input.TenancyName, input.UserName, input.ProfilePictureId);
                return string.Empty;

            }
            catch (Exception ex)
            {
                var data = (ex.ToString(), "Có lỗi");
                return Ok(data);
            }
        }

        public string CreateDirectories(IFormFile file)
        {
            var userId = _session.UserId;
            //Check input

            var fileName = file.FileName;
            var type = file.ContentType;
            string[] arr = new string[5] { ".png", ".jpg",".docx",".xlsx",".pdf" };
            var checkErrorFile = CheckFilePdfValid(file, 15145728, arr);

            if (checkErrorFile == 1)
            {
                return "Dung lượng quá lớn";
            }
            if (checkErrorFile == 2)
            {
                return "Sai định dạng";
            }
            if (checkErrorFile == 0)
            {
                return "Có lỗi !";
            }

            var fileNameNotExtension = Path.GetFileNameWithoutExtension(file.FileName);
            fileName = HttpUtility.UrlEncode(fileName);

            string baseUrl = GetUrlFileDefaut();
            var pathName = "UserId" + userId.ToString() + @"\" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();

            var folderPath = Path.Combine(baseUrl, pathName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            AppFileHelper.DeleteFilesInFolderIfExists(folderPath, fileName);


            var userFile = Path.Combine(folderPath, fileName);
            using (Stream fileStream = new FileStream(userFile, FileMode.Create))
            {
               file.CopyToAsync(fileStream);
            }
            return folderPath + @"\" + fileName;
        }

        public string GetUrlFileDefaut()
        {
            string MHPQ_FILE_PDF = ConfigurationManager.AppSettings["MHPQ"];
            if (string.IsNullOrEmpty(MHPQ_FILE_PDF))
            {
                MHPQ_FILE_PDF = @"C:\MHPQ_FILE";
            }

            if (!Directory.Exists(MHPQ_FILE_PDF))
            {
                Directory.CreateDirectory(MHPQ_FILE_PDF);
            }
            return MHPQ_FILE_PDF;

        }

        private int CheckFilePdfValid(IFormFile input, int sizeLimit, string[] typeExcepted)
        {
            try
            {
                var fileExtension = Path.GetExtension(input.FileName).ToLower();
                if (input.Length > sizeLimit)
                {
                    return 1;
                }
                if (!typeExcepted.Any(fileExtension.Contains))
                {
                    return 2;
                }
                return 3;
            }
            catch (Exception ex)
            {
                Logger.Error($"{DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy")} CheckFilePdfValid {ex.Message} {JsonConvert.SerializeObject(ex)}");
                return 0;
            }
        }
    }
}
