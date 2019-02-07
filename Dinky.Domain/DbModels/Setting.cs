using Dinky.Infrastructure.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Dinky.Domain.Models
{
    public partial class Setting
    {
        [Key]
        public SettingType SettingType { get; set; }

        public string Value { get; set; }
    }
}
