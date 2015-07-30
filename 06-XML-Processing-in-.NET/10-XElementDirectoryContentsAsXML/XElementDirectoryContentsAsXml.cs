namespace _10_XElementDirectoryContentsAsXML
{
    using System;
    using System.IO;
    using System.Xml.Linq;

    public class XElementDirectoryContentsAsXml
    {
        public static void Main()
        {
            var rootDirectory = Directory.GetParent("../../../");
            XElement directoriesAndFiles = new XElement("root-dir");
            directoriesAndFiles.SetAttributeValue("path", rootDirectory.FullName);

            TraverseDirectoriesAndFiles(rootDirectory, directoriesAndFiles);
            directoriesAndFiles.Save("../../../10-DirectoriesAndFiles.xml");
            Console.WriteLine("Successfully exported xml file. " +
                "Look at the solution main folder for file named 10-DirectoriesAndFiles.xml");
        }

        public static void TraverseDirectoriesAndFiles(DirectoryInfo rootDirectory, XElement directoriesAndFiles)
        {
            var directories = Directory.GetDirectories(rootDirectory.FullName);

            foreach (var directory in directories)
            {
                DirectoryInfo innerDirectory = new DirectoryInfo(directory);
                XElement newDirectory = new XElement("dir");
                newDirectory.SetAttributeValue("name", innerDirectory.Name);
                
                var files = innerDirectory.GetFiles();
                foreach (var fileInfo in files)
                {
                    XElement newFile = new XElement("file");
                    newFile.SetAttributeValue("name", fileInfo.Name);
                    newDirectory.Add(newFile);
                }

                directoriesAndFiles.Add(newDirectory);
                TraverseDirectoriesAndFiles(innerDirectory, directoriesAndFiles);
            }
        }   
    }
}
