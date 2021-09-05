using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;
using System.Linq;

namespace tess_main_lib
{
    public static class tess_main_class
    {
        
        public static bool StartsWithUpper(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            char ch = str[0];
            return char.IsUpper(ch);
        }

        public  static String intializeLibatPath(String path,String strCmdText)
        {
            try{
                 foreach (string resource in Assembly.GetExecutingAssembly().GetManifestResourceNames())
    {
        if (resource.EndsWith(".zip"))
        {
           // Console.WriteLine("Found a dll--"+resource);
           var assembly = Assembly.GetExecutingAssembly();
           String path_lib_extraction = path+@"\"+resource;
           using (Stream input = assembly.GetManifestResourceStream(resource))
           using (Stream output = File.Create(path_lib_extraction))
            {
                CopyStream(input, output);
                input.Close();
                output.Close();
                System.Threading.Thread.Sleep(5000);
                //return path_lib_extraction;
                //execure Process here and send Response back
                ZipFile.ExtractToDirectory(path_lib_extraction, path);
                 //strCmdText= path+" "+strCmdText;
                 var startInfo = new ProcessStartInfo();
                //startInfo.WorkingDirectory = path;
                startInfo.FileName = path+"/tesseract.exe";
                startInfo.Arguments = strCmdText;
                 var process = System.Diagnostics.Process.Start(startInfo);
                 process.WaitForExit();
                 string contents = File.ReadAllText(path+@"/txtout.txt");
                clearDirectories(path);
                  return contents;
            }
        }
    }
            }catch(Exception e){
                clearDirectories(path);
                Console.WriteLine(e.ToString());
            return e.Message;
            }
              clearDirectories(path);
            return "";
        }

        public static void CopyStream(Stream input, Stream output)
{
    // Insert null checking here for production
    byte[] buffer = new byte[8192*1000];

    int bytesRead;
    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
    {
        output.Write(buffer, 0, bytesRead);
    }
}
public static void clearDirectories(String path){
    System.IO.DirectoryInfo di = new DirectoryInfo(path);

foreach (FileInfo file in di.GetFiles())
{
    file.Delete(); 
}
foreach (DirectoryInfo dir in di.GetDirectories())
{
    dir.Delete(true); 
}
}

    }
}
