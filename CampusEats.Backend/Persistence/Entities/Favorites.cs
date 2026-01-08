using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Persistence.Entities;

[Table("favorites")]
public class Favorite
{
    [Column("student_id")]
    public int StudentId { get; set; }

    [Column("menu_item_id")]
    public int MenuItemId { get; set; }
    
    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }
    
    [ForeignKey(nameof(MenuItemId))]
    public MenuItem? MenuItem { get; set; }
}