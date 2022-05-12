using System.Text;

public static class FileConversion
{
    public static string AddFileExtension(string base64String)
    {
    var data = base64String.Substring(0, 5);

    switch (data.ToUpper())
     {
         case "IVBOR":
            return "data:image/png;base64," + base64String;
         case "/9J/4":
             return "data:image/png;base64," + base64String;
         default:
            return string.Empty;
     }
    }
    public static string BinToBase64String(byte[] picture)
    {
        return Convert.ToBase64String(picture);
    }
    }
     