using System.Collections.Generic;

public class MultiStringMaster
{
    public string managedString = "";

    public List<part> strings = new List<part>();

    public void Add(string str)
    {
        var exist = false;
        for (var i = 0; i < strings.Count; i++)
            if (strings[i].str == str)
            {
                exist = true;
                strings[i].count++;
            }

        if (exist == false)
        {
            var t = new part();
            t.count = 1;
            t.str = str;
            strings.Add(t);
        }

        managedString = "";
        for (var i = 0; i < strings.Count; i++)
            if (strings[i].count == 1)
                managedString += strings[i].str + "\n";
            else
                managedString += strings[i].str + "*" + strings[i].count + "\n";
    }

    public void clear()
    {
        strings.Clear();
        managedString = "";
    }

    public void remove(string str)
    {
        part t = null;
        for (var i = 0; i < strings.Count; i++)
            if (strings[i].str.Replace(str, "miaowu") != strings[i].str)
                t = strings[i];
        if (t != null)
        {
            if (t.count == 1)
                strings.Remove(t);
            else
                t.count--;
        }

        managedString = "";
        for (var i = 0; i < strings.Count; i++)
            if (strings[i].count == 1)
                managedString += strings[i].str + "\n";
            else
                managedString += strings[i].str + "*" + strings[i].count + "\n";
    }

    public class part
    {
        public int count;
        public string str;
    }
}