using FileRepository.Manager;
using FileRepository.Manager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Image
{
    internal class ImageLoader
    {
        #region Properties

        //File repository Instance
        private FileRepository.Manager.FileRepository FileRepositoryInstance
        {
            get
            {
                if (oFileRepositoryInstance == null)
                {
                    oFileRepositoryInstance = (new FileRepositoryFactory()).GetFileRepository(SaludGuruProfile.Manager.Models.Constants.C_Settings_Image_FileModuleName);
                    oFileRepositoryInstance.OperationError += new FileRepository.Manager.FileRepository.DOperationError(OperationError);
                    oFileRepositoryInstance.OperationFinish += new FileRepository.Manager.FileRepository.DOperationFinish(OperationFinish);
                }
                return oFileRepositoryInstance;
            }
        }
        private FileRepository.Manager.FileRepository oFileRepositoryInstance;

        //remote folder
        public string RemoteFolder { get; set; }

        //files to upload location
        public List<string> FilesToUpload { get; set; }

        //eval end process file
        public bool EndUpload
        {
            get
            {
                if (FileRepositoryInstance.CurrentOperations == null)
                {
                    return false;
                }
                else
                {
                    return !FileRepositoryInstance.CurrentOperations.Any(x => x.ActionResult == enumActionResult.NotStart);
                }
            }
        }

        public List<FileModel> UploadedFiles
        {
            get
            {
                if (FileRepositoryInstance == null)
                    return null;
                else
                    return FileRepositoryInstance.CurrentOperations;
            }
        }

        #endregion

        #region Public Methods

        public void StartUpload()
        {
            //fill file info to load
            oFileRepositoryInstance.CurrentOperations = new List<FileModel>();
            FilesToUpload.All(OriginFile =>
            {
                oFileRepositoryInstance.CurrentOperations.Add(
                    new FileModel()
                    {
                        FilePathLocalSystem = OriginFile,
                        FilePathRemoteSystem = RemoteFolder.TrimEnd('/') + "/" + OriginFile.Substring(OriginFile.LastIndexOf("/"), OriginFile.Length),
                        Operation = enumOperation.UploadFile
                    });

                return true;
            });
            //start load
            oFileRepositoryInstance.StartOperation();
        }

        #endregion

        #region File repository events

        private void OperationFinish(FileModel FileDescription)
        {

        }

        private void OperationError(FileModel FileDescription, Exception Error)
        {

        }

        #endregion
    }
}
