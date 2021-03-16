using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CWTDesktopDatabase.Validation;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Models
{
    [MetadataType(typeof(TeamOutOfOfficeItemValidation))]
	public partial class TeamOutOfOfficeItem : CWTBaseModel
    {
        public Team PrimaryTeam { get; set; }

        public bool HasPrimaryTeam { get; set; }

        public TeamOutOfOfficeItemBackupTeam PrimaryBackupTeam { get; set; }
        public TeamOutOfOfficeItemBackupTeam SecondaryBackupTeam { get; set; }
        public TeamOutOfOfficeItemBackupTeam TertiaryBackupTeam { get; set; }
    }

    public class TeamOutOfOfficeItemBackupTeam
    {
        public int? TeamId { get; set; }
        public string TeamName { get; set; }
    }

	public partial class TeamOutOfOfficeItemExport
	{
		public string TeamOutOfOfficeGroupName { get; set; }
		public string ClientSubUnitGuid { get; set; }
		public string ClientSubUnitName { get; set; }
		public int? PrimaryTeamId { get; set; }
		public string PrimaryTeamName { get; set; }
		public int? PrimaryBackupTeamId { get; set; }
		public string PrimaryBackupTeamName { get; set; }
		public int? SecondaryBackupTeamId { get; set; }
		public string SecondaryBackupTeamName { get; set; }
		public int? TertiaryBackupTeamId { get; set; }
		public string TertiaryBackupTeamName { get; set; }

		public DateTime? TeamOutOfOfficeGroup_CreationTimestamp { get; set; }
		public string TeamOutOfOfficeGroup_CreationUserIdentifier { get; set; }
		public bool? TeamOutOfOfficeGroup_DeletedFlag { get; set; }
		public DateTime? TeamOutOfOfficeGroup_DeletedDateTime { get; set; }
		public bool? TeamOutOfOfficeGroup_EnabledFlag { get; set; }
		public DateTime? TeamOutOfOfficeGroup_EnabledDate { get; set; }

		public DateTime? TeamOutOfOfficeItem_CreationTimestamp { get; set; }
		public string TeamOutOfOfficeItem_CreationUserIdentifier { get; set; }
	}
}