using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data.Models;
using Common.Data.Models.Umms;
using DbStationary.Models;

namespace DbStationary.Data
{
    public class SQLiteMaps
    {
		public static UserModel UserModelMap(SQLiteDataReaderEx reader)
		{
			return new UserModel
			{
				Login = reader.GetString("Login"),
				Password = reader.GetString("Password"),
				RoleCode = reader.GetString("RoleCode"),
			};
		}

        public static CaseModel CaseModelMap(SQLiteDataReaderEx reader)
        {
            var result =  new CaseModel
            {
                Id = reader.GetInt("ID"),
                Comments = reader.GetStringNullable("Comments"),
                ImportedDate = reader.GetDateTime("ImportedDate"),
                ExportedDate = reader.GetDateTime("ExportedDate"),
                Status = reader.GetInt("Status")
            };


            var caseGuid = reader.GetString("CaseGUID");
            result.CaseGUID = Guid.Parse(caseGuid);
            return result;
        }

        public static DossierModel DossierModelMap(SQLiteDataReaderEx reader)
        {
            var result = new DossierModel
            {
                Id = reader.GetInt("ID"),
                Comments = reader.GetStringNullable("Comments"),
            };

            var status = reader.GetIntNullable("ScenarioStatus");
            result.Status = status.HasValue ? status.Value : 0;

            var scenarioNumber = reader.GetIntNullable("ScenarioNumber");
            result.ScenarioNumber = scenarioNumber.HasValue ? scenarioNumber.Value : 0;

            return result;
        }

        public static UmmsPersonModel PersonModelMap(SQLiteDataReaderEx reader)
        {
            return new UmmsPersonModel
            {
                Id = reader.GetInt("ID"),
                PersonId = reader.GetStringNullable("UmmsID"),
                Name = reader.GetStringNullable("Name"),
                DateOfBirth = reader.GetStringNullable("DateOfBirth"),
                DateOfDeath = reader.GetStringNullable("DateOfDeath"),
                PlaceOfBirth = reader.GetStringNullable("PlaceOfBirth"),
                LastIssuedDocument = reader.GetStringNullable("LastIssuedDocument"),
                HidingAttribute = reader.GetBoolean("HidingAttribute"),
                Created = reader.GetDateTime("Created")
            };
        }

        public static UmmsDocumentModel DocumentModelMap(SQLiteDataReaderEx reader)
        {
            return new UmmsDocumentModel
            {
                Id = reader.GetInt("ID"),
                Date = reader.GetStringNullable("Date"),
                Type = reader.GetStringNullable("Type"),
                Series = reader.GetStringNullable("Series"),
                Number = reader.GetStringNullable("Number"),
                Issuer = reader.GetStringNullable("Issuer"),
                Reason = reader.GetStringNullable("Reason"),
                IsUpToDate = reader.GetStringNullable("IsUpToDate"),
                FirstName = reader.GetStringNullable("FirstName"),
                MiddleName = reader.GetStringNullable("MiddleName"),
                LastName = reader.GetStringNullable("LastName"),
                Gender = reader.GetStringNullable("Gender"),
                Created = reader.GetDateTime("Created")
            };
        }

        public static DocumentXmlModel DocumentXmlModelMap(SQLiteDataReaderEx reader)
        {
            return new DocumentXmlModel
            {
                Id = reader.GetInt("ID"),
                DataXml = reader.GetStringNullable("DataXml"),
                Created = reader.GetDateTime("Created")
            };
        }
        public static AddressXmlModel AddressXmlModelMap(SQLiteDataReaderEx reader)
        {
            return new AddressXmlModel
            {
                Id = reader.GetInt("ID"),
                DataXml = reader.GetStringNullable("DataXml"),
                Created = reader.GetDateTime("Created")
            };
        }
        public static CaseXmlModel CaseXmlModelMap(SQLiteDataReaderEx reader)
        {
            return new CaseXmlModel
            {
                Id = reader.GetInt("ID"),
                DataXml = reader.GetStringNullable("DataXml"),
                Created = reader.GetDateTime("Created")
            };
        }

        public static DossierScenarioXmlModel DossierScenarioXmlMap(SQLiteDataReaderEx reader)
        {
            return new DossierScenarioXmlModel
            {
                ScenarioXml = reader.GetStringNullable("ScenarioXml")
            };
        }
    }
}
