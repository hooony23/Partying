using Newtonsoft.Json.Linq;


[System.Serializable]
public class signUpInfo
{
    public string nickname;
    public string pwd;
    public string cellphone;
    public string name;

    public signUpInfo()
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
