using System;
using System.Collections.Generic;

namespace EsportsRoomAttendance.Models
{
    public class User
    {
        public string Uuid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CenterUuid { get; set; }
        public DateTime Birthdate { get; set; }
        public int TimeRemaining { get; set; }
        public string AccountStatus { get; set; }
        public string StudentId { get; set; }
        public string Phone { get; set; }
        public DateTime LastSeen { get; set; }
        public bool Deleted { get; set; }
        public string Notes { get; set; }
        public decimal Balance { get; set; }
        public string GroupUuid { get; set; }
        public DateTime UserGroupMembershipEndDate { get; set; }
        public bool Locked { get; set; }
        public DateTime RegisteredAt { get; set; }
        public decimal PostPayLimit { get; set; }
        public Dictionary<string, UserCustomField> UserCustomFields { get; set; }
        public string PhotoUrl { get; set; }
        public bool HasReminderNote { get; set; }
        public UserGroupMembershipTrial UserGroupMembershipTrial { get; set; }
    }

    public class UserCustomField
    {
        public string FieldUuid { get; set; }
        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public WebAdmin WebAdmin { get; set; }
        public Client Client { get; set; }
        public bool IsDefault { get; set; }
        public string SerializedValue { get; set; }
    }

    public class WebAdmin
    {
        public string Status { get; set; }
        public bool AllowChangeStatus { get; set; }
    }

    public class Client
    {
        public string Status { get; set; }
        public bool AllowChangeStatus { get; set; }
    }

    public class UserGroupMembershipTrial
    {
        public string GroupUuid { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentMethod { get; set; }
    }
}
