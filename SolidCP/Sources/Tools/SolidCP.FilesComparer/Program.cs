// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.FilesComparer
{
    class Program
    {
        static string FilesToExclude;

        static void Main(string[] args)
        {
            if (args.Length < 3 && args.Length > 4)
            {
                Console.WriteLine("Usage: Diff.exe [sourceDir] [targetDir] [resultDir] [/ex:fileToExclude.ext,fileToExclude.ext]");
                Console.WriteLine("Example: Diff.exe c:\\SCP1 c:\\SCP2 c:\\result /ex:Default.aspx,Error.htm");
                Console.WriteLine("NOTE: Please make sure all parameters containg spaces are enclosed in quotes.");
                return;
            }
            try
            {
                //
                if (args.Length == 4)
                    FilesToExclude = args[3];
                //
                CreateComparison(args[0], args[1], args[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void CreateComparison(string sourcePath, string targetPath, string pathToSave)
        {

            DeleteDir(pathToSave, false);

            string[] sourceFiles = GetFiles(sourcePath);
            string[] targetFiles = GetFiles(targetPath);

            if (sourcePath.EndsWith(@"\"))
                sourcePath = sourcePath.Substring(0, sourcePath.Length - 1);

            if (targetPath.EndsWith(@"\"))
                targetPath = targetPath.Substring(0, targetPath.Length - 1);

            int sIndexOf = sourcePath.Length;
            int tIndexOf = targetPath.Length;

            List<string> filesA = new List<string>();
            List<string> filesAlower = new List<string>();
            List<string> filesB = new List<string>();
            List<string> filesBlower = new List<string>();
            List<string> excludes = new List<string>();

            excludes.Add("bin");
            excludes.Add("setup");

            string file;

            foreach (string sourceFile in sourceFiles)
            {
                file = sourceFile.Substring(sIndexOf);
                if (file.StartsWith("\\"))
                    file = file.TrimStart('\\');
                filesA.Add(file);
                filesAlower.Add(file.ToLower());
            }

            foreach (string targetFile in targetFiles)
            {
                file = targetFile.Substring(tIndexOf);
                if (file.StartsWith("\\"))
                    file = file.TrimStart('\\');
                filesB.Add(file);
                filesBlower.Add(file.ToLower());
            }

            //files to delete 
            foreach (string fileA in filesA)
            {
                file = fileA.ToLower();
                if (file.StartsWith("bin\\") || file.StartsWith("setup\\") || file.EndsWith("sitesettings.config"))
                {
                    continue;
                }

                if (filesBlower.IndexOf(file) == -1)
                    excludes.Add(fileA);
            }

            //files to copy
            foreach (string fileB in filesB)
            {
                file = fileB.ToLower();
                if (file.EndsWith("sitesettings.config"))
                    continue;

                if (file.StartsWith("bin\\") || file.StartsWith("setup\\"))
                {
                    //copy all new files from bin and setup folders
                    CopyFile(targetPath, fileB, pathToSave);
                    continue;
                }

                //try to find a new file in the list of old files
                int index = filesAlower.IndexOf(file);
                if (index == -1)
                {
                    //old files do not contain new file - we need to copy a new file
                    CopyFile(targetPath, fileB, pathToSave);
                }
                else
                {
                    //old files contain the same file - we need to compare files
                    string oldFile = Path.Combine(sourcePath, filesA[index]);
                    string newFile = Path.Combine(targetPath, fileB);
                    if (Diff(oldFile, newFile))
                    {
                        //files are not equal - we need to delete old file and copy a new file
                        excludes.Add(filesA[index]);
                        CopyFile(targetPath, fileB, pathToSave);
                    }
                }
            }

            string deleteFile = Path.Combine(pathToSave, "setup\\delete.txt");
            WriteFilesToDelete(deleteFile, excludes);

        }

        private static void DeleteDir(string path, bool delRoot)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    DeleteDir(dir, true);
                }

                if (delRoot)
                {
                    Directory.Delete(path);
                }

            }

        }

        private static bool Diff(string oldFile, string newFile)
        {
            FileInfo fi1 = new FileInfo(oldFile);
            FileInfo fi2 = new FileInfo(newFile);
            if (fi1.Length != fi2.Length)
                return true;

            UInt32 hash1 = GetCRC32(oldFile);
            UInt32 hash2 = GetCRC32(newFile);

            return (hash1 != hash2);

        }

        private static UInt32 GetCRC32(string oldFile)
        {
            CRC32 c = new CRC32();
            UInt32 crc = 0;

            using (FileStream f = new FileStream(oldFile, FileMode.Open, FileAccess.Read, FileShare.Read, 8192))
            {
                crc = c.GetCrc32(f);
            }
            return crc;
        }

        private static void CopyFile(string sourceDir, string fileName, string targetDir)
        {
            string destFile = Path.Combine(targetDir, fileName);
            string dir = Path.GetDirectoryName(destFile);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string sourceFile = Path.Combine(sourceDir, fileName);
            File.Copy(sourceFile, destFile);
        }

        private static void WriteFilesToDelete(string fileName, List<string> list)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                // Write all files received
                foreach (string file in list)
                {
                    sw.WriteLine(file);
                }
            }
        }

        static string[] GetFiles(string path)
        {
            var filesListRaw = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            // See if the switch is specified
            bool useAllFiles = String.IsNullOrEmpty(FilesToExclude);
            // Switch is not empty so do the check
            if (!useAllFiles)
            {
                var resultList = new List<string>();
                //
                foreach (string file in filesListRaw)
                {
                    // Verify to include the file
                    if (FilesToExclude.IndexOf(Path.GetFileName(file), StringComparison.OrdinalIgnoreCase) >= 0)
                        continue;
                    //
                    resultList.Add(file);
                }
                //
                return resultList.ToArray();
            }
            //
            return filesListRaw;
        }
    }
}
