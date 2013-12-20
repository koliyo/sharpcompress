﻿
using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpCompress.Archive;
using SharpCompress.Archive.Zip;
using SharpCompress.Common;
using SharpCompress.Writer;

namespace SharpCompress.Test
{
    [TestClass]
    public class ZipArchiveTests : ArchiveTests
    {
        public ZipArchiveTests()
        {
            UseExtensionInsteadOfNameToVerify = true;
        }

        [TestMethod]
        public void Zip_ZipX_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.zipx");
        }


        [TestMethod]
        public void Zip_BZip2_Streamed_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.bzip2.dd.zip");
        }
        [TestMethod]
        public void Zip_BZip2_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.bzip2.zip");
        }
        [TestMethod]
        public void Zip_Deflate_Streamed2_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.deflate.dd-.zip");
        }
        [TestMethod]
        public void Zip_Deflate_Streamed_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.deflate.dd.zip");
        }
        [TestMethod]
        public void Zip_Deflate_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.deflate.zip");
        }

        [TestMethod]
        public void Zip_LZMA_Streamed_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.lzma.dd.zip");
        }
        [TestMethod]
        public void Zip_LZMA_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.lzma.zip");
        }
        [TestMethod]
        public void Zip_PPMd_Streamed_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.ppmd.dd.zip");
        }
        [TestMethod]
        public void Zip_PPMd_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.ppmd.zip");
        }
        [TestMethod]
        public void Zip_None_ArchiveStreamRead()
        {
            ArchiveStreamRead("Zip.none.zip");
        }

        [TestMethod]
        public void Zip_BZip2_Streamed_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.bzip2.dd.zip");
        }
        [TestMethod]
        public void Zip_BZip2_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.bzip2.zip");
        }
        [TestMethod]
        public void Zip_Deflate_Streamed2_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.deflate.dd-.zip");
        }
        [TestMethod]
        public void Zip_Deflate_Streamed_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.deflate.dd.zip");
        }
        [TestMethod]
        public void Zip_Deflate_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.deflate.zip");
        }

        [TestMethod]
        public void Zip_LZMA_Streamed_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.lzma.dd.zip");
        }
        [TestMethod]
        public void Zip_LZMA_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.lzma.zip");
        }
        [TestMethod]
        public void Zip_PPMd_Streamed_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.ppmd.dd.zip");
        }
        [TestMethod]
        public void Zip_PPMd_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.ppmd.zip");
        }
        [TestMethod]
        public void Zip_None_ArchiveFileRead()
        {
            ArchiveFileRead("Zip.none.zip");
        }

        [TestMethod]
        public void Zip_Random_Write_Remove()
        {
            string scratchPath = Path.Combine(SCRATCH_FILES_PATH, "Zip.deflate.mod.zip");
            string unmodified = Path.Combine(TEST_ARCHIVES_PATH, "Zip.deflate.noEmptyDirs.zip");
            string modified = Path.Combine(TEST_ARCHIVES_PATH, "Zip.deflate.mod.zip");

            base.ResetScratch();
            using (var archive = ZipArchive.Open(unmodified))
            {
                var entry = archive.Entries.Single(x => x.FilePath.EndsWith("jpg"));
                archive.RemoveEntry(entry);
                archive.SaveTo(scratchPath, CompressionType.Deflate);
            }
            CompareArchivesByPath(modified, scratchPath);
        }

        [TestMethod]
        public void Zip_Random_Write_Add()
        {
            string jpg = Path.Combine(ORIGINAL_FILES_PATH, "jpg\\test.jpg");
            string scratchPath = Path.Combine(SCRATCH_FILES_PATH, "Zip.deflate.mod.zip");
            string unmodified = Path.Combine(TEST_ARCHIVES_PATH, "Zip.deflate.mod.zip");
            string modified = Path.Combine(TEST_ARCHIVES_PATH, "Zip.deflate.mod2.zip");

            base.ResetScratch();
            using (var archive = ZipArchive.Open(unmodified))
            {
                archive.AddEntry("jpg\\test.jpg", jpg);
                archive.SaveTo(scratchPath, CompressionType.Deflate);
            }
            CompareArchivesByPath(modified, scratchPath);
        }

        [TestMethod]
        public void Zip_Save_Twice()
        {
            string scratchPath1 = Path.Combine(SCRATCH_FILES_PATH, "a.zip");
            string scratchPath2 = Path.Combine(SCRATCH_FILES_PATH, "b.zip");

            ResetScratch();
            using (var arc = ZipArchive.Create())
            {
                string str = "test.txt";
                var source = new MemoryStream(Encoding.UTF8.GetBytes(str));
                arc.AddEntry("test.txt", source, true, source.Length);
                arc.SaveTo(scratchPath1, CompressionType.Deflate);
                arc.SaveTo(scratchPath2, CompressionType.Deflate);
            }

            Assert.AreEqual(new FileInfo(scratchPath1).Length, new FileInfo(scratchPath2).Length);
        }

        [TestMethod]
        public void Zip_Create_New()
        {
            base.ResetScratch();
            foreach (var file in Directory.EnumerateFiles(ORIGINAL_FILES_PATH, "*.*", SearchOption.AllDirectories))
            {
                var newFileName = file.Substring(ORIGINAL_FILES_PATH.Length);
                if (newFileName.StartsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    newFileName = newFileName.Substring(1);
                }
                newFileName = Path.Combine(SCRATCH_FILES_PATH, newFileName);
                var newDir = Path.GetDirectoryName(newFileName);
                if (!Directory.Exists(newDir))
                {
                    Directory.CreateDirectory(newDir);
                }
                File.Copy(file, newFileName);
            }
            string scratchPath = Path.Combine(SCRATCH2_FILES_PATH, "Zip.deflate.noEmptyDirs.zip");
            string unmodified = Path.Combine(TEST_ARCHIVES_PATH, "Zip.deflate.noEmptyDirs.zip");

            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(SCRATCH_FILES_PATH);
                archive.SaveTo(scratchPath, CompressionType.Deflate);
            }
            CompareArchivesByPath(unmodified, scratchPath);
            Directory.Delete(SCRATCH_FILES_PATH, true);
        }

        [TestMethod]
        public void Zip_Create_New_Add_Remove()
        {
            base.ResetScratch();
            foreach (var file in Directory.EnumerateFiles(ORIGINAL_FILES_PATH, "*.*", SearchOption.AllDirectories))
            {
                var newFileName = file.Substring(ORIGINAL_FILES_PATH.Length);
                if (newFileName.StartsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    newFileName = newFileName.Substring(1);
                }
                newFileName = Path.Combine(SCRATCH_FILES_PATH, newFileName);
                var newDir = Path.GetDirectoryName(newFileName);
                if (!Directory.Exists(newDir))
                {
                    Directory.CreateDirectory(newDir);
                }
                File.Copy(file, newFileName);
            }
            string scratchPath = Path.Combine(SCRATCH2_FILES_PATH, "Zip.deflate.noEmptyDirs.zip");

            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(SCRATCH_FILES_PATH);
                archive.RemoveEntry(archive.Entries.Single(x => x.FilePath.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)));
                Assert.IsFalse(archive.Entries.Any(x => x.FilePath.EndsWith("jpg")));
            }
            Directory.Delete(SCRATCH_FILES_PATH, true);
        }

        [TestMethod]
        public void Zip_Deflate_WinzipAES_Read()
        {
            ResetScratch();
            using (var reader = ZipArchive.Open(Path.Combine(TEST_ARCHIVES_PATH, "Zip.deflate.WinzipAES.zip"), "test"))
            {
                foreach (var entry in reader.Entries.Where(x => !x.IsDirectory))
                {
                    entry.WriteToDirectory(SCRATCH_FILES_PATH, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                }
            }
            VerifyFiles();
        }


        [TestMethod]
        public void Zip_BZip2_Pkware_Read()
        {
            ResetScratch();
            using (var reader = ZipArchive.Open(Path.Combine(TEST_ARCHIVES_PATH, "Zip.bzip2.pkware.zip"), "test"))
            {
                foreach (var entry in reader.Entries.Where(x => !x.IsDirectory))
                {
                    entry.WriteToDirectory(SCRATCH_FILES_PATH, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                }
            }
            VerifyFiles();
        }

        [TestMethod]
        public void Zip_Random_Entry_Access()
        {
            string unmodified = Path.Combine(TEST_ARCHIVES_PATH, "Zip.deflate.noEmptyDirs.zip");

            base.ResetScratch();
            ZipArchive a = ZipArchive.Open(unmodified);
            int count = 0;
            foreach (var e in a.Entries)
                count++;

            //Prints 3
            Assert.AreEqual(count, 3);
            a.Dispose();

            a = ZipArchive.Open(unmodified);
            int count2 = 0;

            foreach (var e in a.Entries)
            {
                count2++;

                //Stop at last file
                if (count2 == count)
                {
                    var s = e.OpenEntryStream();
                    s.ReadByte(); //Actually access stream
                    s.Dispose();
                    break;
                }
            }

            int count3 = 0;
            foreach (var e in a.Entries)
                count3++;

            Assert.AreEqual(count3, 3);
        }


        class NonSeekableMemoryStream : MemoryStream
        {
            public override bool CanSeek
            {
                get
                {
                    return false;
                }
            }
        }

        [TestMethod]
        public void TestSharpCompressWithEmptyStream()
        {
            MemoryStream stream = new NonSeekableMemoryStream();

            using (IWriter zipWriter = WriterFactory.Open(stream, ArchiveType.Zip, CompressionType.Deflate))
            {
                zipWriter.Write("foo.txt", new MemoryStream(new byte[0]));
                zipWriter.Write("foo2.txt", new MemoryStream(new byte[10]));
            }

            stream = new MemoryStream(stream.ToArray());
            File.WriteAllBytes("foo.zip", stream.ToArray());

            using (var zipArchive = ZipArchive.Open(stream))
            {
                foreach(var entry in zipArchive.Entries)
                {
                    using (var entryStream = entry.OpenEntryStream())
                    {
                        MemoryStream tempStream = new MemoryStream();
                        const int bufSize = 0x1000;
                        byte[] buf = new byte[bufSize];
                        int bytesRead = 0;
                        while ((bytesRead = entryStream.Read(buf, 0, bufSize)) > 0)
                            tempStream.Write(buf, 0, bytesRead);
                    }
                }
            }
        }
    }
}
