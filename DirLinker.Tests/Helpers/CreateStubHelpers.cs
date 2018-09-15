using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirLinker.Interfaces;
using Rhino.Mocks;
using System.IO;

namespace DirLinker.Tests.Helpers
{
    public static class CreateStubHelpers
    {
        public static IFolder GetIFolderStub()
        {
            return GetIDirectoryManagerStub(new Char[] { }, 254);
        }

        public static IFolder GetIDirectoryManagerStub(Char[] illegalChars)
        {
            return GetIDirectoryManagerStub(illegalChars, 254);
        }

        public static IFolder GetIDirectoryManagerStub(Int32 maxPath)
        {
            return GetIDirectoryManagerStub(new Char[]{}, maxPath);
        }

        public static IFolder GetIDirectoryManagerStub(Char[] illegalChars, Int32 maxPath)
        {
            IFolder dirManager = MockRepository.GenerateStub<IFolder>();
            dirManager.Stub(d => d.GetIllegalPathChars()).Return(illegalChars);
            dirManager.Stub(d => d.MaxPath()).Return(maxPath);
            return dirManager;
        }

        public static IFile GetIFileStub(String fileName, String path)
        {
            return GetIFileStub(fileName, path, FileAttributes.Normal);
        }

        public static IFile GetIFileStub(String fileName, String path, FileAttributes attr)
        {
            IFile file = MockRepository.GenerateStub<IFile>();

            file.Stub(f => f.FileName).Return(fileName);
            file.Stub(f => f.Folder).Return(path);
            file.Stub(f => f.FullFilePath).Return(Path.Combine(path, fileName));
            file.Stub(f => f.GetAttributes()).Return(attr);

            return file;
        }
    }
}
