using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;

public class Gsmarena
{
    public static DeviceInfo Parse(string page)
    {
        DeviceInfo info = new DeviceInfo();
        info.memory = FindRawMember(page, "internalmemory");
        info.chipset = FindRawMember(page, "chipset");
        info.cpu = FindRawMember(page, "cpu");
        info.gpu = FindRawMember(page, "gpu");
        info.launch = FindRawMember(page, "year");
        info.price = FindRawMember(page, "price");
        info.android = FindRawMember(page, "os");

        // 后处理1：内存
        info.memory = PostProcess_Memory(info.memory);

        // 后处理2：CPU数目 & 主频
        PostProcess_Cpu(info.cpu, out string cpu_count, out string cpu_freq);
        info.cpu_count = cpu_count;
        info.cpu_freq = cpu_freq;

        // 后处理3：Price
        info.price = PostProcess_Price(info.price);

        return info;
    }

    private static string FindRawMember(string page, string member)
    {
        string ToFind = string.Format($@"data-spec=""{member}"">");
        int index1 = page.IndexOf(ToFind);
        if (index1 >= 0)
        {
            int index2 = page.IndexOf(@"</td>", index1);
            if (index1 < 0) throw new Exception("Parse Error !");
            return page.Substring(index1 + ToFind.Length, index2 - index1 - ToFind.Length);
        }
        else
        {
            return "error";
        }
    }

    private static string PostProcess_Memory(string memory)
    {
        string[] mems = memory.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        List<string> ll = new List<string>();
        foreach (string mem in mems)
        {
            string[] ss = mem.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length < 3) throw new Exception("Parse Error ! Memory.");
            if (!ll.Contains(ss[1]))
                ll.Add(ss[1]);
        }

        string result = "";
        foreach (string l in ll)
            result += l + '/';

        if (result.EndsWith('/'))
            result = result[0..^1];//Substring(0, result.Length - 1);
        return result;
    }

    private static void PostProcess_Cpu(string cpu, out string cpu_count, out string cpu_freq)
    {
        int index1 = cpu.IndexOf("(");
        if (index1 >= 0)
        {
            int index2 = cpu.IndexOf("GHz", index1);
            if (index2 < 0) throw new Exception("Parse Error ! CPU.");
            string toSplit = cpu.Substring(index1 + 1, index2 - index1 + 3 - 1);
            if (toSplit.Contains('x'))
            {
                string[] ss = toSplit.Split(new char[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length != 2) throw new Exception("Parse Error ! CPU.");
                cpu_count = ss[0];
                cpu_freq = ss[1];
            }
            else
            {
                cpu_count = "1";
                cpu_freq = toSplit;
            }
        }
        else
        {
            index1 = cpu.IndexOf(" GHz");
            int indexStart = index1 - 1, index2 = -1;
            for (int i = indexStart; i >= 0; i--)
            {
                if (cpu[i] == ' ')
                {
                    index2 = i;
                    break;
                }
            }
            if (index2 != -1)
            {
                cpu_count = "1";
                cpu_freq = cpu.Substring(index2 + 1, index1 - index2 - 1 + 4);
            }
            else
            {
                cpu_count = "error";
                cpu_freq = "error";
            }
        }
    }

    private static string PostProcess_Price(string price)
    {
        if (price.StartsWith("<a"))
        {
            int index1 = price.IndexOf(">");
            if (index1 < 0) throw new Exception("Parse Error ! Price.");
            int index2 = price.IndexOf("</a>", index1);
            if (index2 < 0) throw new Exception("Parse Error ! Price.");
            price = price.Substring(index1 + 1, index2 - index1 - 1);
            price = HttpUtility.HtmlDecode(price);
        }
        return price;
    }
}
