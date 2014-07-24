using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Profile
{
    public class ProfileComunicationModel
    {
        public enumProfileInfoType ComunicationType { get; set; }

        public int ProgramTime { get; set; }

        public List<ProfileInfoModel> MessageType { get; set; }

        public static List<enumProfileInfoType> MessageTypeEnabled
        {
            get
            {
                return BackOffice.Models.General.InternalSettings.Instance
                    [BackOffice.Models.General.Constants.C_Settings_RemindersType].Value.
                    Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).
                    Select(x => (enumProfileInfoType)int.Parse(x)).ToList();
            }
        }

        public ProfileComunicationModel(List<ProfileInfoModel> oOriginalInfo, enumProfileInfoType vComunicationType)
        {
            ComunicationType = vComunicationType;
            MessageType = new List<ProfileInfoModel>();
            ProgramTime = 24;

            if (oOriginalInfo != null && oOriginalInfo.Count > 0)
            {
                MessageType = oOriginalInfo;

                ProgramTime = MessageType.Select(x => Convert.ToInt32(x.LargeValue)).DefaultIfEmpty(24).FirstOrDefault();
            }
        }
    }
}
