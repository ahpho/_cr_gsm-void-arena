public class DeviceInfo
{
    public string name = "error";
    public string count = "error";
    public string memory = "error";
    public string chipset = "error";
    public string cpu_count = "error";
    public string cpu_freq = "error";
    public string cpu = "error";
    public string gpu = "error";
    public string launch = "error";
    public string price = "error";
    public string android = "error";

    public DeviceInfo()
    {
    }
    public DeviceInfo(string _name, string _count)
    {
        name = _name;
        count = _count;
    }
}
