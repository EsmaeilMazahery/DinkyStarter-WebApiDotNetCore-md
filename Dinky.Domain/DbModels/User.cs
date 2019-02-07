using System;

namespace Dinky.Domain.ViewModels
{
    /// <summary>
    /// نام ها درست شوند
    /// اتریبیوتها اضافه شود
    /// طول رشته ها استاندارد شود طبق ثابت ها
    /// کد به ای دی تبدیل شود
    /// مقدار های بیتی تبدیل شوند
    /// camelcase
    /// </summary>
    public class User
    {
        public int code { set; get; }
        public string name { set; get; }
        public string family { set; get; }
        public string userName { set; get; }
        public string pass { set; get; }

        public string des { set; get; }
        public int enab { set; get; }
        public int resselerCode { set; get; }
        public int show { set; get; }

        public string email { set; get; }
        public string melli { set; get; }
        public string phone { set; get; }
        public string mobile { set; get; }

        public DateTime birthday { set; get; }

        //ntext convert to nvarchar
        public string address { set; get; }
        public int credit { set; get; }
    }
}
