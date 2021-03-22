using Newtonsoft.Json.Linq;

[System.Serializable]
public class signInInfo
{
    public string cellphone;
    public string pwd;

    public signInInfo()
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
