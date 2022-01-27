using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using MHPQ.Chat;
using MHPQ.MultiTenancy;
using MHPQ.Web;
using MHPQ.Emailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHPQ.Authorization.Users
{
    public class UserEmailer : MHPQServiceBase, IUserEmailer, ITransientDependency
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly IWebUrlService _webUrlService;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;

        public UserEmailer(IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender,
            IWebUrlService webUrlService,
            IRepository<Tenant> tenantRepository,
            ICurrentUnitOfWorkProvider unitOfWorkProvider)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _webUrlService = webUrlService;
            _tenantRepository = tenantRepository;
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        [UnitOfWork]
        public virtual async Task SendEmailActivationLinkAsync(User user, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new ApplicationException("EmailConfirmationCode should be set in order to send email activation link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);

            var link = _webUrlService.GetSiteRootAddress(tenancyName) + "Account/EmailConfirmation" +
                       "?userId=" + Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.Id.ToString())) +
                       "&tenantId=" + (user.TenantId == null ? "" : Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.TenantId.Value.ToString()))) +
                       "&confirmationCode=" + Uri.EscapeDataString(user.EmailConfirmationCode);

            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(user.TenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", L("EmailActivation_Title"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("EmailActivation_SubTitle"));

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Surname + " " + user.Name + "<br />");

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            if (!plainPassword.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("Password") + "</b>: " + plainPassword + "<br />");
            }

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("EmailActivation_ClickTheLinkBelowToVerifyYourEmail") + "<br /><br />");
            mailMessage.AppendLine("<a href=\"" + link + "\">" + link + "</a>");

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            await _emailSender.SendAsync(user.EmailAddress, L("EmailActivation_Subject"), emailTemplate.ToString());
        }

        public async Task SendEmailUyQuyenAsync(string EmailUyQuyen, string TenDoanhNghiep, string TendonViUyQuyen, string DuongDanCvUyQuyen, int? TenantId)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(TenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", "");
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", "");
            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("Kính gửi : " + TendonViUyQuyen + "<br />");
            string NoiDungThongBao = "Cục ATTP đã tiến hành xem xét hồ sơ đăng ký cấp giấy chứng nhận cơ sở đủ điều kiện của doanh nghiêp " + TenDoanhNghiep +
                " . Qua quá trình xem xét cục ATTP thấy hồ sơ của doanh nghiệp đã đạt yêu cầu và có thể tiến hành thẩm định cơ sở . " + "<br />" +
                "Cục ATTP ủy quyền cho " + TendonViUyQuyen + " thẩm đinh cơ sở . " + TendonViUyQuyen +
                " tiến hành thẩm định cơ sở theo hướng dẫn trong công văn ủy quyền số : " + "<br />";
            mailMessage.AppendLine(NoiDungThongBao);
            mailMessage.AppendLine("Bấm vào link bên dưới để xem công văn ủy quyền : " + "<br />");
            DuongDanCvUyQuyen = _webUrlService.GetSiteRootAddress() + "/File/GoToViewTaiLieu?url=" + DuongDanCvUyQuyen;
            mailMessage.AppendLine("<a href=\"" + DuongDanCvUyQuyen + "\">" + DuongDanCvUyQuyen + "</a>");

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            await _emailSender.SendAsync(EmailUyQuyen, "Công văn ủy quyền", emailTemplate.ToString());
        }

        /// <summary>
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        public async Task SendPasswordResetLinkAsync(User user)
        {
            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new ApplicationException("PasswordResetCode should be set in order to send password reset link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);

            var link = _webUrlService.GetSiteRootAddress(tenancyName) + "Account/ResetPassword" +
                       "?userId=" + Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.Id.ToString())) +
                       "&tenantId=" + (user.TenantId == null ? "" : Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.TenantId.Value.ToString()))) +
                       "&resetCode=" + Uri.EscapeDataString(user.PasswordResetCode);

            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(user.TenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", L("PasswordResetEmail_Title"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("PasswordResetEmail_SubTitle"));

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("PasswordResetEmail_ClickTheLinkBelowToResetYourPassword") + "<br /><br />");
            mailMessage.AppendLine("<a href=\"" + link + "\">" + link + "</a>");

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            await _emailSender.SendAsync(user.EmailAddress, L("PasswordResetEmail_Subject"), emailTemplate.ToString());
        }

        public void TryToSendChatMessageMail(User user, string senderUsername, string senderTenancyName, ChatMessage chatMessage)
        {
            try
            {
                var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(user.TenantId));
                emailTemplate.Replace("{EMAIL_TITLE}", L("NewChatMessageEmail_Title"));
                emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("NewChatMessageEmail_SubTitle"));

                var mailMessage = new StringBuilder();
                mailMessage.AppendLine("<b>" + L("Sender") + "</b>: " + senderTenancyName + "/" + senderUsername + "<br />");
                mailMessage.AppendLine("<b>" + L("Time") + "</b>: " + chatMessage.CreationTime.ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + chatMessage.Message + "<br />");
                mailMessage.AppendLine("<br />");

                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

                _emailSender.Send(user.EmailAddress, L("NewChatMessageEmail_Subject"), emailTemplate.ToString());
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        private string GetTenancyNameOrNull(int? tenantId)
        {
            if (tenantId == null)
            {
                return null;
            }

            using (_unitOfWorkProvider.Current.SetTenantId(null))
            {
                return _tenantRepository.Get(tenantId.Value).TenancyName;
            }
        }
    }
}
