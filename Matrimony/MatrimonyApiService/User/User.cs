﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MatrimonyApiService.Commons;
using MatrimonyApiService.Commons.Enums;
using MatrimonyApiService.Commons.Validations;
using Microsoft.EntityFrameworkCore;

namespace MatrimonyApiService.User;

[Index(nameof(Email), Name = "Email_Ind")]
public class User : BaseEntity
{
    // [Key] public int UserId { get; set; }

    [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    [MaxLength(256)]
    public required string Email { get; set; }

    [MaxLength(50)] public required string FirstName { get; set; }
    [MaxLength(50)] public required string LastName { get; set; }

    [MaxLength(10, ErrorMessage = "Phone number must be of 10 numbers")]
    public required string PhoneNumber { get; set; }

    [ForeignKey("AddressId")] public int AddressId;
    public Address.Address? Address;
    public bool IsVerified { get; set; }
    public required byte[] Password { get; set; }
    public required byte[] HashKey { get; set; }
    public int LoginAttempts { get; set; }

    [Required] [EnumTypeValidation(typeof(Role))] [AllowedValues(["User", "Admin"], ErrorMessage = "Invalid Role")]
    public string Role { get; set; } = Commons.Enums.Role.User.ToString();

    public IEnumerable<Message.Message>? MessagesSent { get; set; }
    public IEnumerable<Message.Message>? MessagesReceived { get; set; }
    public IEnumerable<ProfileView.ProfileView>? Views { get; set; }
}