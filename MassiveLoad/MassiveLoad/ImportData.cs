using MassiveLoad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaludGuruProfile;
using SaludGuruProfile.Manager.Models.Profile;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models;
using System.IO;

namespace MassiveLoad
{
    public class ImportData
    {
        StringBuilder sb = new StringBuilder();

        public void processFile(string FilePath, string LogFile)
        {
            //get excel rows
            LinqToExcel.ExcelQueryFactory XlsInfo = new LinqToExcel.ExcelQueryFactory(FilePath);

            List<ExcelModel> oAptToProcess =
                (from x in XlsInfo.Worksheet<ExcelModel>(0)
                 select x).ToList();

            //Create Info Profile
            Console.Write("Cargando...");
            createProfile(oAptToProcess, LogFile);
        }

        public void createProfile(List<ExcelModel> oProfToProcess, string LogFile)
        {
            //process create profile
            oProfToProcess.All(aProf =>
            {
                try
                {
                    #region Create Profile

                    ProfileModel CurrentProfile = GetProfileInfoModel(aProf);
                    CurrentProfile.ProfilePublicId = SaludGuruProfile.Manager.Controller.Profile.UpsertProfileInfo(CurrentProfile);

                    #endregion

                    #region Insert log file Profile
                    using (StreamWriter file = new System.IO.StreamWriter(LogFile, true))
                    {
                        file.WriteLine("Ingresando Perfil: " + CurrentProfile.Name + CurrentProfile.LastName);
                    }
                    #endregion

                    createOffice(aProf, CurrentProfile.ProfilePublicId, LogFile);

                    createSpecialtyToProfile(aProf, CurrentProfile.ProfilePublicId, LogFile, CurrentProfile);

                    createInsuranceToProfile(aProf, CurrentProfile.ProfilePublicId, aProf.Seguro1, LogFile, CurrentProfile);

                    if (!string.IsNullOrEmpty(aProf.Seguro2))
                    {
                        createInsuranceToProfile(aProf, CurrentProfile.ProfilePublicId, aProf.Seguro2, LogFile, CurrentProfile);
                    }
                }
                catch (Exception err)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFile, true))
                    {
                        file.WriteLine(err.Message + "CreateProfile");
                    }
                    Console.Write(err.Message + "CreateProfile");
                }
                return true;
            });
        }

        public void createOffice(ExcelModel oProfToProcess, string ProfilePublicId, string LogFile)
        {
            //process create office
            string oOfficePublicId = "";

            try
            {
                #region Create Related Office
                
                OfficeModel CurrentOffice = GetOfficeInfoModel(oProfToProcess);
                oOfficePublicId = SaludGuruProfile.Manager.Controller.Office.UpsertOfficeInfo(ProfilePublicId, CurrentOffice);
                
                #region Insert log file Office
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFile, true))
                {
                    file.WriteLine("Ingresando Oficina: " + CurrentOffice.Name + ", Office Id: " + CurrentOffice.OfficePublicId);
                }
                #endregion

                #endregion
            }
            catch (Exception err)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFile, true))
                {
                    file.WriteLine(err.Message + "CreateOffice");
                }
                Console.Write(err.Message + "CreateOffice");
            }
        }

        public void createSpecialtyToProfile(ExcelModel oProfToProcess, string ProfilePublicId, string LogFile, ProfileModel CurrentProfile)
        {
            //process create related specialty
            try
            {
                //get profile info
                CurrentProfile.RelatedSpecialty = new List<SpecialtyModel>();
                List<SpecialtyModel> CurrentSpecialty = SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(oProfToProcess.Especialidad);

                #region Create Specialty to Profile
                int aCategoryId = CurrentSpecialty.Select(x => x.CategoryId).FirstOrDefault();
                string aName = CurrentSpecialty.Select(x => x.Name).FirstOrDefault();
                CurrentProfile.LastModify = DateTime.Now;

                CurrentProfile.DefaultSpecialty = new SpecialtyModel();
                CurrentProfile.DefaultSpecialty.CategoryId = aCategoryId;
                CurrentProfile.DefaultSpecialty.Name = aName;

                CurrentProfile.RelatedSpecialty.Add(CurrentSpecialty.FirstOrDefault());
                SaludGuruProfile.Manager.Controller.Profile.SpecialtyProfileUpsert(CurrentProfile);

                #region Insert log file Office
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFile, true))
                {
                    file.WriteLine("Ingresando la especialidad: " + CurrentProfile.RelatedSpecialty.Select(x => x.Name).FirstOrDefault().ToString());
                }
                #endregion

                #endregion
            }
            catch (Exception err)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFile, true))
                {
                    file.WriteLine(err.Message + "SpecialtyToProfile");
                }
                Console.Write(err.Message + "SpecialtyToProfile");
            }
        }

        public void createInsuranceToProfile(ExcelModel oProfToProcess, string ProfilePublicId, string aInsurance, string LogFile, ProfileModel CurrentProfile)
        {
            //process create related Insurance
            try
            {
                //get profile info
                CurrentProfile.RelatedInsurance = new List<InsuranceModel>();
                List<InsuranceModel> CurrentInsurance = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(aInsurance);

                #region Create Treatment to Profile
                CurrentProfile.CreateDate = DateTime.Now;

                CurrentInsurance.FirstOrDefault().CategoryId = CurrentInsurance.Select(x => x.CategoryId).FirstOrDefault();
                CurrentProfile.ProfilePublicId = ProfilePublicId;
                CurrentProfile.Name = CurrentInsurance.Select(x => x.Name).FirstOrDefault();

                CurrentProfile.RelatedInsurance.Add(CurrentInsurance.FirstOrDefault());
                SaludGuruProfile.Manager.Controller.Profile.InsuranceProfileUpsert(CurrentProfile);

                #region Insert log file Insurance
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFile, true))
                {
                    file.WriteLine("Ingresando el Seguro: " + CurrentProfile.RelatedInsurance.Select(x => x.Name).FirstOrDefault());
                }
                #endregion

                #endregion
            }
            catch (Exception err)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogFile, true))
                {
                    file.WriteLine(err.Message + "InsuranceToProfile");
                }
                Console.Write(err.Message + "InsuranceToProfile");
            }
        }

        #region private methods
        private ProfileModel GetProfileInfoModel(ExcelModel profileModel)
        {
            ProfileModel oReturn = new ProfileModel()
            {
                Name = profileModel.Nombres,
                LastName = profileModel.Apellidos,
                ProfileStatus = SaludGuruProfile.Manager.Models.enumProfileStatus.Free,
                ProfileType = SaludGuruProfile.Manager.Models.enumProfileType.Doctor,
                CreateDate = DateTime.Now,
                LastModify = DateTime.Now,
                ProfileInfo = new List<ProfileInfoModel>() 
                { 
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.IdentificationType,
                        Value = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.IdentificationNumber,
                        Value = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.Gender,
                        Value = "false",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.Email,
                        Value = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.Website,
                        Value = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.FacebookProfile,
                        Value = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.IsCertified,
                        Value = "false",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.FeaturedProfile,
                        Value = "false",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.AgendaOnline,
                        Value = "false",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.ProfileText,
                        LargeValue = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.Education,
                        LargeValue = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.Certification,
                        LargeValue = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.SalesforceCode,
                        Value = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.KeyWords,
                        LargeValue = "",
                    },
                        new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.Mobile,
                        Value = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.OldProfileId,
                        Value = "",
                    },
                    new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.MarketPlaceSlotDuration,
                        Value = "",
                    },
                        new ProfileInfoModel()
                    {
                        ProfileInfoId = 0,
                        ProfileInfoType = enumProfileInfoType.ShortProfile,
                        LargeValue = "",
                    },
                }
            };
            return oReturn;
        }

        private OfficeModel GetOfficeInfoModel(ExcelModel officeModel)
        {
            string getCity = officeModel.Ciudad == "Medellin" ? "Medellín" : "Ibagué";
            CityModel city = SaludGuruProfile.Manager.Controller.City.CityGetAll().Where(x => x.CityName == getCity).FirstOrDefault();

            OfficeModel oReturn = new OfficeModel()
            {
                CreateDate = DateTime.Now,
                Name = officeModel.Direccion,
                City = city,
                IsDefault = true,
                OfficeInfo = new List<OfficeInfoModel>() 
                { 
                    new OfficeInfoModel()
                    {
                        OfficeInfoId = 0,
                        OfficeInfoType = enumOfficeInfoType.Address,
                        Value = !string.IsNullOrEmpty(officeModel.Direccion.ToString()) ? officeModel.Direccion.ToString() : "",
                    },
                    new OfficeInfoModel()
                    {
                        OfficeInfoId= 0,
                        OfficeInfoType = enumOfficeInfoType.MarketPlaceEnabled,
                        Value = "true",
                    },
                    new OfficeInfoModel()
                    {
                        OfficeInfoId = 0,
                        OfficeInfoType = enumOfficeInfoType.Telephone,
                        Value = !string.IsNullOrEmpty(officeModel.Telefono.ToString()) ? officeModel.Telefono.ToString() : "",
                    },
                    new OfficeInfoModel()
                    {
                        OfficeInfoId = 0,
                        OfficeInfoType = enumOfficeInfoType.Geolocation,
                        Value = "",
                    },
                    new OfficeInfoModel()
                    {
                        OfficeInfoId = 0,
                        OfficeInfoType = enumOfficeInfoType.SlotMinutes,
                        Value = "",
                    },
                }
            };
            return oReturn;
        }
        #endregion
    }
}
