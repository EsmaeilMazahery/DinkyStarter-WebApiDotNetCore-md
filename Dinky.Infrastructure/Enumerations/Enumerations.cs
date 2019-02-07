
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Dinky.Infrastructure.Models;

namespace Dinky.Infrastructure.Enumerations
{

    [AttributeUsage(AttributeTargets.All)]
    public class ValueAttribute : DescriptionAttribute
    {
        public ValueAttribute(object value)
        {
            this.Value = value;
        }

        public object Value { get; set; }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class NameAttribute : DescriptionAttribute
    {
        public NameAttribute(string Name)
        {
            this.Name = Name;
        }

        public string Name { get; set; }

        public static implicit operator string(NameAttribute c)
        {
            return c.Name == null ? "" : c.Name;
        }
    }

    public static class Extensions
    {
        public static object GetValue(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<ValueAttribute>().Value;
        }

        public static object GetName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<NameAttribute>().Name;
        }
    }

    public static class Enum<T> where T : struct
    {
        /// <summary>
        /// آیا این مقدار در اینام تعریف شده یا نه
        /// </summary>
        public static bool Has(byte value)
        {
            return Enum.IsDefined(typeof(T), value);
        }

    }

    public enum DeviceType : byte
    {
        WebUser = 1,
        WebCustomer = 2,
        WebAdmin = 3,
        Android = 4,
        Ios = 5
    }
    

    public static class CustomEnumConverter
    {
        /// <summary>
        /// Get description attribute value of this enum item or return null.
        /// </summary>
        public static string GetDescriptionOrNull<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return null;
        }

        /// <summary>
        /// Get description attribute value of this enum item or string of value.
        /// </summary>
        public static string GetDescriptionOrDefault<T>(this T enumerationValue) where T : struct
        {
            string temp = enumerationValue.GetDescriptionOrNull();
            if (string.IsNullOrWhiteSpace(temp))
                return enumerationValue.ToString();
            else
                return temp;
        }
    }
    public enum AdminSmsMessageSendType : byte
    {
        [Description("فراموشی رمزعبور")]
        RememberPassword = 1
    }
    public enum PropertiseKey : byte
    {
        [DefaultValue("")]
        [Description("شماره پنل پیامک")]
        SmsPanelNumber = 1,
        [DefaultValue("")]
        [Description("نام کاربری پنل پیامک")]
        SmsPanelUsername = 2,
        [DefaultValue("")]
        [Description("گذرواژه پنل پیامک")]
        SmsPanelPassword = 3
    }
    public enum PropertiseType : byte
    {
        [Description("String")]
        String = 1,
        [Description("Int")]
        Int = 2,
        [Description("Float")]
        Float = 3,
        [Description("DateTime")]
        DateTime = 4,
        [Description("DropDown")]
        DropDown = 5,
        [Description("CheckList")]
        CheckList = 6,
        [Description("Radio")]
        Radio = 7,
    }

    public enum SettingType : byte
    {
    }


    public enum JqueryResultType : byte
    {
        Default = 0,
        Success = 1,
        Info = 2,
        Warning = 3,
        Danger = 4
    }

    public enum ErrorType : byte
    {
        Default = 1,
        NotFound = 2,
        ModelState = 3,
        AccountExpired = 4,
    }

    public enum SmsSendStatus : byte
    {
        [Description("ارسال شد")]
        Sent = 1,

        [Description("شماره گیرنده خالی است")]
        ReceiverNumberEmpty = 2,

        [Description("متن پیام خالی است")]
        MessageTextEmpty = 3,

        [Description("نام کاربری یا رمز عبور صحیح نیست")]
        InvalidUsernameOrPassword = 4,

        [Description("اعتبار حساب کافی نیست")]
        CreditLow = 5,

        [Description("حساب کاربر فعال نیست")]
        UserDeactive = 6,

        [Description("شماره گیرنده معتبر نیست")]
        InvalidReceiverNumber = 7,

        [Description("شماره فرستنده معتبر نیست")]
        InvalidSenderNumber = 8,

        [Description("هیچ شماره ای به حساب شما اختصاص نیافته است")]
        InvalidNumber = 9,

        [Description("متن پیام خالی است")]
        MessageTextLenght = 10,

        [Description("خطا در سرویس دهنده")]
        ServiceError = 11,

        [Description("خطا در برقراری ارتباط با سوییچ مخابرات")]
        ErrorCommunicatingToSwitchTelecommunications = 12,

        [Description("شناسه پیام نامعتبر است")]
        InvalidMessageID = 13,

        [Description("آدرس انتقال ترافیک معتبر نیست")]
        AddressIsNotValidTraffic = 14,

        [Description("رمز عبور خالی است")]
        PasswordEmpty = 15,

        [Description("پیام دریافتی معتبر نمی باشد")]
        MessageIsNotValid = 16,

        [Description("شماره گیرنده امکان دریافت پیام تبلیغاتی ندارد")]
        ReceiverNumberNotReceiveAdvertisingSms = 17,

        [Description("شماره فرستنده خالی است")]
        SenderNumberEmpty = 18,

        [Description("نام کاربری پنل پیامک خالی است")]
        UsernameEmpty = 19,

        [Description("خطای ناشناخته")]
        UnknownError = 20,

        [Description("در انتظار ارسال")]
        WaitingForSend = 21,

        [Description("اعتبار پیامک به پایان رسیده")]
        AccountCreditLow = 22,
    }
    
    public enum MediaType : byte
    {
        [Description("jpg")]
        jpg = 1,
        [Description("mp4")]
        mp4 = 2,
        [Description("zip")]
        zip = 3,
        [Description("xls")]
        xls = 4,
        [Description("xlsx")]
        xlsx = 5,
        [Description("pdf")]
        pdf = 6,
        [Description("doc")]
        doc = 7,
        [Description("docx")]
        docx = 8,
        [Description("jpeg")]
        jpeg = 9,
        [Description("bmp")]
        bmp = 10,
        [Description("png")]
        png = 11,
        [Description("gif")]
        gif = 12,
        [Description("txt")]
        txt = 13,
        [Description("html")]
        html = 14,
        [Description("htm")]
        htm = 15,
        [Description("css")]
        css = 16,
        [Description("7zip")]
        sevenzip = 17,
        [Description("mp3")]
        mp3 = 18,
        [Description("ogg")]
        ogg = 19,
        [Description("wav")]
        wav = 20,
        [Description("wma")]
        wma = 21,
        [Description("7z")]
        sevenz = 22,
        [Description("pps")]
        pps = 23,
        [Description("ppt")]
        ppt = 24,
        [Description("pptx")]
        pptx = 25,
        [Description("xlr")]
        xlr = 26,
        [Description("ods")]
        ods = 27,
        [Description("odp")]
        odp = 28,
        [Description("3gp")]
        threegp = 29,
        [Description("avi")]
        avi = 30,
        [Description("flv")]
        flv = 31,
        [Description("h264")]
        h264 = 32,
        [Description("mkv")]
        mkv = 33,
        [Description("mov")]
        mov = 34,
        [Description("mpg")]
        mpg = 35,
        [Description("mpeg")]
        mpeg = 36,
        [Description("swf")]
        swf = 37,
        [Description("wmv")]
        wmv = 38,
        [Description("odt")]
        odt = 39,
        [Description("wks")]
        wks = 40,
        [Description("wps")]
        wps = 41,
        [Description("rar")]
        rar = 42
    }

    public enum RolesKey : byte
    {
       
    }

    public enum NotificationType { LIKE, UNLIKE }
}
