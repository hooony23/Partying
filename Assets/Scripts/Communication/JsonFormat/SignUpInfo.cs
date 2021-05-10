using Newtonsoft.Json.Linq;
namespace Communication.JsonFormat
{

    public class SignUpInfo
    {
        public string nickname;
        public string pwd;
        public string cellphone;
        public string name;

        public SignUpInfo()
        {
            nickname = "hong134";
            pwd = "asdf123$";
            cellphone = "01012341234";
            name = "김성훈";
        }

        public void UpdateInfo(string id, string password, string mobile, string name)
        {
            nickname = id;
            pwd = password;
            cellphone = mobile;
            this.name = name;
        }

    }

}
