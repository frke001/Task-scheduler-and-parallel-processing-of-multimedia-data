using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DokanNet;

namespace KorisnickiFajlSistem
{

    internal class UserspaceFileSistem : IDokanOperations 
    {
        public class File
        {
            private byte[] fileData;
            private string fileName;
            private DateTime createdTime;

            public File(string name, DateTime created)
            {
                fileData = Array.Empty<byte>();
                fileName = name;
                createdTime = created;
            }

            public byte[] FileData 
            { 
                get 
                { 
                    return fileData; 
                } 
                set 
                { 
                    fileData = value; 
                }
            }

            public string FileName 
            { 
                get 
                { 
                    return fileName; 
                } 
                set 
                { fileName = value; 
                } 
            }

            public DateTime CreatedTime 
            { 
                get 
                { 
                    return createdTime; 
                } 
                set 
                {
                    createdTime = value; 
                } 
            }
        }

        private readonly Dictionary<string, File> inputFiles = new(); // saving mappings for input files
        private readonly Dictionary<string, File> outputFiles = new(); // saving mappings for output files

        private readonly static int DiskCapacity = 1024 * 1024 * 1024;
        private readonly int totalNumberOfBytes = DiskCapacity;
        private int totalNumberOfFreeBytes = DiskCapacity/2;
        private static readonly string INPUT = "input";
        private static readonly string OUTPUT = "output";

        public void Cleanup(string fileName, IDokanFileInfo info) { }

        public void CloseFile(string fileName, IDokanFileInfo info) { }

        public NtStatus CreateFile(string fileName, DokanNet.FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, IDokanFileInfo info)
        {
            if (mode == FileMode.CreateNew)
            {
                if (fileName.StartsWith(Path.DirectorySeparatorChar + INPUT + Path.DirectorySeparatorChar) && !inputFiles.ContainsKey(fileName))
                    inputFiles.Add(fileName, new File(fileName, DateTime.Now));
                else if (fileName.StartsWith(Path.DirectorySeparatorChar + OUTPUT + Path.DirectorySeparatorChar) && !outputFiles.ContainsKey(fileName))
                    outputFiles.Add(fileName, new File(fileName, DateTime.Now));
            }
            return NtStatus.Success;
        }

        public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public NtStatus DeleteFile(string fileName, IDokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, IDokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, IDokanFileInfo info)
        {
            files = new List<FileInformation>();
            if (fileName == Path.DirectorySeparatorChar.ToString())
            {
                files.Add(new FileInformation() 
                {
                    Attributes = FileAttributes.Directory,
                    FileName = INPUT
                });
                files.Add(new FileInformation()
                {
                    Attributes = FileAttributes.Directory,
                    FileName = OUTPUT
                });
            }
            else if (fileName.StartsWith(Path.DirectorySeparatorChar + INPUT))
            {
                foreach (var file in inputFiles.Values)
                {
                    files.Add(new FileInformation()
                    {
                        FileName = Path.GetFileName(file.FileName),
                        Length = file.FileData.Length,
                        Attributes = FileAttributes.Normal,
                        CreationTime = file.CreatedTime
                    });
                }
            }
            else if (fileName.StartsWith(Path.DirectorySeparatorChar + OUTPUT))
            {
                foreach (var file in outputFiles.Values)
                {
                    files.Add(new FileInformation()
                    {
                        FileName = Path.GetFileName(file.FileName),
                        Length = file.FileData.Length,
                        Attributes = FileAttributes.Normal,
                        CreationTime = file.CreatedTime
                    });
                }
            }
            return NtStatus.Success;
        }

        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info)
        {
            streams = Array.Empty<FileInformation>();
            return NtStatus.NotImplemented;
        }

