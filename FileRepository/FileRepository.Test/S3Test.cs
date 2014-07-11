using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileRepository.Manager;
using FileRepository.Manager.Models;

namespace FileRepository.Test
{
    [TestClass]
    public class S3Test
    {
        [TestMethod]
        public void StartUploadS3()
        {
            FileRepository.Manager.FileRepository oFileRepositoryInstance = (new FileRepositoryFactory()).GetFileRepository("S3_SaludGuru");
            oFileRepositoryInstance.OperationError += new FileRepository.Manager.FileRepository.DOperationError(OperationError);
            oFileRepositoryInstance.OperationFinish += new FileRepository.Manager.FileRepository.DOperationFinish(OperationFinish);

            oFileRepositoryInstance.CurrentOperations = new System.Collections.Generic.List<FileModel>();

            oFileRepositoryInstance.CurrentOperations.Add(
                new FileModel()
                {
                    FilePathLocalSystem = @"D:\Proyectos\Github\SaludGuru\FileRepository\FileRepository.Test\imgTest.JPG",
                    FilePathRemoteSystem = "NuevaCarpeta/imagen1.jpg",
                    Operation = eOperation.UploadFile
                });

            oFileRepositoryInstance.StartOperation();
            do
            {
                System.Threading.Thread.Sleep(600);
            } while (oFileRepositoryInstance.GetProgressProcess(oFileRepositoryInstance.CurrentOperations[0].InternalFileId) < 100);

        }

        [TestMethod]
        public void StartDeleteS3()
        {
            FileRepository.Manager.FileRepository oFileRepositoryInstance = (new FileRepositoryFactory()).GetFileRepository("S3_SaludGuru");
            oFileRepositoryInstance.OperationError += new FileRepository.Manager.FileRepository.DOperationError(OperationError);
            oFileRepositoryInstance.OperationFinish += new FileRepository.Manager.FileRepository.DOperationFinish(OperationFinish);

            oFileRepositoryInstance.CurrentOperations = new System.Collections.Generic.List<FileModel>();

            oFileRepositoryInstance.CurrentOperations.Add(
                new FileModel()
                {
                    FilePathRemoteSystem = "NuevaCarpeta/imagen1.jpg",
                    Operation = eOperation.DeleteFile
                });

            oFileRepositoryInstance.StartOperation();
            do
            {
                System.Threading.Thread.Sleep(600);
            } while (oFileRepositoryInstance.GetProgressProcess(oFileRepositoryInstance.CurrentOperations[0].InternalFileId) < 100);

        }

        private void OperationFinish(Manager.Models.FileModel FileDescription)
        {
            Assert.AreEqual(100, FileDescription.ProgressProcess);
        }

        private void OperationError(Manager.Models.FileModel FileDescription, Exception Error)
        {
            Assert.AreEqual(0, 1);
        }
    }
}
