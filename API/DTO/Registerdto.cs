using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO;
//Data transfer object, data transfer between two different layers and also shape the data
public class Registerdto
{
    [Required]
    [MaxLength(100)]
    public required string UserName { get; set; }

    [Required]
    public required string Password { get; set; }

}