        public NtStatus FlushFileBuffers(string fileName, IDokanFileInfo info)
        {
            return NtStatus.Success;
        }

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info)
        {
            totalNumberOfFreeBytes = this.totalNumberOfFreeBytes;
            totalNumberOfBytes = this.totalNumberOfBytes;
            freeBytesAvailable = this.totalNumberOfFreeBytes;
            return NtStatus.Success;
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info)
        {
            if (fileName == Path.DirectorySeparatorChar.ToString())
            {
                fileInfo = new()
                {
                    FileName = fileName,
                    Attributes = FileAttributes.Directory
                };
            }
            else if (fileName == Path.DirectorySeparatorChar + INPUT)
            {
                fileInfo = new()
                {
                    FileName = INPUT,
                    Attributes = FileAttributes.Directory
                };
            }
            else if (fileName == Path.DirectorySeparatorChar + OUTPUT)
            {
                fileInfo = new()
                {
                    FileName = OUTPUT,
                    Attributes = FileAttributes.Directory,

                };
            }
            else if (fileName.StartsWith(Path.DirectorySeparatorChar + INPUT) && inputFiles.ContainsKey(fileName))
            {
                fileInfo = new()
                {
                    FileName = fileName,
                    Length = inputFiles[fileName].FileData.Length,
                    Attributes = FileAttributes.Normal,
                    CreationTime = inputFiles[fileName].CreatedTime
                };
            }
            else if (fileName.StartsWith(Path.DirectorySeparatorChar + OUTPUT) && outputFiles.ContainsKey(fileName))
            {
                fileInfo = new()
                {
                    FileName = fileName,
                    Length = outputFiles[fileName].FileData.Length,
                    Attributes = FileAttributes.Normal,
                    CreationTime = outputFiles[fileName].CreatedTime
                };
            }
            else
            {
                fileInfo = default;
                return NtStatus.Error;
            }
            return NtStatus.Success;
        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            security = null;
            return NtStatus.Success;
        }

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info)
        {
            volumeLabel = "USFileSystem";
            features = FileSystemFeatures.None;
            fileSystemName = "UserSpaceFS";
            maximumComponentLength = 255;
            return NtStatus.Success;
        }

        public NtStatus LockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            return NtStatus.Error;
        }

        public NtStatus MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
        {
            File? file = null;
            if (fileName.StartsWith(Path.DirectorySeparatorChar + INPUT))
                file = inputFiles[fileName];
            else if (fileName.StartsWith(Path.DirectorySeparatorChar + OUTPUT))
                file = outputFiles[fileName];
            file?.FileData.Skip((int)offset).Take(buffer.Length).ToArray().CopyTo(buffer, 0);
            int diff = file.FileData.Length - (int)offset;
            bytesRead = buffer.Length > diff ? diff : buffer.Length;
            return NtStatus.Success;
        }

        public NtStatus SetAllocationSize(string fileName, long length, IDokanFileInfo info)
        {
            return NtStatus.Error;
        }

        public NtStatus SetEndOfFile(string fileName, long length, IDokanFileInfo info)
        {
            return NtStatus.Error;
        }

        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info)
        {
            return NtStatus.Success;
        }

        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            return NtStatus.Error;
        }

        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info)
        {
            return NtStatus.Error;
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            return NtStatus.Error;
        }

        public NtStatus Unmounted(IDokanFileInfo info)
        {
            return NtStatus.Success;
        }

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
        {
            File? file = null;
            if (fileName.StartsWith(Path.DirectorySeparatorChar + INPUT))
            {
                if (!inputFiles.ContainsKey(fileName))
                    inputFiles.Add(fileName, new File(fileName, DateTime.Now));
                file = inputFiles[fileName];
            }
            else if (fileName.StartsWith(Path.DirectorySeparatorChar + OUTPUT))
            {
                if (!outputFiles.ContainsKey(fileName))
                    outputFiles.Add(fileName, new File(fileName, DateTime.Now));
                file = outputFiles[fileName];
            }
            if (info.WriteToEndOfFile) // apend
            {
                file.FileData = file.FileData.Concat(buffer).ToArray(); // appended the content
                bytesWritten = buffer.Length;
            }
            else // rewrite
            {
                int difference = file.FileData.Length - (int)offset;
                totalNumberOfFreeBytes += difference;
                file.FileData = file.FileData.Take((int)offset).Concat(buffer).ToArray();
                bytesWritten = buffer.Length;
            }
            totalNumberOfFreeBytes -= bytesWritten;
            return NtStatus.Success;
        }

        public NtStatus Mounted(string mountPoint, IDokanFileInfo info)
        {
            return DokanResult.Success;
        }


    }
}
