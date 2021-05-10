using Newtonsoft.Json.Linq;
namespace Communication.JsonFormat
{

    public class SignInInfo
    {
        public string cellphone;
        public string pwd;

        public SignInInfo()
        {
            cellphone = "01012341234";
            pwd = "asdf123$";
        }

        public void UpdateInfo(string cellphone, string pwd)
        {
            this.cellphone = cellphone;
            this.pwd = pwd;
        }
    }


}