using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password is at least 6 character");
            // -> ta có thể throw ra các câu thông báo khác nhau tùy từng trường hợp

            //RememberMe là kiểu boolean, nó đã được validate mặc định bởi swagger
            //nếu không chọn thì nó sẽ mặc định là false
        }
    }
}