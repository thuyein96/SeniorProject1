using System.ComponentModel.DataAnnotations;

namespace SnowFlake.Dtos;

public class BaseEntity
{
    [Key]
    public string Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}