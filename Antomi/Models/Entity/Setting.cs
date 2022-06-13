using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Setting
    {
        public int Id { get; set; }
        public string LogoImage { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public string PlaystoreUrl { get; set; }
        public string AppstoreUrl { get; set; }
    }

    public class SettingValidation : AbstractValidator<Setting>
    {
        public SettingValidation()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.Phone).NotEmpty().NotNull();
            RuleFor(x => x.Address).NotEmpty().NotNull();
            RuleFor(x => x.FacebookUrl).NotEmpty().NotNull();
            RuleFor(x => x.TwitterUrl).NotEmpty().NotNull();
            RuleFor(x => x.TwitterUrl).NotEmpty().NotNull();
            RuleFor(x => x.InstagramUrl).NotEmpty().NotNull();
            RuleFor(x => x.LinkedinUrl).NotEmpty().NotNull();
            RuleFor(x => x.PlaystoreUrl).NotEmpty().NotNull();
            RuleFor(x => x.AppstoreUrl).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(1000);
        }
    }
}
